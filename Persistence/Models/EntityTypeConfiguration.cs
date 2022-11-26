using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HumbleNote.Persistence.Models;

public class EntityTypeConfiguration : IEntityTypeConfiguration<Note>,
                                       IEntityTypeConfiguration<HashTag>,
                                       IEntityTypeConfiguration<NoteMention>,
                                       IEntityTypeConfiguration<HashTagIndex>
{
    void IEntityTypeConfiguration<Note>.Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(n1 => n1.RootNote)
            .WithMany()
            .HasForeignKey(n1 => n1.RootNoteId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n1 => n1.ParentNote)
            .WithMany(n2 => n2.ChildrenNotes)
            .HasForeignKey(n1 => n1.ParentNoteId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n1 => n1.OldVersionNote)
            .WithOne(n2 => n2.NewVersionNote)
            .HasForeignKey<Note>(n1 => n1.OldVersionNoteId);

        builder.HasIndex(n => new { n.UserId, n.Id })
            .IsDescending()
            .IsUnique();

        builder.HasIndex(n => new { n.UserId, n.LastActivatedAt })
            .IsDescending();
    }

    void IEntityTypeConfiguration<HashTag>.Configure(EntityTypeBuilder<HashTag> builder)
    {
        builder.HasKey(t => new { t.UserId, t.TagValue });

        builder.HasIndex(t => new { t.UserId, t.LastUsedAt, t.TagValue })
            .IsDescending();
    }

    void IEntityTypeConfiguration<NoteMention>.Configure(EntityTypeBuilder<NoteMention> builder)
    {
        builder.HasKey(m => new { m.NoteId, m.MentionedNoteId });

        builder.HasOne(m => m.Note)
            .WithMany(n => n.NoteMentions)
            .HasForeignKey(m => m.NoteId);

        builder.HasOne(m => m.MentionedNote)
            .WithMany(n => n.NoteMentionBackLinks)
            .HasForeignKey(m => m.MentionedNoteId);
    }

    void IEntityTypeConfiguration<HashTagIndex>.Configure(EntityTypeBuilder<HashTagIndex> builder)
    {
        builder.HasKey(idx => new { idx.UserId, idx.HashTag, idx.NoteId });

        builder.HasIndex(idx => new { idx.UserId, idx.HashTag, idx.Timestamp })
            .IsDescending();
    }
}