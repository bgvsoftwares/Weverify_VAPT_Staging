using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WebAppBGV.Models
{
    public class ImageResize
    {
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static string CompressImage(string SoucePath, string DestPath, int quality)
        {
            string result = "";
            var FileName = Path.GetFileName(SoucePath);
            string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.ToString();
            DestPath = DestPath + "\\" + newFile;
            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);
                result = DestPath;
                return result + "::" + newFile;
            }
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static void CompressImageWH(string SoucePath, string DestPath, int quality, int Width, int Height)
        {
            var FileName = Path.GetFileName(SoucePath);
            DestPath = DestPath + "\\" + FileName;
            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                var originalBMP = new Bitmap(SoucePath);
                int originalWidth = originalBMP.Width;
                int originalHeight = originalBMP.Height;
                if (Width == 0)
                {
                    Width = originalBMP.Width;
                }
                if (Height == 0)
                {
                    Height = originalBMP.Height;
                }
                //float ratioX = (float)Width / (float)originalWidth;
                //float ratioY = (float)Height / (float)originalHeight;
                //float ratio = Math.Min(ratioX, ratioY);
                //Width = (int)(originalWidth * ratio);
                //Height = (int)(originalHeight * ratio);
                //var bitmap = new Bitmap(originalBMP, Width, Height);
                //var imgGraph = Graphics.FromImage(bitmap);
                //imgGraph.SmoothingMode = SmoothingMode.Default;
                //imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //imgGraph.DrawImage(originalBMP, 0, 0, Width, Height);
                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);
                //EncoderParameter myEncoderParameter2 = new EncoderParameter(QualityEncoder, Width);
                //EncoderParameter myEncoderParameter3 = new EncoderParameter(QualityEncoder, Height);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //myEncoderParameters.Param[1] = myEncoderParameter2;
                //myEncoderParameters.Param[2] = myEncoderParameter3;
                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);
            }
        }
        public static string CompressImg(string SoucePath, string DestPath, int quality, int Width, int Height)
        {
            string result = "";
            string newFile = string.Empty;
            var FileName = Path.GetFileName(SoucePath);
            if (FileName != "")
            {
                int width = Convert.ToInt32(Width);
                int height = Convert.ToInt32(Height);
                Stream inp_Stream = GetStream(SoucePath);

                using (var image = System.Drawing.Image.FromStream(inp_Stream))
                {
                    if (height == 0)
                    {
                        height = image.Height;
                    }
                    if (width == 0)
                    {
                        width = image.Width;
                    }
                    if (quality == 0)
                    {
                        quality = 60;
                    }
                    Bitmap myImg = new Bitmap(width, height);
                    Graphics myImgGraph = Graphics.FromImage(myImg);
                    myImgGraph.CompositingQuality = CompositingQuality.HighQuality;
                    myImgGraph.SmoothingMode = SmoothingMode.HighQuality;
                    myImgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imgRectangle = new Rectangle(0, 0, width, height);
                    myImgGraph.DrawImage(image, imgRectangle);
                    string ext = string.Empty;
                    ext = System.IO.Path.GetExtension(FileName.ToString()).ToLower();
                    var path = Path.Combine(DestPath, newFile);
                    if (File.Exists(path))
                    {
                        newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.ToString();
                        path = Path.Combine(DestPath, newFile);
                    }
                    else
                    {
                        newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + FileName.ToString();
                        path = Path.Combine(DestPath, newFile);

                    }
                    myImg.Save(path, image.RawFormat);
                    result = CompressImage(path, DestPath, quality);
                    //lblResult.Text = "Image resized and saved in folder 'ResizedImages' successfully";
                    //lblResult.ForeColor = Color.Green;
                    //txtWidth.Text = txtHeight.Text = "";
                }
            }
            else
            {
                //lblResult.Text = "please upload image";
            }
            return result;

        }
        public static void FileDelete(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static Stream GetStream(string fileName)
        {
            Stream result;
            try
            {
                result = File.OpenRead(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}