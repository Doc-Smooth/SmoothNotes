using System;
using System.Collections.Generic;
using System.Text;

namespace SmoothNotes.Models
{
    public class Folder
    {
        public string Id { get; set; }
        public string ProfileId { get; set; }
        public string Name { get; set; }
        public bool Fav { get; set; }
        public List<Note> Notes { get; set; }

    }
}
