using Microsoft.UI.Xaml.Controls;
using NoteApp.Core.Services;

namespace NoteApp.Features.NoteList;

public sealed partial class NoteListPage : Page
{
    public NoteListViewModel ViewModel { get; }

    public NoteListPage()
    {
        InitializeComponent();

        var repo = new NoteRepository();
        ViewModel = new NoteListViewModel(repo);

        DataContext = ViewModel;
    }
}