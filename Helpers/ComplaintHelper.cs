using Student_Hall_Management.Models;

namespace Student_Hall_Management.Helpers
{
    public class ComplaintHelper
    {
        public string? StoreImage(string ImageData,string title)
        {
             string base64Data = ImageData;

                if (base64Data.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    int commaIndex = base64Data.IndexOf(',');
                    if (commaIndex > 0)
                    {
                        base64Data = base64Data.Substring(commaIndex + 1);
                    }
                }
                byte[] imageBytes = Convert.FromBase64String(base64Data);



                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Complaints", "Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = title + ".jpg";

                string filePath = Path.Combine(folderPath, fileName);

                try
                {
                    System.IO.File.WriteAllBytes(filePath, imageBytes);
                }
                catch (Exception ex)
                {
                Console.WriteLine(ex.ToString());
                    return null;
                }
                return filePath;
        }

        public string? storeFile(string fileData, string title)
        {
            string base64Data = fileData;

            //want to store as any type of file
            if (base64Data.StartsWith("data:application", StringComparison.OrdinalIgnoreCase))
            {
                int commaIndex = base64Data.IndexOf(',');
                if (commaIndex > 0)
                {
                    base64Data = base64Data.Substring(commaIndex + 1);
                }
            }
            byte[] fileBytes = Convert.FromBase64String(base64Data);

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Complaints", "Files");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = title + ".pdf";

            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                System.IO.File.WriteAllBytes(filePath, fileBytes);
            }
            catch (Exception ex)
            {
                return null;
            }

            return filePath;
        }
        
    }
}
