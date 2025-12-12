using CommunityToolkit.Mvvm.ComponentModel;
using NoteApp.Core.Models;
using NoteApp.Core.Services;
using System.Collections.ObjectModel;

namespace NoteApp.Features.NoteList;

public partial class NoteListViewModel : ObservableObject
{
    private readonly NoteRepository _noteRepository;

    public ObservableCollection<Note> Notes { get; } = new();

    public NoteListViewModel(NoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
        LoadNotes();
    }

    private async void LoadNotes()
    {
        var notes = await _noteRepository.GetAllAsync();
        Notes.Clear();
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
    }
}