using System;
using System.Collections.Generic;
using System.Text;

namespace SmoothNotes.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PuK { get; set; }
        public List<Folder> folders { get; set; }
    }
}
