using ImageMagick;
using ImageMagick.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DijitalCard.Infrastructure.Extensions
{
    public class ImageHelper
    {
        public bool Resize(int width, int height, string source_path)
        {
            if (!File.Exists(source_path))
                return false;

            try
            {
                var file_path = Path.GetDirectoryName(source_path);

                if (!Directory.Exists(Path.Combine(file_path)))
                    Directory.CreateDirectory(Path.Combine(file_path));

                using (var image = new MagickImage(source_path))
                {
                    image.Resize(width, height);
                    image.Write(source_path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Crop(int width, int height, string source_path)
        {
            if (!File.Exists(source_path))
                return false;

            try
            {
                using (var image = new MagickImage(source_path))
                {
                    image.Crop(width, height);
                    image.Write(source_path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool CropAndResize(int width, int height, string source_path)
        {
            if (!File.Exists(source_path))
                return false;

            try
            {
                using (var image = new MagickImage(source_path))
                {
                    var request_file_ratio = (double)width / (double)height;

                    var current_file_ratio = (double)image.Width / (double)height;

                    if (request_file_ratio > current_file_ratio)
                    {
                        var new_height = image.Width * height / width;
                        image.Crop(image.Width, new_height);
                    }
                    else
                    {
                        var new_width = image.Height * width / height;
                        image.Crop(new_width, image.Height);
                    }

                    image.Resize(width, height);
                    image.Write(source_path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Optimize(string source_path)
        {
            if (!File.Exists(source_path))
                return false;

            try
            {
                using (var image = new MagickImage(source_path))
                {
                    image.Settings.SetDefines(new JpegWriteDefines()
                    {
                        SamplingFactor = JpegSamplingFactor.Ratio420,
                    });

                    image.Strip();
                    image.Quality = 85;
                    image.Interlace = Interlace.Jpeg;
                    image.ColorSpace = ColorSpace.sRGB;

                    image.Write(source_path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Quality(int quality, string source_path)
        {
            if (!File.Exists(source_path))
                return false;

            try
            {
                using (var image = new MagickImage(source_path))
                {
                    image.Quality = quality;
                    image.Write(source_path);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
