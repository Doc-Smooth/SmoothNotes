using Newtonsoft.Json;
using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SmoothNotes.Services.Storage
{
    public static class BiometricService
    {
        public async static Task<List<string>> BiometricStatus()
        {
            List<string> bValue;
            var bioJson = "";
            bioJson = await SecureStorage.GetAsync("bio_status");
            if (string.IsNullOrEmpty(bioJson))
                bValue = new List<string>();
            else
                bValue = JsonConvert.DeserializeObject<List<string>>(bioJson);
            return bValue;
        }

        public async static Task BioStatusChange(string username)
        {
            List<string> bValue;
            var bioJson = "";
            bioJson = await SecureStorage.GetAsync("bio_status");
            if(string.IsNullOrEmpty(bioJson))
                bValue = new List<string>();
            else
                bValue = JsonConvert.DeserializeObject<List<string>>(bioJson);

            var found = false;
            foreach (var item in bValue)
            {
                if(item == username)
                {
                    found = true;
                }
            }
            if(!found)
                bValue.Add(username);
            else
                bValue.Remove(username);

            bioJson = JsonConvert.SerializeObject(bValue);
            await SecureStorage.SetAsync("bio_status", bioJson);
        }

        public async static Task UpdateList(string username)
        {
            List<string> bValue;
            var bioJson = "";
            bioJson = await SecureStorage.GetAsync("bio_status");
            if (string.IsNullOrEmpty(bioJson))
                return;
            else
                bValue = JsonConvert.DeserializeObject<List<string>>(bioJson);

            var found = false;
            foreach (var item in bValue)
            {
                if (item == username)
                {
                    found = true;
                }
            }
            if (found)
                bValue.Remove(username);

            bioJson = JsonConvert.SerializeObject(bValue);
            await SecureStorage.SetAsync("bio_status", bioJson);
        }
        public async static Task FirstTimeSetup()
        {
            List<string> bValue = new List<string>();
            var bioJson = await SecureStorage.GetAsync("bio_status");
            if (string.IsNullOrEmpty(bioJson))
                await SecureStorage.SetAsync("bio_status", bioJson);
        }
    }
}
