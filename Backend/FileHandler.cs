using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

public static class FileHandler
{
    private static string folderPath = @"C:\temp\file_uploads";
    public static IResult Upload(IFormFileCollection UploadFiles, HttpContext context)
    {
        try
        {
            foreach (var file in UploadFiles)
            {
                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                if (!System.IO.File.Exists(filename))
                {
                    Directory.CreateDirectory(folderPath);
                    using (var fs = System.IO.File.Create($"{folderPath}\\{filename}"))
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
            Results.Content("No Content", "application/json; charset=utf-8");

            return Results.StatusCode(204);
        }

        return Results.BadRequest();
    }
}