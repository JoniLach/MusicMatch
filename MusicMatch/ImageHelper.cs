using System;
using System.IO;

namespace MusicMatch
{
    public static class ImageHelper
    {
        public static string SaveImageLocally(string sourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                    return null;

                string appBase = AppDomain.CurrentDomain.BaseDirectory;
                string imagesFolder = Path.Combine(appBase, "Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string extension = Path.GetExtension(sourcePath);
                string newFileName = $"Profile_{Guid.NewGuid()}{extension}";
                string destinationPath = Path.Combine(imagesFolder, newFileName);

                File.Copy(sourcePath, destinationPath, true);

                // Return relative path or absolute? 
                // Using absolute for simplicity in WPF binding, but storing relative is better if app moves.
                // Let's return absolute path for now as it's easier for WPF ImageSource to resolve without converters
                // BUT User object should probably store something portable. 
                // Let's store the full path for now as it matches current usage in TeacherProfilePage (which checks File.Exists)
                return destinationPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }
    }
}
