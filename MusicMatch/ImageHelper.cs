using System;
using System.IO;

namespace MusicMatch
{
    public static class ImageHelper
    {
        public static string GetImagePath(string relativePath)
        {
            string appBase = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(appBase, @"..\..\.."));

            // Define the relative path for the Images folder
            string imagePath = Path.Combine(projectDirectory, "MusicMatch", relativePath);
            return imagePath;
        }
        public static string SaveImageLocally(string sourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                    return null;

                // Get the project directory by navigating up from the bin folder
                string appBase = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Path.GetFullPath(Path.Combine(appBase, @"..\..\.."));

                // Define the relative path for the Images folder
                string imagesFolder = Path.Combine(projectDirectory, "MusicMatch", "Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string extension = Path.GetExtension(sourcePath);
                string newFileName = $"Profile_{Guid.NewGuid()}{extension}";
                string destinationPath = Path.Combine(imagesFolder, newFileName);

                File.Copy(sourcePath, destinationPath, true);

                // Return the relative path for database storage
                return Path.Combine("Images", newFileName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }
    }
}
