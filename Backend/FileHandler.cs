using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

public static class FileHandler
{
    private static string folderPath = @"/uploadedfiles";
    public static IResult Upload(IFormFileCollection UploadFiles, HttpContext context)
    {
        try
        {
            foreach (var file in UploadFiles)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var filetowrite = Path.Combine(folderPath, filename);
                Console.WriteLine($"Filename Uploaded: {filename} - {filetowrite}");
                if (!System.IO.File.Exists(filetowrite))
                {
                    Directory.CreateDirectory(folderPath);
                    using (var fs = System.IO.File.Create($"{filetowrite}"))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    var content = new { id = filename };
                    return Results.Json(content);
                }
                else
                {
                    Results.Content("File already exists.", "application/json; charset=utf-8");
                    return Results.StatusCode(204);

                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occurred: {e.ToString()} - {e.Message} - {e.StackTrace}");
            Results.Content("No Content", "application/text; charset=utf-8");

            return Results.StatusCode(204);
        }

        return Results.BadRequest();
    }
}
