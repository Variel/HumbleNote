using HumbleNote.Domain;
using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Transfer;
using HumbleNote.Http.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HumbleNote.Controllers;

[ApiController]
[Route("notes")]
public class NoteContorller : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NoteContorller> _logger;

    const string userId = "01GJWW8R1CMPB4E1N1M6CN5QWV";

    public NoteContorller(ILogger<NoteContorller> logger, INoteService noteService)
    {
        _logger = logger;
        _noteService = noteService;
    }

    [HttpGet(Name = "List notes")]
    public async Task<IEnumerable<NoteDto>?> ListNotes(string? cursor = null)
    {
        var result = await _noteService.ListNoteAsync(userId, cursor,
            HttpContext.RequestAborted);

        return result.Notes;
    }

    [HttpPost(Name = "Post new note")]
    public async Task<NoteDto?> PostNewNote(PostNoteRequest model)
    {
        var result = await _noteService.CreateNoteAsync(
            new CreateNoteCommand(userId, model.Content),
            HttpContext.RequestAborted);

        return result.Note;
    }

    [HttpPost("{parentId}/children", Name = "Post child note")]
    public async Task<NoteDto?> PostChildNote(string content, string parentId)
    {
        var result = await _noteService.CreateNoteAsync(
            new CreateNoteCommand(userId, content)
            {
                ParentNoteId = parentId
            },
            HttpContext.RequestAborted);

        return result.Note;
    }

    [HttpPost("{oldVersionId}/revision", Name = "Revision a note")]
    public async Task<NoteDto?> RevisionNote(string content, string oldVersionId)
    {
        var result = await _noteService.CreateNoteAsync(
            new CreateNoteCommand(userId, content)
            {
                OldVersionNoteId = oldVersionId
            },
            HttpContext.RequestAborted);

        return result.Note;
    }
}
