using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NoteApp.Core.Models;
using NoteApp.Core.Services;
using System;
using System.Threading.Tasks;

namespace NoteApp.Features.NewNote;

public partial class NewNoteViewModel : ObservableObject
{
    private readonly NoteRepository _noteRepository;

    [ObservableProperty] public string title = string.Empty;

    [ObservableProperty] public string content = string.Empty;

    public NewNoteViewModel(NoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var note = new Note
        {
            Title = Title,
            Content = Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _noteRepository.AddAsync(note);

        Title = string.Empty;
        Content = string.Empty;
    }
}