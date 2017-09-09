using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoFrameAnalyzer;
using System.Windows.Media.Imaging;
using Microsoft.ProjectOxford.Face.Contract;
using System.Configuration;
using OpenCvSharp;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using OpenCvSharp.Extensions;
using Microsoft.ProjectOxford.Common.Contract;
using Newtonsoft.Json;
using System.IO;

namespace LiveCameraForm
{
    public partial class UcCameraEmotion : UserControl
    {
        private EmotionServiceClient _emotionClient = null;
        private FaceServiceClient _faceClient = null;
        private VisionServiceClient _visionClient = null;
        private readonly FrameGrabber<LiveCameraResult> _grabber = null;
        private static readonly ImageEncodingParam[] s_jpegParams = {
            new ImageEncodingParam(ImwriteFlags.JpegQuality, 60)
        };
        private readonly CascadeClassifier _localFaceDetector = new CascadeClassifier();
        private bool _fuseClientRemoteResults;
        private LiveCameraResult _latestResultsToDisplay = null;
        private DateTime _startTime;

        public TimeSpan AnalysisInterval
        {
            get
            {
                return TimeSpan.Parse(ConfigurationManager.AppSettings["AnalysisInterval"]);
            }
        }

        public UcCameraEmotion()
        {

            InitializeComponent();



            BackColor = Color.Black;
            // Create grabber. 
            _grabber = new FrameGrabber<LiveCameraResult>();

            _grabber.NewFrameProvided += (s, e) =>
            {


                var rects = _localFaceDetector.DetectMultiScale(e.Frame.Image);
                // Attach faces to frame. 
                e.Frame.UserData = rects;

                this.Invoke((Action)delegate
                {
                    Visualization.ClearEmojis(imageDrawing);
                    // Display the image in the left pane.
                    imageBase.Image = e.Frame.Image.ToBitmap();
                    // If we're fusing client-side face detection with remote analysis, show the
                    // new frame now with the most recent analysis available. 
                    if (_fuseClientRemoteResults)
                    {
                        imageDrawing.Image = VisualizeResult(e.Frame);
                    }
                });

            };

            // Set up a listener for when the client receives a new result from an API call. 
            _grabber.NewResultAvailable += (s, e) =>
            {
                this.Invoke((Action)delegate
                {
                    Visualization.ClearEmojis(imageDrawing);
                    if (e.TimedOut)
                    {
                        //System.Windows.MessageBox.Show("API call timed out.");
                    }
                    else if (e.Exception != null)
                    {
                        string apiName = "";
                        string message = e.Exception.Message;
                        var faceEx = e.Exception as FaceAPIException;
                        var emotionEx = e.Exception as Microsoft.ProjectOxford.Common.ClientException;
                        var visionEx = e.Exception as Microsoft.ProjectOxford.Vision.ClientException;
                        if (faceEx != null)
                        {
                            apiName = "Face";
                            message = faceEx.ErrorMessage;
                        }
                        else if (emotionEx != null)
                        {
                            apiName = "Emotion";
                            message = emotionEx.Error.Message;
                        }
                        else if (visionEx != null)
                        {
                            apiName = "Computer Vision";
                            message = visionEx.Error.Message;
                        }
                        //System.Windows.MessageBox.Show(string.Format("{0} API call failed on frame {1}. Exception: {2}", apiName, e.Frame.Metadata.Index, message));
                    }
                    else
                    {
                        _latestResultsToDisplay = e.Analysis;

                        // Display the image and visualization in the right pane. 
                        if (!_fuseClientRemoteResults)
                        {
                            imageDrawing.Image = VisualizeResult(e.Frame);
                        }
                    }


                });
            };

            _localFaceDetector.Load("Data/haarcascade_frontalface_alt2.xml");
        }

        private void UcCameraEmotion_Load(object sender, EventArgs e)
        {
            CameraListLoad();

            StartCamera();
        }


        /// <summary> Function which submits a frame to the Face API. </summary>
        /// <param name="frame"> The video frame to submit. </param>
        /// <returns> A <see cref="Task{LiveCameraResult}"/> representing the asynchronous API call,
        ///     and containing the faces returned by the API. </returns>
        private async Task<LiveCameraResult> FacesAnalysisFunction(VideoFrame frame)
        {
            // Encode image. 
            var jpg = frame.Image.ToMemoryStream(".jpg", s_jpegParams);
            // Submit image to API. 
            var attrs = new List<FaceAttributeType> { FaceAttributeType.Age,
                FaceAttributeType.Gender, FaceAttributeType.HeadPose };
            var faces = await _faceClient.DetectAsync(jpg, returnFaceAttributes: attrs);

            return new LiveCameraResult { Faces = faces };
        }

