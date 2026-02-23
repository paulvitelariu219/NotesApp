using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using NotesApp.Model;

namespace NotesApp.ViewModel.Helpers
{
    public class FirebaseHelper
    {
        private static string api_key = "PUT_YOUR_FIREBASE_API_KEY_HERE";
        public class FirebaseResult
        {
            public string kind { get; set; }
            public string idToken { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
            public string localId { get; set; }
        }
        public class ErrorDetails
        {
            public int code { get; set; }
            public string message { get; set; }
        }
        public class Error
        {
            public ErrorDetails error { get; set; }
        }

        public static async Task<bool> Login(User user)
        {
            using (var client = new HttpClient())
            {
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };

                var bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={api_key}", data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }

        public static async Task<bool> Register(User user)
        {
            using (var client = new HttpClient())
            {
                var body = new
                {
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    password = user.Password,
                    confirmPassword = user.ConfirmPassword,
                    returnSecureToken = true
                };

                var bodyJson = JsonConvert.SerializeObject(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={api_key}", data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<Error>(errorJson);
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }
    }
}
