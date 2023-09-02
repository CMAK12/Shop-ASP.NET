namespace MyWebApp.Helpers
{
    public static class UploadPhotoHelper
    {
        public static string UploadPhoto(IFormFile photo, IWebHostEnvironment appEnvironment)
        {
            // Generate unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

            // Route to folder, where the photo will be stored
            string uploadsFolder = Path.Combine(appEnvironment.WebRootPath, "images");

            // Full route to file
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file in disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}
