using LiveCameraForm.Images;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveCameraForm
{
    public class DisplayEmotion
    {
        private Func<int, int, int> CreateStep = (min, max) =>
        {
            var random = new Random();
            return random.Next(min, max);
        };

        public async Task ShowEmoji(Control control, double x, double y, string emocao)
        {

            var emoji = new PictureBox();
            emoji.BackgroundImageLayout = ImageLayout.Stretch;
            emoji.BackColor = System.Drawing.Color.Transparent;
            emoji.Height = 74;
            emoji.Width = 74;
            emoji.Location = new System.Drawing.Point((int)x, (int)y);
            emoji.BackgroundImage = GetEmojiBipMap(emocao);

            Task.Run(() => ExecuteSecure(control, () => control.Controls.Add(emoji)));

        }

        public async Task ShowEmoji(DrawingContext drawingContext, Rect face, string emocao)
        {

            //var emoji = new PictureBox();
            //emoji.BackgroundImageLayout = ImageLayout.Stretch;
            //emoji.BackColor = System.Drawing.Color.Transparent;
            //emoji.Height = 74;
            //emoji.Width = 74;
            //emoji.Location = new System.Drawing.Point((int)x, (int)y);
            //emoji.BackgroundImage = GetEmojiBipMap(emocao);

            //Task.Run(() => ExecuteSecure(control, () => control.Controls.Add(emoji)));

            drawingContext.DrawImage(DisplayEmotion.ImageSourceForBitmap(DisplayEmotion.GetEmojiBipMap(emocao)), face);

        }

        private void ExecuteSecure(Control form, Action a)
        {
            if (form.InvokeRequired)
                form.BeginInvoke(a);
            else
                a();
        }

        public static Bitmap GetEmojiBipMap(string emocao)
        {
            switch (emocao.ToLower())
            {
                case "desgosto":
                    return EmotionFace.desgosto;

                case "desprezo":
                    return EmotionFace.desprezo;

                case "felicidade":
                    return EmotionFace.felicidade;

                case "medo":
                    return EmotionFace.medo;

                case "neutro":
                    return EmotionFace.neutro;

                case "raiva":
                    return EmotionFace.raiva;

                case "surpresa":
                    return EmotionFace.surpresa;

                case "tristeza":
                    return EmotionFace.tristeza;

                default:
                    return EmotionFace.neutro;

            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
    }
}
