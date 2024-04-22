using Microsoft.JSInterop;
using SportTogetherBlazor.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Services
{
    public class LocalStorageServices
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageServices(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveUserToLocalStore(Utilisateur utilisateurInfo)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userInfo", JsonSerializer.Serialize(utilisateurInfo));
        }

        public async Task<UtilisateurInfo> GetUserFromLocalStore()
        {
            var userInfoJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userInfo");
            return userInfoJson == null ? default : JsonSerializer.Deserialize<UtilisateurInfo>(userInfoJson)!;
        }
       
    }
}
