using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class AnnonceImageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AnnonceImageController> _logger;

        //    public AnnonceImageController(IWebHostEnvironment environment, ILogger<AnnonceImageController> logger, SportTogetherContext context)
        //    {
        //        _environment = environment;
        //        _logger = logger;
        //        _context = context;
        //    }

        //    // GET: ApiSportTogether/AnnonceImage
        //    [HttpGet]
        //    public ActionResult<List<AnnonceImage>> GetAnnonceImages()
        //    {
        //        return _context.AnnonceImages.ToList();
        //    }

        //    // GET: ApiSportTogether/AnnonceImage/5
        //    [HttpGet("GetAnnonceImageByAnnonceId/{annonceId}")]
        //    public ActionResult<List<AnnonceImage>> GetAnnonceImageByAnnonceId(int annonceId)
        //    {
        //        List<AnnonceImage> annonceImage = _context.AnnonceImages.Where(ai => ai.AnnoncesId == annonceId).ToList();
        //        return annonceImage == null ? NotFound() : annonceImage;
        //    }

        //    // GET: ApiSportTogether/AnnonceImage/GetImageByPath
        //    [HttpGet("GetImageByPath/{imageId}")]
        //    public ActionResult GetImageByPath(int imageId)
        //    {
        //        var annonceImage = _context.AnnonceImages.FirstOrDefault(i => i.ImageId == imageId);
        //        if (annonceImage == null)
        //        {
        //            return NotFound("Image not found.");
        //        }

        //        if (!System.IO.File.Exists(annonceImage.Url))
        //        {
        //            return NotFound("File does not exist on server.");
        //        }

        //        return PhysicalFile(annonceImage.Url, "image/jpeg"); // Assuming the images are JPEGs
        //    }

        //    [HttpPost("Upload/{annoncesId}")]
        //    public async Task<ActionResult<IList<UploadResult>>> UploadAnnonceImage([FromForm] IEnumerable<IFormFile> files, int annoncesId)
        //    {
        //        var maxAllowedFiles = 3;
        //        long maxFileSize = 10485760; // 10 MB
        //        var filesProcessed = 0;
        //        var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
        //        List<UploadResult> uploadResults = new();

        //        foreach (var file in files)
        //        {
        //            var uploadResult = new UploadResult();
        //            var untrustedFileName = file.FileName;
        //            uploadResult.FileName = untrustedFileName;
        //            var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

        //            if (filesProcessed < maxAllowedFiles)
        //            {
        //                if (file.Length == 0)
        //                {
        //                    _logger.LogInformation("{FileName} length is 0 (Err: 1)", trustedFileNameForDisplay);
        //                    uploadResult.ErrorCode = 1;
        //                }
        //                else if (file.Length > maxFileSize)
        //                {
        //                    _logger.LogInformation("{FileName} of {Length} bytes is larger than the limit of {Limit} bytes (Err: 2)",
        //                        trustedFileNameForDisplay, file.Length, maxFileSize);
        //                    uploadResult.ErrorCode = 2;
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        var extension = Path.GetExtension(untrustedFileName);
        //                        var trustedFileNameForFileStorage = GenerateFileName(annoncesId) + extension;
        //                        var path = Path.Combine(_environment.ContentRootPath, "Images", trustedFileNameForFileStorage);

        //                        await using FileStream fs = new(path, FileMode.Create);
        //                        await file.CopyToAsync(fs);

        //                        _logger.LogInformation("{FileName} saved at {Path}", trustedFileNameForDisplay, path);
        //                        uploadResult.Uploaded = true;
        //                        uploadResult.StoredFileName = trustedFileNameForFileStorage;

        //                        var annonceImage = new AnnonceImage
        //                        {
        //                            Url = path,
        //                            Timestamp = DateTime.Now,
        //                            AnnoncesId = annoncesId
        //                        };

        //                        _context.AnnonceImages.Add(annonceImage);
        //                        _context.SaveChanges();
        //                    }
        //                    catch (IOException ex)
        //                    {
        //                        _logger.LogError("{FileName} error on upload (Err: 3): {Message}", trustedFileNameForDisplay, ex.Message);
        //                        uploadResult.ErrorCode = 3;
        //                    }
        //                }

        //                filesProcessed++;
        //            }
        //            else
        //            {
        //                _logger.LogInformation("{FileName} not uploaded because the request exceeded the allowed {Count} of files (Err: 4)",
        //                    trustedFileNameForDisplay, maxAllowedFiles);
        //                uploadResult.ErrorCode = 4;
        //            }

        //            uploadResults.Add(uploadResult);
        //        }

        //        return new CreatedResult(resourcePath, uploadResults);
        //    }

        //    private string GenerateFileName(int annoncesId)
        //    {
        //        return $"{annoncesId}_{Guid.NewGuid()}{Path.GetExtension(Path.GetRandomFileName())}";
        //    }



        //    // PUT: ApiSportTogether/AnnonceImage/{id}
        //    [HttpPut("{id}")]
        //    public async Task<ActionResult> PutAnnonceImage(int id, [FromForm] IFormFile file)
        //    {
        //        var annonceImage = _context.AnnonceImages.FirstOrDefault(i => i.ImageId == id);
        //        if (annonceImage == null)
        //        {
        //            return NotFound();
        //        }

        //        // Remove old image file
        //        if (System.IO.File.Exists(annonceImage.Url))
        //        {
        //            System.IO.File.Delete(annonceImage.Url);
        //        }

        //        // Save new image file
        //        string fileName = GenerateFileName(annonceImage.AnnoncesId ?? 0);
        //        string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        // Update database record
        //        annonceImage.Url = filePath;
        //        annonceImage.Timestamp = DateTime.Now; // Update timestamp

        //        _context.Entry(annonceImage).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(nameof(GetImageByPath), new { imageId = annonceImage.ImageId }, annonceImage);
        //    }

        //    // DELETE: ApiSportTogether/AnnonceImage/5
        //    [HttpDelete("{id}")]
        //    public ActionResult DeleteAnnonceImage(int id)
        //    {
        //        var annonceImage = _context.AnnonceImages.Find(id);
        //        if (annonceImage == null)
        //        {
        //            return NotFound();
        //        }

        //        // Remove image file
        //        if (System.IO.File.Exists(annonceImage.Url))
        //        {
        //            System.IO.File.Delete(annonceImage.Url);
        //        }

        //        _context.AnnonceImages.Remove(annonceImage);
        //        _context.SaveChanges();

        //        return NoContent();
        //    }
        //}
        //public class UploadResult
        //{
        //    public string FileName { get; set; }
        //    public int ErrorCode { get; set; }
        //    public bool Uploaded { get; set; }
        //    public string StoredFileName { get; set; }
        //}
    }

}
