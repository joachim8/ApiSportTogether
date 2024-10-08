using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SportTogetherBlazor.Models;

namespace SportTogetherBlazor.Services
{
    public class TaskNotificationServices
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/ApiSportTogether/NotificationUtilisateur";

        public TaskNotificationServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all notifications
        public async Task<(bool IsSuccess, List<NotificationUtilisateur> Notifications, string ErrorMessage)> GetNotificationsAsync()
        {
            try
            {
                var notifications = await _httpClient.GetFromJsonAsync<List<NotificationUtilisateur>>(BaseUrl);
                return (true, notifications, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erreur lors de la récupération des notifications : {ex.Message}");
            }
        }

        // Get a notification by ID
        public async Task<(bool IsSuccess, NotificationUtilisateur Notification, string ErrorMessage)> GetNotificationByIdAsync(int id)
        {
            try
            {
                var notification = await _httpClient.GetFromJsonAsync<NotificationUtilisateur>($"{BaseUrl}/{id}");
                return (true, notification, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erreur lors de la récupération de la notification : {ex.Message}");
            }
        }

        // Get notifications by user ID
        public async Task<(bool IsSuccess, List<NotificationUtilisateur> Notifications, string ErrorMessage)> GetNotificationsByUserIdAsync(int userId)
        {
            try
            {
                var notifications = await _httpClient.GetFromJsonAsync<List<NotificationUtilisateur>>($"{BaseUrl}/GetByUserId/{userId}");
                return (true, notifications, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erreur lors de la récupération des notifications de l'utilisateur : {ex.Message}");
            }
        }

        // Create a new notification
        public async Task<(bool IsSuccess, NotificationUtilisateur Notification, string ErrorMessage)> CreateNotificationAsync(NotificationUtilisateur notification)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/CreateNotification", notification);
                response.EnsureSuccessStatusCode();

                var createdNotification = await response.Content.ReadFromJsonAsync<NotificationUtilisateur>();
                return (true, createdNotification, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Erreur lors de la création de la notification : {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Erreur inconnue : {ex.Message}");
            }
        }

        // Update an existing notification
        public async Task<(bool IsSuccess, string ErrorMessage)> UpdateNotificationAsync(NotificationUtilisateur notification)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{notification.NotificationId}", notification);
                response.EnsureSuccessStatusCode();
                return (true, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erreur lors de la mise à jour de la notification : {ex.Message}");
            }
        }

        // Delete a notification
        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteNotificationAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                return (true, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erreur lors de la suppression de la notification : {ex.Message}");
            }
        }

        // Mark a notification as read
        public async Task<(bool IsSuccess, string ErrorMessage)> MarkNotificationAsReadAsync(int id)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"{BaseUrl}/MarkAsRead/{id}", null);
                response.EnsureSuccessStatusCode();
                return (true, null);
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Erreur lors de la mise à jour de l'état de la notification : {ex.Message}");
            }
        }
    }
}
