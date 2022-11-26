using System.ComponentModel.DataAnnotations;
using HumbleNote.Common;

namespace HumbleNote.Persistence.Models;

public class User
{
    [MaxLength(UlidHelper.StringLength)]
    public string Id { get; set; } = Ulid.NewUlid().ToString();

    public string UserName { get; set; }

    public string Name { get; set; }
}