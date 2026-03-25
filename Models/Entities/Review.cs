namespace StoRvStar.Models.Entities;

public class Review
{
    public int Id { get; set; }

    public string Content { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}