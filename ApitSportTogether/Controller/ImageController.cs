using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImageController(SportTogetherContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ApiSportTogether/Image
        [HttpGet]
        public ActionResult<List<Image>> GetImages()
        {
            return _context.Images.ToList();
        }

        // GET: ApiSportTogether/Image/GetImageByPath
        [HttpGet("GetImageByPath/{imageId}")]
        public ActionResult GetImageByPath(int imageId)
        {
            var image = _context.Images.FirstOrDefault(i => i.ImagesId == imageId);
            if (image == null)
            {
                return NotFound("Image not found.");
            }

            if (!System.IO.File.Exists(image.Url))
            {
                return NotFound("File does not exist on server.");
            }

            return PhysicalFile(image.Url, "image/jpeg"); // Assuming the images are JPEGs
        }



        // POST: ApiSportTogether/Image/Upload
        [HttpPost("Upload")]
        public async Task<ActionResult> UploadImage([FromForm] IFormFile file, [FromForm] int utilisateurId, [FromForm] string type, [FromForm] int? annonceId = null, [FromForm] int? publicationId = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileName = GenerateFileName(utilisateurId, type, annonceId, publicationId);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new Image
            {
                Url = filePath,
                Type = type,
                Timestamp = DateTime.Now,
                UtilisateurId = utilisateurId
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = image.ImagesId }, image);
        }


        // PUT: ApiSportTogether/Image/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutImage(int id, [FromForm] IFormFile file, [FromForm] int utilisateurId, [FromForm] string type, [FromForm] int? annonceId = null, [FromForm] int? publicationId = null)
        {
            var image = _context.Images.FirstOrDefault(i => i.ImagesId == id);
            if (image == null)
            {
                return NotFound();
            }

            // Remove old image file
            if (System.IO.File.Exists(image.Url))
            {
                System.IO.File.Delete(image.Url);
            }

            // Save new image file
            string fileName = GenerateFileName(utilisateurId, type, annonceId, publicationId);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update database record
            image.Url = filePath;
            image.Timestamp = DateTime.Now; // Update timestamp

            _context.Entry(image).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = image.ImagesId }, image);
        }


        // DELETE: ApiSportTogether/Image/5
        [HttpDelete("{id}")]
        public ActionResult DeleteImage(int id)
        {
            var image = _context.Images.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            // Remove image file
            if (System.IO.File.Exists(image.Url))
            {
                System.IO.File.Delete(image.Url);
            }

            _context.Images.Remove(image);
            _context.SaveChanges();

            return NoContent();
        }

        private string GenerateFileName(int utilisateurId, string type, int? annonceId, int? publicationId)
        {
            switch (type.ToLower())
            {
                case "profil":
                    return $"photo_de_profil_pseudo_{utilisateurId}";
                case "annonce":
                    return $"photo_annonce_{utilisateurId}_{annonceId}";
                case "publication":
                    return $"photo_publications_{utilisateurId}_{publicationId}";
                default:
                    throw new ArgumentException("Invalid image type.");
            }
        }
    
    }
}
