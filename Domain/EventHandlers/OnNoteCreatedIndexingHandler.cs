using System.Text.RegularExpressions;
using HumbleNote.Domain.Exceptions;
using HumbleNote.Domain.Models.Events;
using HumbleNote.Persistence.Models;
using HumbleNote.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HumbleNote.Domain.EventHandlers;

public class OnNoteCreatedIndexingHandler : INotificationHandler<NoteCreatedEvent>
{
    private readonly DatabaseContext _database;

    public OnNoteCreatedIndexingHandler(DatabaseContext database)
    {
        _database = database;
    }

    public async Task Handle(NoteCreatedEvent notification, CancellationToken cancellationToken)
    {
        var createdNote
            = await _database.Notes.FirstOrDefaultAsync(n => n.Id == notification.NoteId,
                                                        cancellationToken);

        if (createdNote is null)
        {
            throw new NoteNotFoundException();
        }

        await HandleHashTags(createdNote, cancellationToken);
        await HandleMentions(createdNote, cancellationToken);
    }

    private async Task HandleHashTags(Note createdNote, CancellationToken cancellationToken)
    {
        var matches = Regex.Matches(createdNote.Content, @"#(?<tag>[\w\d_]{1,50})(?>\s|$)");
        var hashTags = matches.Select(m => m.Groups["tag"].Value.ToLower())
                              .Distinct()
                              .ToArray();

        var indexEntries = hashTags.Select(tag =>
            new HashTagIndex(createdNote.UserId, tag, createdNote.Id, createdNote.Timestamp));

        _database.HashTagIndices.AddRange(indexEntries);

        var existingTags = await _database.HashTags
            .Where(tag => tag.UserId == createdNote.UserId && hashTags.Contains(tag.TagValue))
            .Select(tag => tag.TagValue)
            .ToArrayAsync(cancellationToken);

        var tagsToAdd = hashTags.Except(existingTags)
                                .Select(tag => new HashTag(createdNote.UserId, tag)
                                {
                                    CreatedAt = createdNote.Timestamp,
                                    LastUsedAt = createdNote.Timestamp
                                });

        _database.HashTags.AddRange(tagsToAdd);

        await _database.HashTags
            .Where(tag => tag.UserId == createdNote.UserId && existingTags.Contains(tag.TagValue))
            .ExecuteUpdateAsync(p =>
                p.SetProperty(tag => tag.LastUsedAt, _ => createdNote.Timestamp),
                cancellationToken);
    }

    private async Task HandleMentions(Note createdNote, CancellationToken cancellationToken)
    {
        var matches = Regex.Matches(createdNote.Content, @"@(?<mention>[a-zA-Z0-9]{26})(?>\s|$)");
        var mentionedIds = matches.Select(m => m.Groups["mention"].Value.ToLower())
                                  .Distinct()
                                  .ToArray();

        var relations = mentionedIds.Select(id => new NoteMention(createdNote.Id, id)).ToArray();
        _database.NoteMentions.AddRange(relations);

        await _database.Notes
            .Where(n => n.UserId == createdNote.UserId && mentionedIds.Contains(n.Id))
            .ExecuteUpdateAsync(p =>
                p.SetProperty(n => n.LastActivatedAt, _ => createdNote.Timestamp),
                cancellationToken);
    }
}