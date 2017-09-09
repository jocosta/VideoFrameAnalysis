using LiveCameraForm.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WinFormAnimation;

namespace LiveCameraForm
{
    public class DisplayEmotion
    {
        private Func<int, int, int> CreateStep = (min, max) =>
        {
            var random = new Random();
            return random.Next(min, max);
        };

        Dictionary<string, PictureBox> emojis = new Dictionary<string, PictureBox>();



        public async Task ShowEmoji(Control control, Rect face, string emocao)
        {
            //PictureBox emoji = null;
            // if (!emojis.TryGetValue(emocao, out emoji))
            //{
            var emoji = new PictureBox();
            emoji.Top = (int)(face.Top);
            emoji.Left = (int)(face.Left + face.Right + 10);

            emoji.BackgroundImageLayout = ImageLayout.Stretch;
            emoji.BackColor = Color.Transparent;
            emoji.Height = 74;
            emoji.Width = 74;

            emojis.Add(emocao, emoji);
            //}


            var x = (int)(face.Location.X + 250);
            var y = (int)(face.Location.Y);

            emoji.Location = new System.Drawing.Point(x, (int)face.Y);



            switch (emocao.ToLower())
            {
                case "desgosto":
                    emoji.Image = EmotionFace.desgosto;
                    break;
                case "desprezo":
                    emoji.Image = EmotionFace.desprezo;
                    break;
                case "felicidade":
                    emoji.Image = EmotionFace.felicidade;
                    break;
                case "medo":
                    emoji.Image = EmotionFace.medo;
                    break;
                case "neutro":
                    emoji.Image = EmotionFace.neutro;
                    break;
                case "raiva":
                    emoji.Image = EmotionFace.raiva;
                    break;
                case "surpresa":
                    emoji.Image = EmotionFace.surpresa;
                    break;
                case "tristeza":
                    emoji.Image = EmotionFace.tristeza;
                    break;
                default:
                    emoji.Image = EmotionFace.neutro;
                    break;
            }

            Task.Run(() => ExecuteSecure(control, () => control.Controls.Add(emoji)));
            //Task.Run(() =>
            //{
            //    ExecuteSecure(control, () => control.Controls.Add(emoji));

            //    new Animator2D(
            //                      new Path2D(new Float2D(CreateStep(emoji.Top, emoji.Top + 20), CreateStep(emoji.Top + 20, emoji.Top + 40)), new Float2D(CreateStep(emoji.Top + 20, emoji.Top + 40), CreateStep(emoji.Top + 20, emoji.Top + 40)), 300)
            //                          .ContinueTo(new Float2D(CreateStep(emoji.Top, emoji.Top + 20), CreateStep(100, 150)), 300)
            //                          .ContinueTo(new Float2D(CreateStep(emoji.Top, emoji.Top + 20), CreateStep(100, 150)), 300))
            //                          .Play(emoji, Animator2D.KnownProperties.Location, new SafeInvoker(() => { ExecuteSecure(control, () => control.Controls.Remove(emoji)); }));
            //});
        }

        private void ExecuteSecure(Control form, Action a)
        {
            if (form.InvokeRequired)
                form.BeginInvoke(a);
            else
                a();
        }
    }
}
