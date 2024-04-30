using SportTogetherBlazor.Models;
using System.Text.Json;
using static SportTogetherBlazor.Components.Pages.InscriptionLogin.Inscription;

namespace SportTogetherBlazor.Services
{
    public  class SessionStorageServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionStorageServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SaveUserToSession(Utilisateur utilisateurInfo)
        {
            try
            {
                var serializedUser = JsonSerializer.Serialize(utilisateurInfo);
                _httpContextAccessor.HttpContext.Session.SetString("userInfo", serializedUser);
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
    }
}


