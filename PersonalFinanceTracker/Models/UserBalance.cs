namespace PersonalFinanceTracker.Models
{
    public class UserBalance
    {
        public int UserId { get; set; }
        public decimal Balance { get; internal set; }
    }
}
