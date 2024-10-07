using SportTogetherBlazor.Models;
using System.Net.Http;
using System.Text.Json;
using static SportTogetherBlazor.Components.Pages.InscriptionLogin.Inscription;

namespace SportTogetherBlazor.Services
{
    public  class SessionStorageServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public SessionStorageServices(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public void SaveUserToSession(Utilisateur utilisateurInfo)
        {
            try
            {
                var serializedUser = JsonSerializer.Serialize(utilisateurInfo);
                _httpContextAccessor.HttpContext.Session.SetString("userInfo", serializedUser);
                // Si l'authentification réussit, générer un GUID
                var userSessionId = Guid.NewGuid().ToString();

                // Stocker le GUID dans le SessionStorage
                _httpContextAccessor.HttpContext.Session.SetString("UserSessionId", userSessionId);
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error saving user to session: " + ex.Message);
            }
        }

        public Utilisateur? GetUserFromSession()
        {
            try
            {
                var userInfoJson = _httpContextAccessor.HttpContext.Session.GetString("userInfo");
                return userInfoJson == null ? default : JsonSerializer.Deserialize<Utilisateur>(userInfoJson);
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error saving user to session: " + ex.Message);
                return null;
            }
           
        }
        public string? GetUserSessionIdFromSession()
        {
            try
            {
                var userInfoJson = _httpContextAccessor.HttpContext.Session.GetString("UserSessionId");
                return userInfoJson == null ? default : userInfoJson;
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error saving user to session: " + ex.Message);
                return null;
            }

        }

        public void SaveImageUrl(string imageURL)
        {
            try
            {
                // Directly saving the image URL to session without serialization
                _httpContextAccessor.HttpContext.Session.SetString("imageProfilUrl", imageURL);
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error saving image URL to session: " + ex.Message);
            }
        }

        public string? GetImageUrl()
        {
            try
            {
                // Directly getting the image URL from session without deserialization
                return _httpContextAccessor.HttpContext.Session.GetString("imageProfilUrl");
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error retrieving image URL from session: " + ex.Message);
                return null;
            }
        }
        public async Task Logout()
        {
            try
            {
                HttpClient http = _httpClientFactory.CreateClient("ApiSportTogetherClient");
                var userInfoJson = _httpContextAccessor.HttpContext.Session.GetString("userInfo");
                Utilisateur utili = JsonSerializer.Deserialize<Utilisateur>(userInfoJson);
                await http.GetAsync($"auth/deconnexion/{utili!.UtilisateursId}");

                _httpContextAccessor.HttpContext.Session.Clear();
            }
            catch (Exception ex)
            {
                // Log this error or handle it accordingly
                Console.WriteLine("Error during logout: " + ex.Message);
            }
        }

    }
}


