using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;

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
            Mat img = Cv2.ImRead(openFileDialog1.FileName, LoadMode.GrayScale) ; // 4.JPG pyt.JPG 5.png 30 Stop
            Mat img_result = Cv2.ImRead(openFileDialog1.FileName, LoadMode.Color); // 4.JPG pyt.JPG 5.png

            // img[np.where((image == [0, 0, 0]).all(axis = 2))] = [0, 0, 255]
            int a = img_result.Channels();
           Mat[] ColorMats = img_result.Split();
            img_result = ColorMats[0];
            img = ColorMats[0].Threshold(55, 255, ThresholdType.Binary);
            img_result = Cv2.ImRead(openFileDialog1.FileName, LoadMode.Color);
            /
            CvCircleSegment[] circles = img.HoughCircles(HoughCirclesMethod.Gradient, 1, 10,100,30, 50, 100);


            var watch = Stopwatch.StartNew();
            

            watch.Stop();
            Console.WriteLine("Detection time = {0}ms", watch.ElapsedMilliseconds);
            //  Console.WriteLine("{0} region(s) found", found.Length);


            for (int i = 0; i < circles.Length; i++)
            {
                float R = circles[i].Radius;
                Point2f pnt = circles[i].Center;

                Rect r = new Rect()
                {
                    Top = (int)(pnt.Y - R),
                    Left = (int)(pnt.X - R),
                    Height = (int)(2 * R) + 1,
                    Width = (int)(2 * R) + 1
                };

                img_result.Rectangle(r.BottomRight, r.TopLeft, Scalar.Red, 1, LineType.AntiAlias);
                // img_result.Circle(pnt, (int)R, Scalar.Red, 3);


            }

            using (var window = new Window("people detector", WindowMode.None, img_result))
            {
                window.SetProperty(WindowProperty.AspectRatio, 1);
                Cv.WaitKey(0);
            }
        }
    }
}