        /// <summary> Function which submits a frame to the Emotion API. </summary>
        /// <param name="frame"> The video frame to submit. </param>
        /// <returns> A <see cref="Task{LiveCameraResult}"/> representing the asynchronous API call,
        ///     and containing the emotions returned by the API. </returns>
        private async Task<LiveCameraResult> EmotionAnalysisFunction(VideoFrame frame)
        {
            // Encode image. 
            var jpg = frame.Image.ToMemoryStream(".jpg", s_jpegParams);
            // Submit image to API. 
            Emotion[] emotions = null;

            // See if we have local face detections for this image.
            var localFaces = (OpenCvSharp.Rect[])frame.UserData;
            if (localFaces == null)
            {
                // If localFaces is null, we're not performing local face detection.
                // Use Cognigitve Services to do the face detection.
                emotions = await _emotionClient.RecognizeAsync(jpg);
            }
            else if (localFaces.Count() > 0)
            {
                // If we have local face detections, we can call the API with them. 
                // First, convert the OpenCvSharp rectangles. 
                var rects = localFaces.Select(
                    f => new Microsoft.ProjectOxford.Common.Rectangle
                    {
                        Left = f.Left,
                        Top = f.Top,
                        Width = f.Width,
                        Height = f.Height
                    });
                emotions = await _emotionClient.RecognizeAsync(jpg, rects.ToArray());
            }
            else
            {
                // Local face detection found no faces; don't call Cognitive Services.
                emotions = new Emotion[0];
            }

            // Output. 
            return new LiveCameraResult
            {
                Faces = emotions.Select(e => CreateFace(e.FaceRectangle)).ToArray(),
                // Extract emotion scores from results. 
                EmotionScores = emotions.Select(e => e.Scores).ToArray()
            };
        }

        /// <summary> Function which submits a frame to the Computer Vision API for celebrity
        ///     detection. </summary>
        /// <param name="frame"> The video frame to submit. </param>
        /// <returns> A <see cref="Task{LiveCameraResult}"/> representing the asynchronous API call,
        ///     and containing the celebrities returned by the API. </returns>
        private async Task<LiveCameraResult> CelebrityAnalysisFunction(VideoFrame frame)
        {
            // Encode image. 
            var jpg = frame.Image.ToMemoryStream(".jpg", s_jpegParams);
            // Submit image to API. 
            var result = await _visionClient.AnalyzeImageInDomainAsync(jpg, "celebrities");

            // Output. 
            var celebs = JsonConvert.DeserializeObject<CelebritiesResult>(result.Result.ToString()).Celebrities;
            return new LiveCameraResult
            {
                // Extract face rectangles from results. 
                Faces = celebs.Select(c => CreateFace(c.FaceRectangle)).ToArray(),
                // Extract celebrity names from results. 
                CelebrityNames = celebs.Select(c => c.Name).ToArray()
            };
        }

        private Face CreateFace(Microsoft.ProjectOxford.Common.Rectangle faceRectangle)
        {
            return new Face
            {
                FaceRectangle = new FaceRectangle
                {
                    Left = faceRectangle.Left,
                    Top = faceRectangle.Top,
                    Width = faceRectangle.Width,
                    Height = faceRectangle.Height
                }
            };
        }

        private Face CreateFace(Microsoft.ProjectOxford.Vision.Contract.FaceRectangle rect)
        {
            return new Face
            {
                FaceRectangle = new FaceRectangle
                {
                    Left = rect.Left,
                    Top = rect.Top,
                    Width = rect.Width,
                    Height = rect.Height
                }
            };
        }

        private Face CreateFace(FaceRectangle rect)
        {
            return new Face
            {
                FaceRectangle = new FaceRectangle
                {
                    Left = rect.Left,
                    Top = rect.Top,
                    Width = rect.Width,
                    Height = rect.Height
                }
            };
        }

        private void MatchAndReplaceFaceRectangles(Face[] faces, OpenCvSharp.Rect[] clientRects)
        {
            // Use a simple heuristic for matching the client-side faces to the faces in the
            // results. Just sort both lists left-to-right, and assume a 1:1 correspondence. 

            // Sort the faces left-to-right. 
            var sortedResultFaces = faces
                .OrderBy(f => f.FaceRectangle.Left + 0.5 * f.FaceRectangle.Width)
                .ToArray();

            // Sort the clientRects left-to-right.
            var sortedClientRects = clientRects
                .OrderBy(r => r.Left + 0.5 * r.Width)
                .ToArray();

            // Assume that the sorted lists now corrrespond directly. We can simply update the
            // FaceRectangles in sortedResultFaces, because they refer to the same underlying
            // objects as the input "faces" array. 
            for (int i = 0; i < Math.Min(faces.Length, clientRects.Length); i++)
            {
                // convert from OpenCvSharp rectangles
                OpenCvSharp.Rect r = sortedClientRects[i];
                sortedResultFaces[i].FaceRectangle = new FaceRectangle { Left = r.Left, Top = r.Top, Width = r.Width, Height = r.Height };
            }
        }

