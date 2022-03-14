using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmoothNotes.Services.Storage
{
    public static class FolderService
    {
        static HttpClient client;
        static FolderService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:7040/api/Folder/")
            };
        }


        /// <summary>
        /// Get folders of a profile from id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<List<Folder>> GetProfileFolders()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var resonse = await client.GetAsync("profileid?profileid=" + ValueParserService.profile.Id);
            try
            {
                var result = JsonConvert.DeserializeObject<List<Folder>>(await resonse.Content.ReadAsStringAsync());
                if (!string.IsNullOrEmpty(result[0].Id))
                {
                    ValueParserService.profile.folders = result;
                    return result;
                }
                else
                {
                    return new List<Folder>();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Create new folder to a profile with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task<bool> Add(string name)
        {
            //folder.ProfileId = ValueParserService.profile.Id;
            //var json = JsonConvert.SerializeObject(folder);
            var json = JsonConvert.SerializeObject(name);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.PostAsync("add/id?id=" + ValueParserService.profile.Id, content);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public static async Task<bool> Edit(Folder arg)
        {
            var json = JsonConvert.SerializeObject(arg);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.PutAsync("edit", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async static Task<bool> Delete(string id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.DeleteAsync("id?id=" + id);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }

}
