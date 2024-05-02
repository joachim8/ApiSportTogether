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
    public class ProfileImageController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProfileImageController(SportTogetherContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: ApiSportTogether/ProfileImage
        [HttpGet]
        public ActionResult<List<ProfileImage>> GetProfileImages()
        {
            return _context.ProfileImages.ToList();
        }

        // GET: ApiSportTogether/ProfileImage/5
        [HttpGet("GetProfileImageByUtilisateurId/{UtilisateurId}")]
        public ActionResult<ProfileImage> GetProfileImageByUtilisateurId(int UtilisateurId)
        {
            var profileImage = _context.ProfileImages.Where(i => i.UtilisateursId == UtilisateurId && i.Type == "Profil").FirstOrDefault();
            return profileImage == null ? NotFound() : profileImage;
        }

        // GET: ApiSportTogether/ProfileImage/GetImageByPath
        [HttpGet("GetImageByPath/{imageId}")]
        public ActionResult GetImageByPath(int imageId)
        {
            var profileImage = _context.ProfileImages.FirstOrDefault(i => i.ImageId == imageId);
            if (profileImage == null)
            {
                return NotFound("Image not found.");
            }

            if (!System.IO.File.Exists(profileImage.Url))
            {
                return NotFound("File does not exist on server.");
            }

            return PhysicalFile(profileImage.Url, "image/jpeg"); // Assuming the images are JPEGs
        }

        // POST: ApiSportTogether/ProfileImage/Upload
        [HttpPost("Upload")]
        public async Task<ActionResult> UploadProfileImage([FromForm] IFormFile file, [FromForm] int utilisateurId, [FromForm] string type)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileName = GenerateFileName(utilisateurId, type);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var profileImage = new ProfileImage
            {
                Url = filePath,
                Type = type,
                Timestamp = DateTime.Now,
                UtilisateursId = utilisateurId
            };

            _context.ProfileImages.Add(profileImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = profileImage.ImageId }, profileImage);
        }

        // PUT: ApiSportTogether/ProfileImage/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProfileImage(int id, [FromForm] IFormFile file, [FromForm] int utilisateurId, [FromForm] string type)
        {
            var profileImage = _context.ProfileImages.FirstOrDefault(i => i.ImageId == id);
            if (profileImage == null)
            {
                return NotFound();
            }

            // Remove old image file
            if (System.IO.File.Exists(profileImage.Url))
            {
                System.IO.File.Delete(profileImage.Url);
            }

            // Save new image file
            string fileName = GenerateFileName(utilisateurId, type);
            string filePath = Path.Combine(_environment.WebRootPath, "Images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update database record
            profileImage.Url = filePath;
            profileImage.Timestamp = DateTime.Now; // Update timestamp

            _context.Entry(profileImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImageByPath), new { imageId = profileImage.ImageId }, profileImage);
        }

        // DELETE: ApiSportTogether/ProfileImage/5
        [HttpDelete("{id}")]
        public ActionResult DeleteProfileImage(int id)
        {
            var profileImage = _context.ProfileImages.Find(id);
            if (profileImage == null)
            {
                return NotFound();
            }

            // Remove image file
            if (System.IO.File.Exists(profileImage.Url))
            {
                System.IO.File.Delete(profileImage.Url);
            }

            _context.ProfileImages.Remove(profileImage);
            _context.SaveChanges();

            return NoContent();
        }

        private string GenerateFileName(int utilisateurId, string type)
        {
            string guidPart = Guid.NewGuid().ToString();
            switch (type.ToLower())
            {
                case "profil":
                    return $"photo_de_profil_{utilisateurId}_{guidPart}";
                case "photos":
                    return $"photo_utilisateur{utilisateurId}_{guidPart}";
             
                default:
                    throw new ArgumentException("Invalid image type.");
            }
        }
    }
}
