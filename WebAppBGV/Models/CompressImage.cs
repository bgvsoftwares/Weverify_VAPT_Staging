using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Net;
using System.Text;
using BAL;
using Encoder = System.Drawing.Imaging.Encoder;
using System.Web;
using DocumentFormat.OpenXml.Drawing;

namespace WebAppBGV
{
    public class CompressImage
    {
        public static string Compressimage(string FileUrl, string FilePath, string FileName)
        {
            string Resp = "";
            try
            {
                string path = FileUrl + FileName;
                //if (File.Exists(path))
                //{
                //    File.Replace(path, path, path);
                //}
                //string targetPath = Path.Combine(FilePath,
                //    $"{Guid.NewGuid()}_{FileName}");
                string targetPath = System.IO.Path.Combine(FilePath, FileName);
                targetPath = FilePath + FileName;
                Stream srcImgStream = GetStream(FileUrl + FileName);
                decimal originalSize = 0;
                if (srcImgStream != null)
                {
                    originalSize = srcImgStream.Length;
                }
                // Convert stream to image
                if (originalSize > 196)  ///536319
                {
                    var image = Image.FromStream(srcImgStream);
                    var originalBMP = new Bitmap(srcImgStream);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;
                    float maxHeight = 900.0f;
                    float maxWidth = 500.0f;
                    int newWidth;
                    int newHeight;
                    string values = "50L";
                    if (originalHeight <= 500)
                    {
                        maxHeight = originalBMP.Height;
                        maxWidth = originalBMP.Width;
                        values = "50L";
                    }
                    else
                    {
                        maxHeight = originalBMP.Height;
                        maxWidth = originalBMP.Width;
                        values = "50L";
                    }
                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }

                    else if (originalWidth >= maxWidth || originalHeight >= maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else if (originalHeight >= maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else if (originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }

                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;
                    }

                    var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
                    var imgGraph = Graphics.FromImage(bitmap);

                    imgGraph.SmoothingMode = SmoothingMode.Default;
                    imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                    var extension = System.IO.Path.GetExtension(targetPath).ToLower();
                    // for file extension having png and gif
                    //if (extension == ".png" || extension == ".gif")
                    //{
                    //    // Save image to targetPath
                    //    bitmap.Save(targetPath, image.RawFormat);
                    //}

                    // for file extension having .jpg or .jpeg
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".Pdf")
                    {
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        //var parameter = new EncoderParameter(myEncoder, 50L);
                        if (values == "55L")
                        {
                            var parameter = new EncoderParameter(myEncoder, 50L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }
                        else if (values == "50L")
                        {
                            var parameter = new EncoderParameter(myEncoder, 50L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }
                        else if (values == "45L")
                        {
                            var parameter = new EncoderParameter(myEncoder, 45L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }
                        else if (values == "40L")
                        {
                            var parameter = new EncoderParameter(myEncoder, 40L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }
                        else if (values == "35L")
                        {
                            var parameter = new EncoderParameter(myEncoder, 35L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }

                        else
                        {
                            var parameter = new EncoderParameter(myEncoder, 60L);
                            encoderParameters.Param[0] = parameter;

                            // Save image to targetPath
                            bitmap.Save(targetPath, jpgEncoder, encoderParameters);

                        }
                    }
                    Resp = "Compressed successfully";
                    //string reps = Convertbase64FromFiles(targetPath);
                    //StringBuilder StrBuilderObj = new StringBuilder();
                    //string json = "{\"Filename\":\"" + FileName + "\",\"FileExtension\":\"" + extension + "\",\"DocumentValue\":\"" + reps + "\"}";
                    //StrBuilderObj.Append(Convert.ToString(reps));
                    //API.WriteLog(json.ToString(), "", "", "ComparessImages", "ImagesLogDetails", FileName);
                    bitmap.Dispose();
                    imgGraph.Dispose();
                    originalBMP.Dispose();
                }
                else
                {
                    if (srcImgStream != null)
                    {
                        if(File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                            File.Copy(path, targetPath);
                        }
                        else
                        {
                            File.Copy(path,targetPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                Resp = ex.Message.ToString();
                return Resp;
            }
            return Resp;
        }
        public static string Convertbase64FromFiles(string Filepath)
        {
            String AsBase64String = "";
            Stream srcImgStream = GetStream(Filepath);
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                srcImgStream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            AsBase64String = Convert.ToBase64String(bytes);
            return AsBase64String;

        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
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
        public static Stream GetStream(string fileName)
        {
            Stream result;
            try
            {

                result = File.OpenRead(fileName);
            }
            catch (Exception ex)
            {
                //throw ex;
                string msg = ex.Message.ToString();
                return null;
            }
            return result;
        }
    }
}
