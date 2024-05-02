using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSportTogether.Controllers
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class PublicationImageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IWebHostEnvironment _environment;

        public PublicationImageController(SportTogetherContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ApiSportTogether/PublicationImage
        [HttpGet]
        public ActionResult<List<PublicationImage>> GetPublicationImages()
        {
            return _context.PublicationImages.ToList();
        }

        // GET: ApiSportTogether/PublicationImage/GetPublicationImageById/5
        [HttpGet("GetPublicationImageById/{imageId}")]
        public ActionResult<PublicationImage> GetPublicationImageById(int imageId)
        {
            var publicationImage = _context.PublicationImages.FirstOrDefault(i => i.ImageId == imageId);
            return publicationImage == null ? NotFound() : publicationImage;
        }

        // GET: ApiSportTogether/PublicationImage/GetImageByPath
        [HttpGet("GetImageByPath/{imageId}")]
        public ActionResult GetImageByPath(int imageId)
        {
            var publicationImage = _context.PublicationImages.FirstOrDefault(i => i.ImageId == imageId);
            if (publicationImage == null)
            {
                return NotFound("Image not found.");
            }

            if (!System.IO.File.Exists(publicationImage.Url))
            {
                return NotFound("File does not exist on server.");
            }

            return PhysicalFile(publicationImage.Url, "image/jpeg"); // Assuming the images are JPEGs
        }

        // POST: ApiSportTogether/PublicationImage/Upload
        [HttpPost("Upload")]
        public async Task<ActionResult> UploadPublicationImage([FromForm] IFormFile file, [FromForm] int publicationsId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileName = GenerateFileName(publicationsId);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var publicationImage = new PublicationImage
            {
                Url = filePath,
                Timestamp = DateTime.Now,
                PublicationsId = publicationsId
            };

            _context.PublicationImages.Add(publicationImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = publicationImage.ImageId }, publicationImage);
        }

        // PUT: ApiSportTogether/PublicationImage/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPublicationImage(int id, [FromForm] IFormFile file)
        {
            var publicationImage = _context.PublicationImages.FirstOrDefault(i => i.ImageId == id);
            if (publicationImage == null)
            {
                return NotFound();
            }

            // Remove old image file
            if (System.IO.File.Exists(publicationImage.Url))
            {
                System.IO.File.Delete(publicationImage.Url);
            }

            // Save new image file
            string fileName = GenerateFileName(publicationImage.PublicationsId ?? 0);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update database record
            publicationImage.Url = filePath;
            publicationImage.Timestamp = DateTime.Now; // Update timestamp

            _context.Entry(publicationImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = publicationImage.ImageId }, publicationImage);
        }

        // DELETE: ApiSportTogether/PublicationImage/5
        [HttpDelete("{id}")]
        public ActionResult DeletePublicationImage(int id)
        {
            var publicationImage = _context.PublicationImages.Find(id);
            if (publicationImage == null)
            {
                return NotFound();
            }

            // Remove image file
            if (System.IO.File.Exists(publicationImage.Url))
            {
                System.IO.File.Delete(publicationImage.Url);
            }

            _context.PublicationImages.Remove(publicationImage);
            _context.SaveChanges();

            return NoContent();
        }

        private string GenerateFileName(int publicationsId)
        {
            string guidPart = Guid.NewGuid().ToString();
            return $"photo_publication_{publicationsId}_{guidPart}.jpeg";
        }
    }
}
