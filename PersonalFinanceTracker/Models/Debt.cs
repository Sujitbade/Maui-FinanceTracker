public class Debt
{
    public string Source { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string Status { get; set; }

    public int UserId { get; set; }
}
