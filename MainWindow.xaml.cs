using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NoteApp.Core.Helper;
using NoteApp.Features.NewNote;
using NoteApp.Features.NoteList;

namespace NoteApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowHelper.SetSize(this, 1200, 700);
            WindowHelper.SetMinSize(this, 1200, 700);
            WindowHelper.CenterOnScreen(this);

            ContentFrame.Navigate(typeof(NoteListPage));
        }

        private void NavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is not NavigationViewItem item) return;
            switch (item.Tag)
            {
                case "NoteListPage":
                    ContentFrame.Navigate(typeof(NoteListPage));
                    break;
                case "NewNotePage":
                    ContentFrame.Navigate(typeof(NewNotePage));
                    break;
            }
        }
    }
}