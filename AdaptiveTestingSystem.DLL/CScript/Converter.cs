using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdaptiveTestingSystem.DLL.CScript
{
    public class Converter
    {
        public static BitmapImage ConvertByteArrayToImage(byte[] imageBytes)
        {
            if (imageBytes.Count() == 0) return null;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public static byte[] ToByteArray(String hexString)
        {
            byte[] retval = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return retval;
        }

        public static byte[] ConvertBitmapSourceToByteArray(string filepath)
        {
            //var image = new BitmapImage(new Uri(filepath));
            //byte[] data;
            //BitmapEncoder encoder = new JpegBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(image));
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    encoder.Save(ms);
            //    data = ms.ToArray();
            //}

            FileStream fS = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fS.Length];
            fS.Read(b, 0, (int)fS.Length);
            fS.Close();

            return b;
        }

        public static byte[] ConvertBitmapSourceToByteArray(BitmapSource image)
        {
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static byte[] ConvertBitmapSourceToByteArray(ImageSource imageSource)
        {
            var image = imageSource as BitmapSource;
            byte[] data;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }


        public static RenderTargetBitmap ConvertUIElementToBitmap(UIElement uiElement, double resolution)
        {
            var scale = resolution / 96d;

            uiElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var sz = uiElement.DesiredSize;
            var rect = new Rect(sz);
            uiElement.Arrange(rect);

            var bmp = new RenderTargetBitmap((int)(scale * (rect.Width)), (int)(scale * (rect.Height)), scale * 96, scale * 96, PixelFormats.Default);
            bmp.Render(uiElement);

            return bmp;
        }


        public static void ConvertToJpeg(UIElement uiElement, string path, double resolution)
        {
            var jpegString = CreateJpeg(ConvertUIElementToBitmap(uiElement, resolution));

            if (path != null)
            {
                try
                {
                    using (var fileStream = File.Create(path))
                    {
                        using (var streamWriter = new StreamWriter(fileStream, Encoding.Default))
                        {
                            streamWriter.Write(jpegString);
                            streamWriter.Close();
                        }

                        fileStream.Close();
                    }
                }

                catch (Exception ex)
                {
                    //TODO: handle exception here
                }
            }
        }

        public static string CreateJpeg(RenderTargetBitmap bitmap)
        {
            var jpeg = new JpegBitmapEncoder();
            jpeg.Frames.Add(BitmapFrame.Create(bitmap));
            string result;

            using (var memoryStream = new MemoryStream())
            {
                jpeg.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memoryStream, Encoding.Default))
                {
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                memoryStream.Close();
            }

            return result;
        }
    }
}
