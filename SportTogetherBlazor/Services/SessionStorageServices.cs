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
        public void Logout()
        {
            try
            {

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


