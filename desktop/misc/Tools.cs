using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace desktop.misc
{
    internal static class Tools
    {
        public static bool GetImageSource(string path, out ImageSource? imageSource)
        {
            if (path.Length == 0)
            {
                path = "image-help-1.png"; // change to image-not-found
            }
            try
            {
                Stream imageStreamSource = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );
                PngBitmapDecoder decoder = new PngBitmapDecoder(
                    imageStreamSource,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.Default
                );
                BitmapSource bitmapSource = decoder.Frames[0];
                /*BitmapImage image = new BitmapImage();
                Uri? uri;
                image.BeginInit();
                image.UriSource = Uri.TryCreate(path, UriKind.Relative, out uri) ? uri : new Uri(@"");
                image.EndInit();*/

                imageSource = bitmapSource;
                //Trace.WriteLine("Succesfully got image source");
                return true;
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine($"File {path} was not found.");
                imageSource = null;
                //logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name).Wait();
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool GetImageSource(string path, out BitmapSource? imageSource)
        {
            if (path.Length == 0)
            {
                path = "image-help-1.png"; // change to image-not-found
            }
            try
            {
                Stream imageStreamSource = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read
                );
                PngBitmapDecoder decoder = new PngBitmapDecoder(
                    imageStreamSource,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.Default
                );
                BitmapSource bitmapSource = decoder.Frames[0];
                /*BitmapImage image = new BitmapImage();
                Uri? uri;
                image.BeginInit();
                image.UriSource = Uri.TryCreate(path, UriKind.Relative, out uri) ? uri : new Uri(@"");
                image.EndInit();*/

                imageSource = bitmapSource;
                //Trace.WriteLine("Succesfully got image source");
                return true;
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine($"File {path} was not found.");
                imageSource = null;
                //logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name).Wait();
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                Trace.WriteLine($"File {path} was not found. Exception: {ex.Message}");
                imageSource = null;
                //logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name).Wait();
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
