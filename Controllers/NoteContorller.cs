using HumbleNote.Domain;
using HumbleNote.Domain.Models.Commands;
using HumbleNote.Domain.Models.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace HumbleNote.Controllers;

[ApiController]
[Route("notes")]
public class NoteContorller : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NoteContorller> _logger;

    public NoteContorller(ILogger<NoteContorller> logger, INoteService noteService)
    {
        _logger = logger;
        _noteService = noteService;
    }

    [HttpGet("add", Name = "GetWeatherForecast")]
    public async Task<NoteDto?> Get(string content, string? parentId = null, string? OldVersionId = null)
    {
        var result = await _noteService.CreateNoteAsync(new CreateNoteCommand("01GJVE3WWKSR7FM2KW0N11FYN2", content)
        {
            ParentNoteId = parentId,
            OldVersionNoteId = OldVersionId
        });

        return result.Note;
    }
}
