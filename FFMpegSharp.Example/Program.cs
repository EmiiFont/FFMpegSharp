using System;
using System.IO;
using System.Linq;
using FFMpegSharp.FFMPEG;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;

namespace FFMpegSharp.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var encoder = new FFMpeg();

            var _path = ConfigurationManager.AppSettings["path"];

            var di = new DirectoryInfo(_path);

            var images = di.GetFiles().Where(c => c.Extension == ".png").Select(v => new { SentenceId = v.Name.Split('-')[0], Info = v });

            var grouped = images.GroupBy(b => b.SentenceId);

            var list = new List<ImageInfo>();

            foreach (var item in grouped)
            {
                var orderList = item.OrderBy(b => int.Parse(Path.GetFileNameWithoutExtension(b.Info.Name.Split('-')[1])));

                list.AddRange(orderList.Take(orderList.Count() - 4).Select(b => new ImageInfo { Path = b.Info.FullName, ImageBitmap = new Bitmap(b.Info.FullName), Duration = 0.1 }).ToList());

                list.AddRange(orderList.Skip(orderList.Count() - 4).Select(b => new ImageInfo { Path = b.Info.FullName, ImageBitmap = new Bitmap(b.Info.FullName), Duration = 0.4 }).ToList());
            }



            // Bind Progress Handler
            encoder.OnProgress += percentage => {
                Console.WriteLine("Progress {0}%", percentage);
            };

            encoder.CreateVideoFromImages(list, new FileInfo(_path + "\\test.mp4"), 10, 1280, 790);

        }
    }
}