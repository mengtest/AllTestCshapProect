using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


/// <summary>
/// 改变图片大小
/// </summary>
namespace ChangeImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Image img = Image.FromFile("e:/0.png");
            Image _img = resizeImage(img, new Size(512, 512));
            _img.Save("e:/1.png");
        }
        private static Image resizeImage(Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            //destWidth = size.Width;
            //destHeight = size.Height;
            
            //Bitmap b = new Bitmap(destWidth, destHeight);
            Bitmap b = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, size.Height / 2 - destHeight / 2, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }
    }
}
