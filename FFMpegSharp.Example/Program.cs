using System;
using System.IO;
using System.Linq;
using FFMpegSharp.FFMPEG;
using System.Configuration;
using System.Collections.Generic;

namespace FFMpegSharp.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var encoder = new FFMpeg();

            //  var images = Directory.GetFiles().OrderBy(c=> int.Parse(Path.GetFileNameWithoutExtension(c))).Select(v => new ImageInfo { Path = v, ImageBitmap = new Bitmap(v) });
            var _path = ConfigurationManager.AppSettings["path"];
            var di = new DirectoryInfo(_path);
            var images = di.GetFiles().Where(c => c.Extension == ".png").Select(v => new { SentenceId = v.Name.Split('-')[0], Info = v });

            var grouped = images.GroupBy(b => b.SentenceId);
            var list = new List<ImageInfo>();

            foreach (var item in grouped)
            {
                list.AddRange(item.Where(b => !b.Info.Name.Contains("cursor")).OrderBy(b => int.Parse(Path.GetFileNameWithoutExtension(b.Info.Name.Split('-')[1])))
                    .Select(b => new ImageInfo { Path = b.Info.FullName, ImageBitmap = new Bitmap(b.Info.FullName) }).ToList());
            }



            encoder.CreateVideoFromImages(list, new FileInfo(_path + "\\test.mp4"), 10, 1280, 790);
            // Bind Progress Handler
            encoder.OnProgress += percentage => { Console.WriteLine("Progress {0}%", percentage); };
            //// Start Encoding
            //var input = new VideoInfo("D:\\Typly\\typly.mp4");
            //encoder.ToGif(input, new FileInfo(input.FullName.Replace(input.Extension, ".gif")));
        }
    }
    }
}