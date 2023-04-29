namespace ASP.Data.Entity;

public class EmailConfirmationToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public String UserEmail { get; set; } = null!;
    public DateTime Moment { get; set; }
    public int Used { get; set; } = 0;
}