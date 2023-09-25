namespace MyWebApp.Helpers
{
    public static class DeletePhotoHelper
    {
        public static void DelPhoto(string photoPath, IWebHostEnvironment appEnvironment)
        {
            string toDeletePath = $"{appEnvironment.WebRootPath}\\{photoPath}";

            File.Delete(toDeletePath);
        }
    }
}
