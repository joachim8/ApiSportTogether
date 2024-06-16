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
    public class AnnonceImageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IWebHostEnvironment _environment;

        public AnnonceImageController(SportTogetherContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ApiSportTogether/AnnonceImage
        [HttpGet]
        public ActionResult<List<AnnonceImage>> GetAnnonceImages()
        {
            return _context.AnnonceImages.ToList();
        }

        // GET: ApiSportTogether/AnnonceImage/5
        [HttpGet("GetAnnonceImageByAnnonceId/{annonceId}")]
        public ActionResult<List<AnnonceImage>> GetAnnonceImageByAnnonceId(int annonceId)
        {
            List<AnnonceImage> annonceImage = _context.AnnonceImages.Where(ai => ai.AnnoncesId == annonceId).ToList();
            return annonceImage == null ? NotFound() : annonceImage;
        }

        // GET: ApiSportTogether/AnnonceImage/GetImageByPath
        [HttpGet("GetImageByPath/{imageId}")]
        public ActionResult GetImageByPath(int imageId)
        {
            var annonceImage = _context.AnnonceImages.FirstOrDefault(i => i.ImageId == imageId);
            if (annonceImage == null)
            {
                return NotFound("Image not found.");
            }

            if (!System.IO.File.Exists(annonceImage.Url))
            {
                return NotFound("File does not exist on server.");
            }

            return PhysicalFile(annonceImage.Url, "image/jpeg"); // Assuming the images are JPEGs
        }

        // POST: ApiSportTogether/AnnonceImage/Upload/5
        [HttpPost("Upload/{annoncesId}")]
        public async Task<ActionResult> UploadAnnonceImage([FromBody] IFormFile file, int annoncesId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileName = GenerateFileName(annoncesId);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var annonceImage = new AnnonceImage
            {
                Url = filePath,
                Timestamp = DateTime.Now,
                AnnoncesId = annoncesId
            };

            _context.AnnonceImages.Add(annonceImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = annonceImage.ImageId }, annonceImage);
        }

        // PUT: ApiSportTogether/AnnonceImage/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAnnonceImage(int id, [FromForm] IFormFile file)
        {
            var annonceImage = _context.AnnonceImages.FirstOrDefault(i => i.ImageId == id);
            if (annonceImage == null)
            {
                return NotFound();
            }

            // Remove old image file
            if (System.IO.File.Exists(annonceImage.Url))
            {
                System.IO.File.Delete(annonceImage.Url);
            }

            // Save new image file
            string fileName = GenerateFileName(annonceImage.AnnoncesId ?? 0);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update database record
            annonceImage.Url = filePath;
            annonceImage.Timestamp = DateTime.Now; // Update timestamp

            _context.Entry(annonceImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = annonceImage.ImageId }, annonceImage);
        }

        // DELETE: ApiSportTogether/AnnonceImage/5
        [HttpDelete("{id}")]
        public ActionResult DeleteAnnonceImage(int id)
        {
            var annonceImage = _context.AnnonceImages.Find(id);
            if (annonceImage == null)
            {
                return NotFound();
            }

            // Remove image file
            if (System.IO.File.Exists(annonceImage.Url))
            {
                System.IO.File.Delete(annonceImage.Url);
            }

            _context.AnnonceImages.Remove(annonceImage);
            _context.SaveChanges();

            return NoContent();
        }

        private string GenerateFileName(int annoncesId)
        {
            string guidPart = Guid.NewGuid().ToString();
            return $"photo_annonce_{annoncesId}_{guidPart}.jpeg";
        }
    }
}
