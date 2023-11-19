using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            var img = Cv2.ImRead(openFileDialog1.FileName, LoadMode.GrayScale) ; // 4.JPG pyt.JPG 5.png 30 Stop
            var img_result = Cv2.ImRead(openFileDialog1.FileName, LoadMode.Color); // 4.JPG pyt.JPG 5.png
            
            int work_width, work_height;
            if (img.Width % 2 == 0)
            {
                work_width = img.Width - 1;
            }
            else work_width = img.Width;
            if (img.Height % 2 == 0)
            {
                work_height = img.Height - 1;
            }
            else work_height = img.Height;
            OpenCvSharp.CPlusPlus.Size size = new OpenCvSharp.CPlusPlus.Size(work_width, work_height);
            //img = img.GaussianBlur(size,1);
            /*img = img.Dilate(new Mat());*/
            
           // img = img.Erode(new Mat());
            img = img.Dilate(new Mat());
            img = img.Canny(img.Width,img.Height);
            //img = img.Dilate(new Mat());
            //img = img.GaussianBlur(size, 3);
            //img = img.Dilate(new Mat());

            CvCircleSegment[] circles = img.HoughCircles(HoughCirclesMethod.Gradient, 1, 0.1);


            var watch = Stopwatch.StartNew();
            

            watch.Stop();
            Console.WriteLine("Detection time = {0}ms", watch.ElapsedMilliseconds);
            //  Console.WriteLine("{0} region(s) found", found.Length);


            for (int i = 0; i < circles.Length; i++)
            {
                float R = circles[i].Radius;
                Point2f cnt = circles[i].Center;

                Rect r = new Rect()
                {
                    Top = (int)(cnt.Y - R),
                    Left = (int)(cnt.X - R),
                    Height = (int)(2 * R) + 1,
                    Width = (int)(2 * R) + 1
                };

                img_result.Rectangle(r.BottomRight, r.TopLeft, Scalar.Red, 1, LineType.AntiAlias);
            }

            using (var window = new Window("people detector", WindowMode.None, img_result))
            {
                window.SetProperty(WindowProperty.AspectRatio, 1);
                Cv.WaitKey(0);
            }
        }
    }
}
