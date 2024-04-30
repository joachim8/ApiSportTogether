using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SportTogetherBlazor.Models;
namespace SportTogetherBlazor.Services
{
    public class TaskUtilisateurServices
    {
        private readonly HttpClient _httpClient;

        public TaskUtilisateurServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Utilisateur>> GetUtilisateursAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Utilisateur>>("ApiSportTogether/Utilisateur");
        }

        public async Task<Utilisateur> GetUtilisateurByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Utilisateur>($"ApiSportTogether/Utilisateur/{id}");
        }

        public async Task<Utilisateur> CreateUtilisateurAsync(Utilisateur utilisateur)
        {
            var response = await _httpClient.PostAsJsonAsync("ApiSportTogether/Utilisateur/CreateUtilisateur", utilisateur);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Utilisateur>();
            }
            else
            {
                // Gérer les erreurs selon les besoins, par exemple renvoyer null ou lever une exception
                return null;
            }
        }

        public async Task<bool> UpdateUtilisateurAsync(int id, Utilisateur utilisateur)
        {
            var response = await _httpClient.PutAsJsonAsync($"ApiSportTogether/Utilisateur/{id}", utilisateur);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUtilisateurAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"ApiSportTogether/Utilisateur/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
