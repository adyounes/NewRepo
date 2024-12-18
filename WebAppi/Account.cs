using BankingConsoleApp;

namespace WebAPI.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; } // Pro or Private
        public string Address { get; set; } // For Pro accounts

        public Client Client { get; set; }// Navigation property
        public int ClientID { get; set; }

    }
}
