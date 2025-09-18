namespace MWBAYLY.Helper_servec
{
    public static class Imgfile
    {
        private const string imageFolder = "wwwroot\\images\\images";
        public static string SaveImage(IFormFile ImgUrl, string? oldImage = null)
        {
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageFolder, fileName);
                Directory.CreateDirectory(imageFolder);
                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgUrl.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(oldImage))
                {
                    DeleteImage(oldImage);
                }

                return fileName;
            }
            return oldImage ?? string.Empty;


        }
        public static void DeleteImage(string? imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), imageFolder, imageName);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
        }
     }
}
