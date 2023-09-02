﻿namespace MyWebApp.Helpers
{
    public static class UpdatePhotoHepler
    {
        public static string UpdatePhoto(string photoPath, IFormFile photo, IWebHostEnvironment appEnvironment)
        {
            string uniqueName = Guid.NewGuid().ToString() + '_' + photo.FileName;
            string imagesPath = Path.Combine(appEnvironment.WebRootPath, "images");
            string fullPath = Path.Combine(imagesPath, uniqueName);
            string toDeletePath = Path.Combine(appEnvironment.WebRootPath, photoPath);

            File.Delete(toDeletePath);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return "/images/" + uniqueName;
        }
    }
}
