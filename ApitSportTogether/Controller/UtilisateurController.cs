﻿using Microsoft.AspNetCore.Mvc;
using ApiSportTogether.model.ObjectContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiSportTogether.model.dbContext;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace ApiSportTogether.Controllers
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class UtilisateurController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;

        public UtilisateurController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: ApiSportTogether/Utilisateur
        [HttpGet]
        public ActionResult<List<Utilisateur>> GetUtilisateurs()
        {
            return _context.Utilisateurs
                           .Include(u => u.Image)
                           .Include(u => u.AmiUtilisateurId1Navigations)
                           .Include(u => u.AmiUtilisateurId2Navigations)
                           .Include(u => u.Annonces)
                           .Include(u => u.Publications)
                           .ToList();
        }

        // GET: ApiSportTogether/Utilisateur/5
        [HttpGet("{id}")]
        public ActionResult<Utilisateur> GetUtilisateurById(int id)
        {
            var utilisateur = _context.Utilisateurs
                                       .Include(u => u.Image)
                                       .Include(u => u.AmiUtilisateurId1Navigations)
                                       .Include(u => u.AmiUtilisateurId2Navigations)
                                       .Include(u => u.Annonces)
                                       .Include(u => u.Publications)
                                       .FirstOrDefault(u => u.UtilisateursId == id);

            return utilisateur == null ? NotFound() : utilisateur;
        }

        // POST: ApiSportTogether/Utilisateur
        [HttpPost]
        public ActionResult<Utilisateur> PostUtilisateur(Utilisateur utilisateur)
        {
            string motDePasseEncrypter = string.Empty;
            if(utilisateur != null)
            {
                if(utilisateur.MotDePasse != null && utilisateur.MotDePasse != string.Empty)
                {
                    utilisateur.MotDePasse = HashPassword(utilisateur.MotDePasse);
                }
                _context.Utilisateurs.Add(utilisateur);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUtilisateurById), new { id = utilisateur.UtilisateursId }, utilisateur);
            }else 
            { return NotFound(); }

        }

        // PUT: ApiSportTogether/Utilisateur/5
        [HttpPut("{id}")]
        public IActionResult PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateursId)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Utilisateurs.Any(u => u.UtilisateursId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        // DELETE: ApiSportTogether/Utilisateur/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUtilisateur(int id)
        {
            var utilisateur = _context.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
