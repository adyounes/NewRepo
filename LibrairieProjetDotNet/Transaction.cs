namespace BankingConsoleApp
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public Account Account { get; set; }
        public string Currency { get; set; }
        public string CardNumber { get; set; }

    }
}