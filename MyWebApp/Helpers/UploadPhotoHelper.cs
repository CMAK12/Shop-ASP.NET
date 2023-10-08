namespace MyWebApp.Helpers
{
    public static class UploadPhotoHelper
    {
        public static async Task<string> UploadPhotoAsync(IFormFile photo, IWebHostEnvironment appEnvironment)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string uploadsFolder = Path.Combine(appEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}
