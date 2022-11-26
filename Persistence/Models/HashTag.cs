using System.ComponentModel.DataAnnotations;
using HumbleNote.Common;

namespace HumbleNote.Persistence.Models;

public class HashTag
{
    [MaxLength(UlidHelper.StringLength)]
    public string UserId { get; set; }
    
    [MaxLength(50)]
    public string TagValue { get; set; }

    public DateTimeOffset LastUsedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }


    public HashTag(string userId, string tagValue)
    {
        UserId = userId;
        TagValue = tagValue;
    }
}