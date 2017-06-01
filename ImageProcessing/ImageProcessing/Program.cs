using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Dosya Adı: ");
            var dosyaAdi = Console.ReadLine();

            Console.Write("Dönüştürelecek Uzantı: ");
            var uzanti = Console.ReadLine();

            var filePath = "../../" + dosyaAdi;

            if (File.Exists(filePath))
            {
                ChangeFileFormat(filePath, new TiffFormat());
            }
            else
            {
                Console.WriteLine("Dosya Bulunamadı.");
            }

            Console.ReadKey();



        }
        public static void ChangeFileFormat(string filePath, ISupportedImageFormat fileFormat, int fileQuality = 100)
        {

            var file = File.OpenRead(filePath);
            var fileInfo = new FileInfo(file.Name);

            byte[] photoBytes = File.ReadAllBytes(filePath);
            
            
            // Format is automatically detected though can be changed.
            //ISupportedImageFormat format = new TiffFormat { Quality = fileQuality };
            //Size size = new Size(300, 0);

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        fileFormat.Quality = fileQuality;
                        imageFactory.Load(inStream)
                                    //.Resize(size)
                                    .Format(fileFormat)
                                    .Save(outStream);
                    }

                    Console.WriteLine(fileInfo.FullName);
                    var newFilePath = fileInfo.DirectoryName + "/" + Path.GetFileNameWithoutExtension(fileInfo.FullName) + "." + fileFormat.DefaultExtension;
                    Console.WriteLine(newFilePath);
                    FileStream fileStream = new FileStream(newFilePath, FileMode.Create);
                    outStream.WriteTo(fileStream);
                }
            }
        }
    }
}
