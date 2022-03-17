using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmoothNotes.Services.Storage
{
    public class ProfileService
    {
        static HttpClient client;

        static ProfileService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:7040/api/Profile/")
            };
        }

        /// <summary>
        /// Login on profile and get bearer token
        /// </summary>
        /// <param name="args"></param>
        /// <returns>boolean</returns>
        public static async Task<bool> Login(ProfileDTO args)
        {
            string token = "";
            var json = JsonConvert.SerializeObject(args);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("login", content);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }


            token = await response.Content.ReadAsStringAsync();
            ValueParserService.token = token;
            await SecureStorage.SetAsync("auth_token", token);


            return true;
        }

        /// <summary>
        /// Register new profile
        /// </summary>
        /// <param name="args">Username and password</param>
        /// <returns>profile id or error message</returns>
        public static async Task<string> Register(Register args)
        {
            args = await CryptographService.AddKeys(args);
            

            var json = JsonConvert.SerializeObject(args);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("register", content);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets profile data from username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Profile data</returns>
        public static async Task<Profile> GetProfile(string username)
        {
            //var json = JsonConvert.SerializeObject($"'username':'{username}'");
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var resonse = await client.GetAsync("username?username=" + username);
            Profile p = new Profile();

            var result = JsonConvert.DeserializeObject<Profile>(await resonse.Content.ReadAsStringAsync());
            //if(result.folders == null)
            //    result.folders = new List<Folder>();

            if (!resonse.IsSuccessStatusCode)
                return null;
            return result;
        }

        public static async Task<bool> Delete(string id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var resonse = await client.DeleteAsync("id?id=" + id);
            if (resonse.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> Refresh()
        {
            string token = "";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.GetAsync("refresh/username?username=" + ValueParserService.profile.Name);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            token = await response.Content.ReadAsStringAsync();
            ValueParserService.token = token;
            await SecureStorage.SetAsync("auth_token", token);
            return true;
        }
    }
}
