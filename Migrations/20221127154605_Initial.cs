using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumbleNote.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HashTags",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    TagValue = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastUsedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashTags", x => new { x.UserId, x.TagValue });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    RootNoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: true),
                    ParentNoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: true),
                    NewVersionNoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: true),
                    OldVersionNoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastActivatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Notes_NewVersionNoteId",
                        column: x => x.NewVersionNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Notes_ParentNoteId",
                        column: x => x.ParentNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notes_Notes_RootNoteId",
                        column: x => x.RootNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HashTagIndices",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    HashTag = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashTagIndices", x => new { x.UserId, x.HashTag, x.NoteId });
                    table.ForeignKey(
                        name: "FK_HashTagIndices_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteMentions",
                columns: table => new
                {
                    NoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false),
                    MentionedNoteId = table.Column<string>(type: "TEXT", maxLength: 26, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteMentions", x => new { x.NoteId, x.MentionedNoteId });
                    table.ForeignKey(
                        name: "FK_NoteMentions_Notes_MentionedNoteId",
                        column: x => x.MentionedNoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteMentions_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "UserName" },
                values: new object[] { "01GJWW8R1CMPB4E1N1M6CN5QWV", "예차니", "yechanism" });

            migrationBuilder.CreateIndex(
                name: "IX_HashTagIndices_NoteId",
                table: "HashTagIndices",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_HashTagIndices_UserId_HashTag_Timestamp",
                table: "HashTagIndices",
                columns: new[] { "UserId", "HashTag", "Timestamp" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_HashTags_UserId_LastUsedAt_TagValue",
                table: "HashTags",
                columns: new[] { "UserId", "LastUsedAt", "TagValue" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_NoteMentions_MentionedNoteId",
                table: "NoteMentions",
                column: "MentionedNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NewVersionNoteId",
                table: "Notes",
                column: "NewVersionNoteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OldVersionNoteId",
                table: "Notes",
                column: "OldVersionNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentNoteId",
                table: "Notes",
                column: "ParentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_RootNoteId",
                table: "Notes",
                column: "RootNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId_Id",
                table: "Notes",
                columns: new[] { "UserId", "Id" },
                unique: true,
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId_LastActivatedAt",
                table: "Notes",
                columns: new[] { "UserId", "LastActivatedAt" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HashTagIndices");

            migrationBuilder.DropTable(
                name: "HashTags");

            migrationBuilder.DropTable(
                name: "NoteMentions");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