        private async void StartCamera()
        {
            if (!(CameraList.Items.Count > 0))
            {
                System.Windows.MessageBox.Show("No cameras found; cannot start processing");
                return;
            }

            _faceClient = new FaceServiceClient(ConfigurationManager.AppSettings["FaceAPIKey"], ConfigurationManager.AppSettings["FaceAPIHost"]);
            _emotionClient = new EmotionServiceClient(ConfigurationManager.AppSettings["EmotionAPIKey"], ConfigurationManager.AppSettings["EmotionAPIHost"]);
            _visionClient = new VisionServiceClient(ConfigurationManager.AppSettings["VisionAPIKey"], ConfigurationManager.AppSettings["VisionAPIHost"]);


            //Emotion
            _grabber.AnalysisFunction = EmotionAnalysisFunction;
            _fuseClientRemoteResults = true;
            imageDrawing.Visible = true;
            imageBase.Visible = false;


            // How often to analyze. 
            _grabber.TriggerAnalysisOnInterval(AnalysisInterval);

            // Record start time, for auto-stop
            _startTime = DateTime.Now;


            await _grabber.StartProcessingCameraAsync(CameraList.SelectedIndex);
        }


        private void EmotionCamera()
        {
            _faceClient = new FaceServiceClient(ConfigurationManager.AppSettings["FaceAPIKey"], ConfigurationManager.AppSettings["FaceAPIHost"]);
            _emotionClient = new EmotionServiceClient(ConfigurationManager.AppSettings["EmotionAPIKey"], ConfigurationManager.AppSettings["EmotionAPIHost"]);
            _visionClient = new VisionServiceClient(ConfigurationManager.AppSettings["VisionAPIKey"], ConfigurationManager.AppSettings["VisionAPIHost"]);


            //Emotion
            _grabber.AnalysisFunction = EmotionAnalysisFunction;
            _fuseClientRemoteResults = true;
            imageDrawing.Visible = true;
            imageBase.Visible = false;

            StartCamera();
        }


        private void FaceCamera()
        {
            _faceClient = new FaceServiceClient(ConfigurationManager.AppSettings["FaceAPIKey"], ConfigurationManager.AppSettings["FaceAPIHost"]);
            _emotionClient = new EmotionServiceClient(ConfigurationManager.AppSettings["EmotionAPIKey"], ConfigurationManager.AppSettings["EmotionAPIHost"]);
            _visionClient = new VisionServiceClient(ConfigurationManager.AppSettings["VisionAPIKey"], ConfigurationManager.AppSettings["VisionAPIHost"]);


            //Emotion
            _grabber.AnalysisFunction = FacesAnalysisFunction;
            _fuseClientRemoteResults = true;
            imageDrawing.Visible = true;
            imageBase.Visible = false;

            StartCamera();
        }

        private void CameraListLoad()
        {
            int numCameras = _grabber.GetNumCameras();
            if (numCameras == 0)
            {
                System.Windows.MessageBox.Show("No cameras found!");
            }

            CameraList.DataSource = Enumerable.Range(0, numCameras).Select(i => string.Format("Camera {0}", i + 1)).ToList();
            CameraList.SelectedIndex = 0;
        }

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        private Bitmap VisualizeResult(VideoFrame frame)
        {
            BitmapSource visImage = frame.Image.ToBitmapSource();

            var result = _latestResultsToDisplay;

            if (result != null)
            {
                // See if we have local face detections for this image.
                var clientFaces = (OpenCvSharp.Rect[])frame.UserData;
                if (clientFaces != null && result.Faces != null)
                {
                    // If so, then the analysis results might be from an older frame. We need to match
                    // the client-side face detections (computed on this frame) with the analysis
                    // results (computed on the older frame) that we want to display. 
                    MatchAndReplaceFaceRectangles(result.Faces, clientFaces);
                }

                visImage = Visualization.DrawFaces(visImage, result.Faces, result.EmotionScores, result.CelebrityNames, imageDrawing);
                visImage = Visualization.DrawTags(visImage, result.Tags);
            }
            return BitmapFromSource(visImage);
        }



    }
}
