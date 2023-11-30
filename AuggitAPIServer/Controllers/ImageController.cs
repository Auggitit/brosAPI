using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImageController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost("upload")]
    public IActionResult Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }
        // Save to wwwroot/images directory
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        var imageUrl = Url.Action("GetImage", "Image", new { fileName = Path.GetFileName(filePath) });
        return Ok(new
        {
            message = "Image uploaded successfully",
            filePath,
            fileName = file.FileName,
            fileSize = file.Length,
            fileType = file.ContentType,
            imageUrl
        });
    }

    [HttpGet("getimage/{fileName}")]
    public IActionResult GetImage(string fileName)
    {
        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

        // Return the image file
        return PhysicalFile(imagePath, "image/jpeg");
    }
}
