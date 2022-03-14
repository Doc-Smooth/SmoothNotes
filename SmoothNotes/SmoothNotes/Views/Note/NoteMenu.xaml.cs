using SmoothNotes.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmoothNotes.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteMenu : Popup
    {
        private Models.Note _note;
        public Models.Note Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }
        public NoteMenu()
        {
            InitializeComponent();
            BindingContext = this;
            Note = ValueParserService.note;
        }
    }
}