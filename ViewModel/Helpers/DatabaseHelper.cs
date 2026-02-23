using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NotesApp.Model;

namespace NotesApp.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        private static string dbPath = "PUT_YOUR_FIREBASE_DATABASE_URL_HERE";

        public static async Task<List<T>> Read<T>()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                var jsonResult = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);

                    // dacă T are proprietatea "Id", o setăm cu cheia Firebase
                    var idProp = typeof(T).GetProperty("Id");
                    if (idProp != null && idProp.CanWrite && idProp.PropertyType == typeof(string))
                    {
                        foreach (var kv in objects)
                        {
                            if (kv.Value != null)
                                idProp.SetValue(kv.Value, kv.Key);
                        }
                    }

                    return objects.Values.ToList();

                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<bool> Insert<T>(T item)
        {
            var jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.PostAsync($"{dbPath}{typeof(T).Name.ToLower()}.json", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<bool> Update<T>(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            string id = idProperty.GetValue(item).ToString();

            var jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
           
            using (var client = new HttpClient())
            {
                var result = await client.PatchAsync($"{dbPath}{typeof(T).Name.ToLower()}/{id}.json", content);

                if (result.IsSuccessStatusCode )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
