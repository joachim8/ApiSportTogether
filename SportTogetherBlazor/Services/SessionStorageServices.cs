using SportTogetherBlazor.Models;
using System.Text.Json;

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
            var serializedUser = JsonSerializer.Serialize(utilisateurInfo);
             _httpContextAccessor.HttpContext!.Session.SetString("userInfo", serializedUser);
        }

        public Utilisateur? GetUserFromSession()
        {
            var userInfoJson = _httpContextAccessor.HttpContext.Session.GetString("userInfo");
            return userInfoJson == null ? default : JsonSerializer.Deserialize<Utilisateur>(userInfoJson);
        }
    }
}


