using SmoothNotes.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmoothNotes.Services.Storage
{
    public static class ValueParserService
    {
        public static ProfileDTO profileDTO { get; set; }
        public static Profile profile { get; set; }
        public static Folder folder { get; set; }
        public static Note note { get; set; }
        public static string token { get; set; }

        public static async Task<bool> Empty()
        {
            try
            {
                profileDTO = null;
                note = null;
                folder = null;
                token = null;
                profile = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
