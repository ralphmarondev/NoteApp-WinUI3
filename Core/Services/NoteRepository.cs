using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NoteApp.Core.Models;

namespace NoteApp.Core.Services;

public class NoteRepository
{
    private readonly string _connectionString;

    public NoteRepository()
    {
        var fullPath = Path.Combine(AppContext.BaseDirectory, "notes.db");
        _connectionString = $"Data Source={fullPath}";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS Notes (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Content TEXT NOT NULL,
            CreatedAt TEXT NOT NULL,
            UpdatedAt TEXT);";
        tableCmd.ExecuteNonQuery();
    }

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        var notes = new List<Note>();

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var selectedCmd = connection.CreateCommand();
        selectedCmd.CommandText = "SELECT * FROM Notes";

        await using var reader = await selectedCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            notes.Add(new Note
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Content = reader.GetString(2),
                CreatedAt = DateTime.Parse(reader.GetString(3)),
                UpdatedAt = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4))
            });
        }

        return notes;
    }

    public async Task AddAsync(Note note)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt)
              VALUES ($title, $content, $createdAt, $updatedAt);";

        insertCmd.Parameters.AddWithValue("$title", note.Title ?? "");
        insertCmd.Parameters.AddWithValue("$content", note.Content ?? "");
        insertCmd.Parameters.AddWithValue("$createdAt", note.CreatedAt.ToString("o"));
        insertCmd.Parameters.AddWithValue("$updatedAt", (object?)note.UpdatedAt?.ToString("o") ?? DBNull.Value);

        await insertCmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var updateCmd = connection.CreateCommand();
        updateCmd.CommandText =
            @"UPDATE Notes
              SET Title = $title, Content = $content, UpdatedAt = $updatedAt
              WHERE Id = $id";
        updateCmd.Parameters.AddWithValue("$id", note.Id);
        updateCmd.Parameters.AddWithValue("$title", note.Title);
        updateCmd.Parameters.AddWithValue("$content", note.Content ?? "");
        updateCmd.Parameters.AddWithValue("$updatedAt", note.UpdatedAt?.ToString("o"));

        await updateCmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Notes WHERE Id = $id";
        deleteCmd.Parameters.AddWithValue("$id", id);

        await deleteCmd.ExecuteNonQueryAsync();
    }
}