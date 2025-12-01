using Microsoft.UI.Xaml.Controls;
using NoteApp.Core.Services;

namespace NoteApp.Features.NewNote;

public sealed partial class NewNotePage : Page
{
    public NewNoteViewModel ViewModel { get; }

    public NewNotePage()
    {
        InitializeComponent();

        var repo = new NoteRepository();
        ViewModel = new NewNoteViewModel(repo);

        DataContext = ViewModel;
    }
}