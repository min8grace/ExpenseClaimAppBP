using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp.Processing;

namespace ExpenseClaimApp.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] OptimizeImageSize(this IBrowserFile file, int maxWidth, int maxHeight)
        {
            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                using (var writeStream = new MemoryStream())
                {
                    //if (image.Width > maxWidth)
                    //{
                    //    var thumbScaleFactor = maxWidth / image.Width;
                    //    image.Mutate(i => i.Resize(maxWidth, image.Height *
                    //        thumbScaleFactor));
                    //}
                    if (image.Height > maxHeight)
                    {
                        var thumbScaleFactor = maxHeight / image.Height;
                        image.Mutate(i => i.Resize(maxHeight, image.Width *
                            thumbScaleFactor));
                    }
                    image.SaveAsPng(writeStream);
                    return writeStream.ToArray();
                }
            }
        }
    }
}
