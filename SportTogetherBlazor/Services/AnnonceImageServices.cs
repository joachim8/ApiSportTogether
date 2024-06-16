using Microsoft.AspNetCore.Mvc;
using SportTogetherBlazor.Models;

namespace SportTogetherBlazor.Services
{
    public class AnnonceImageServices
    {
        private readonly HttpClient _httpClient;

        public AnnonceImageServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Méthode pour récupérer toutes les images d'annonces
        public async Task<List<AnnonceImage>> GetAllAnnonceImagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("ApiSportTogether/AnnonceImage");
                if (response.IsSuccessStatusCode)
                {
                    var annonceImages = await response.Content.ReadFromJsonAsync<List<AnnonceImage>>();
                    return annonceImages;
                }
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
            }
            return null;
        }

        // Méthode pour récupérer les images par ID d'annonce
        public async Task<List<AnnonceImage>> GetAnnonceImagesByAnnonceIdAsync(int annonceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ApiSportTogether/AnnonceImage/GetAnnonceImageByAnnonceId/{annonceId}");
                if (response.IsSuccessStatusCode)
                {
                    var annonceImages = await response.Content.ReadFromJsonAsync<List<AnnonceImage>>();
                    return annonceImages;
                }
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
            }
            return null;
        }

        // Méthode pour télécharger une image d'annonce
        public async Task<HttpResponseMessage> UploadAnnonceImageAsync(IFormFile file, int annoncesId)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                formData.Add(new StringContent(annoncesId.ToString()), "annoncesId");

                return await _httpClient.PostAsync("ApiSportTogether/AnnonceImage/Upload", formData);
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        // Méthode pour mettre à jour une image d'annonce
        public async Task<HttpResponseMessage> UpdateAnnonceImageAsync(int id, IFormFile file)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

                return await _httpClient.PutAsync($"ApiSportTogether/AnnonceImage/{id}", formData);
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        // Méthode pour supprimer une image d'annonce
        public async Task<bool> DeleteAnnonceImageAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"ApiSportTogether/AnnonceImage/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
                return false;
            }
        }

        // Méthode pour obtenir le chemin d'une image
        public async Task<ActionResult> GetImageByPathAsync(int imageId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ApiSportTogether/AnnonceImage/GetImageByPath/{imageId}");
                if (response.IsSuccessStatusCode)
                {
                    // Assumer que l'image est traitée ici comme nécessaire
                }
                return new ActionResult { Success = response.IsSuccessStatusCode };
            }
            catch (Exception ex)
            {
                // Gestion d'erreurs
                return new ActionResult { Success = false, Message = ex.Message };
            }
        }
    }
    public class ActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
