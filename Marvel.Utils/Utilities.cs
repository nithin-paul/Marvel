using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Utils
{
    public static class Utilities
    {

        private const int IMAGE_WIDTH = 400;
        private const int IMAGE_HEIGHT = 400;

        private const int IMAGE_WIDTH_DETAIL = 500;
        private const int IMAGE_HEIGHT_DETAIL = 500;
        /// <summary>
        /// Splits the string using the specified seperators, and removes the delimiters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="seperators">The seperators.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns>The split array of strings</returns>
        public static string[] Split(this string input, string seperators, string delimiters)
        {
            string[] splitBySeperator = input.Split(seperators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(delimiters))
            {
                // Trim away the delimiters
                for (int i = 0; i < splitBySeperator.Length; i++)
                {
                    splitBySeperator[i] = splitBySeperator[i].Trim(delimiters.ToCharArray());
                }
            }

            return splitBySeperator;
        }

        public static void ResizeImage(Image image, string filePath)
        {
            string fileNameNoExt = Path.GetFileNameWithoutExtension(filePath);
            string ext = Path.GetExtension(filePath);
            string thumbNailPath = Path.GetDirectoryName(filePath)+"\\"+fileNameNoExt + "_thumb" + ext;
            string detailPath = Path.GetDirectoryName(filePath) + "\\" + fileNameNoExt + "_detail" + ext;
            Image thumbNailImage = (Image)image.Clone();
            Image detailImage = (Image)image.Clone();
            // Figure out the ratio
            double ratioX = (double)IMAGE_WIDTH / (double)image.Width;
            double ratioY = (double)IMAGE_HEIGHT / (double)image.Height;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(image.Height * ratio);
            int newWidth = Convert.ToInt32(image.Width * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((IMAGE_WIDTH - (image.Width * ratio)) / 2);
            int posY = Convert.ToInt32((IMAGE_HEIGHT - (image.Height * ratio)) / 2);

            Bitmap thumbnailBitmap = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT);
            Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbnailGraph.DrawImage(thumbNailImage, imageRectangle);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(thumbNailPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    thumbnailBitmap.Save(memory, thumbNailImage.RawFormat);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            thumbnailBitmap.Dispose();
            thumbNailImage.Dispose();


            // Figure out the ratio
            double ratioDetailX = (double)IMAGE_WIDTH_DETAIL / (double)image.Width;
            double ratioDetailY = (double)IMAGE_HEIGHT_DETAIL / (double)image.Height;
            // use whichever multiplier is smaller
            double ratioDetail = ratioDetailX < ratioDetailY ? ratioDetailX : ratioDetailY;

            // now we can get the new height and width
            int newDetailHeight = Convert.ToInt32(image.Height * ratioDetail);
            int newDetailWidth = Convert.ToInt32(image.Width * ratioDetail);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posDetailX = Convert.ToInt32((IMAGE_WIDTH_DETAIL - (image.Width * ratioDetail)) / 2);
            int posDetailY = Convert.ToInt32((IMAGE_HEIGHT_DETAIL - (image.Height * ratioDetail)) / 2);
            Bitmap detailBitmap = new Bitmap(IMAGE_WIDTH_DETAIL, IMAGE_HEIGHT_DETAIL);
            Graphics detailGraph = Graphics.FromImage(detailBitmap);
            detailGraph.CompositingQuality = CompositingQuality.HighQuality;
            detailGraph.SmoothingMode = SmoothingMode.HighQuality;
            detailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangleDetail = new Rectangle(0, 0, newDetailWidth, newDetailHeight);
            detailGraph.DrawImage(detailImage, imageRectangleDetail);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(detailPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    detailBitmap.Save(memory, detailImage.RawFormat);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            detailBitmap.Dispose();
            detailImage.Dispose();
            image.Dispose();
        }
    }
}
