using BankingConsoleApp;
namespace BankingConsoleApp
{
    public class Account
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public DateTime OpenDateAccount { get; set; }
        public int ClientID { get; set; }

        public override string ToString()
        {
            return $"ID: {AccountId}, Date d'ouverture : {OpenDateAccount} Solde: {Balance}";
        }
        public Account()
        {
            // Constructeur par défaut
        }
    }
}
