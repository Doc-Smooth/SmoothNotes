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
    public static class NoteService
    {
        static HttpClient client;
        static NoteService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:7040/api/Note/")
            };
        }

        public static async Task<bool> Gather()
        {
            List<Note> notes = new List<Note>();
            notes = await GetNotes(ValueParserService.folder.Id);
            List<Note> dNote = new List<Note>();
            if (notes.Count > 0 && notes != null)
            {
                dNote = await CryptographService.DecryptNotes(notes);
                ValueParserService.folder.Notes = dNote;
                return true;
            }
            else
            {
                ValueParserService.folder.Notes = new List<Note>();
                return false;
            }

        }

        private static async Task<List<Note>> GetNotes(string id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.GetAsync("folderid?folderid=" + id);
            try
            {
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<Note>();
                }
                else
                {
                    var result = JsonConvert.DeserializeObject<List<Note>>(await response.Content.ReadAsStringAsync());
                    return result;
                }

            }
            catch (Exception)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        internal static async Task<bool> Add(Note note)
        {
            var json = JsonConvert.SerializeObject(note);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.PostAsync("add", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        internal static async Task<bool> Edit(Note note)
        {
            var json = JsonConvert.SerializeObject(note);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.PutAsync("edit", content);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        internal static async Task<bool> Remove(string id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("auth_token"));
            var response = await client.DeleteAsync(id);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
