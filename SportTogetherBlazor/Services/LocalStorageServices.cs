using Microsoft.JSInterop;
using SportTogetherBlazor.Models;
using System.Net.Http;
using System.Text.Json;

namespace SportTogetherBlazor.Services
{
    public class LocalStorageServices
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IHttpClientFactory _httpClientFactory;

        public LocalStorageServices(IJSRuntime jsRuntime, IHttpClientFactory httpClientFactory)
        {
            _jsRuntime = jsRuntime;
            _httpClientFactory = httpClientFactory;
        }

        // Sauvegarde l'utilisateur dans LocalStorage
        public async Task SaveUserToLocalStorage(Utilisateur utilisateurInfo)
        {
            try
            {
                var serializedUser = JsonSerializer.Serialize(utilisateurInfo);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userInfo", serializedUser);

                // Générer un GUID pour la session utilisateur
                var userSessionId = Guid.NewGuid().ToString();
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "UserSessionId", userSessionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving user to local storage: " + ex.Message);
            }
        }

        // Récupère l'utilisateur depuis LocalStorage
        public async Task<Utilisateur?> GetUserFromLocalStorage()
        {
            try
            {
                var userInfoJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userInfo");
                return userInfoJson == null ? default : JsonSerializer.Deserialize<Utilisateur>(userInfoJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user from local storage: " + ex.Message);
                return null;
            }
        }

        // Récupère l'ID de la session utilisateur depuis LocalStorage
        public async Task<string?> GetUserSessionIdFromLocalStorage()
        {
            try
            {
                var sessionId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "UserSessionId");
                return sessionId == null ? default : sessionId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving session ID from local storage: " + ex.Message);
                return null;
            }
        }

        // Sauvegarde l'URL de l'image dans LocalStorage
        public async Task SaveImageUrl(string imageURL)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "imageProfilUrl", imageURL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving image URL to local storage: " + ex.Message);
            }
        }

        // Récupère l'URL de l'image depuis LocalStorage
        public async Task<string?> GetImageUrl()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "imageProfilUrl");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving image URL from local storage: " + ex.Message);
                return null;
            }
        }

        // Méthode pour la déconnexion
        public async Task Logout()
        {
            try
            {
                HttpClient http = _httpClientFactory.CreateClient("ApiSportTogetherClient");
                var userInfoJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userInfo");
                Utilisateur utili = JsonSerializer.Deserialize<Utilisateur>(userInfoJson);
                await http.GetAsync($"auth/deconnexion/{utili!.UtilisateursId}");

                // Efface les données du LocalStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.clear");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during logout: " + ex.Message);
            }
        }
    }
}
