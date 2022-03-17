using System;
using System.Collections.Generic;
using System.Text;

namespace SmoothNotes.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime CrDate { get; set; }
        public DateTime EdDate { get; set; }
    }
}
