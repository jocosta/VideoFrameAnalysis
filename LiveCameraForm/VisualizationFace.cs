using Microsoft.ProjectOxford.Common.Contract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveCameraForm
{
    public class VisualizationFace
    {
        private static SolidBrush s_lineBrush = new SolidBrush(Color.Yellow);
        private static Font s_fontface = new Font("Arial", 8, System.Drawing.FontStyle.Regular);

        public static Bitmap DrawFaces(Bitmap baseImage, Microsoft.ProjectOxford.Face.Contract.Face[] faces, EmotionScores[] emotionScores, string[] celebName)
        {
            if (faces == null)
            {
                return baseImage;
            }

            Action<System.Drawing.Graphics, double> drawAction = (drawingContext, annotationScale) =>
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    var face = faces[i];
                    if (face.FaceRectangle == null) { continue; }

                    Rectangle faceRect = new Rectangle(
                       face.FaceRectangle.Left, face.FaceRectangle.Top,
                       face.FaceRectangle.Width, face.FaceRectangle.Height);

                    string text = "";

                    if (face.FaceAttributes != null)
                    {
                        text += Aggregation.SummarizeFaceAttributes(face.FaceAttributes);
                    }

                    faceRect.Inflate((int)(6 * annotationScale), (int)(6 * annotationScale));
                    
                    double lineThickness = 4 * annotationScale;

                    drawingContext.DrawRectangle(new Pen(Color.Yellow, (float)lineThickness), faceRect);
                }
            };
            
            return DrawOverlay(baseImage, drawAction); ;
        }

        private static Bitmap DrawOverlay(Bitmap baseImage, Action<Graphics, double> drawAction)
        {
            double annotationScale = baseImage.Height / 320;

            DrawingVisual visual = new DrawingVisual();
            DrawingContext drawingContext = visual.RenderOpen();

            drawingContext.DrawImage(baseImage, new Rect(0, 0, baseImage.Width, baseImage.Height));

            drawAction(drawingContext, annotationScale);

            drawingContext.Close();

            RenderTargetBitmap outputBitmap = new RenderTargetBitmap(
                baseImage.PixelWidth, baseImage.PixelHeight,
                baseImage.DpiX, baseImage.DpiY, PixelFormats.Pbgra32);

            outputBitmap.Render(visual);

            return outputBitmap;
        }

        /*
         
        double annotationScale = baseImage.PixelHeight / 320;

            DrawingVisual visual = new DrawingVisual();
            DrawingContext drawingContext = visual.RenderOpen();

            drawingContext.DrawImage(baseImage, new Rect(0, 0, baseImage.Width, baseImage.Height));

            drawAction(drawingContext, annotationScale);

            drawingContext.Close();

            RenderTargetBitmap outputBitmap = new RenderTargetBitmap(
                baseImage.PixelWidth, baseImage.PixelHeight,
                baseImage.DpiX, baseImage.DpiY, PixelFormats.Pbgra32);

            outputBitmap.Render(visual);

            return outputBitmap;
         
         */
    }
}
