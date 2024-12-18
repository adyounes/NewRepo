using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class TransactionProcessor
{
    private readonly DbConnection _dbConnection;

    private static readonly string Key = "YourSecureKey123"; // 16 caractères
    private static readonly string IV = "YourInitVector12"; // 16 caractères
    private string encryptedCardNumber;

    public TransactionProcessor(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public void ProcessTransactions(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Fichier non trouvé : {filePath}");
            return;
        }

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var columns = line.Split(',');

            // Vérifiez si le format est correct (par exemple, 6 colonnes attendues)
            if (columns.Length != 7)
            {
                CurrencyConverter converter = new CurrencyConverter();
                int accountId = int.Parse(columns[1].Trim());
                decimal amount = decimal.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);
                string currency = columns[5].Trim();
                converter.AmountCurrency(amount, currency);
                DateTime transactionDate = DateTime.Parse(columns[3].Trim(), CultureInfo.InvariantCulture);
                InsertAnomaly(accountId, amount, transactionDate, "Format incorrect", "Le nombre de colonnes est invalide.");
                continue;
            }

            try
            {
                CurrencyConverter converter = new CurrencyConverter();
                // Parse des colonnes
                // Extraire et convertir les valeurs
                string cardNumber = columns[0].Trim();
                int accountId = int.Parse(columns[1].Trim());
                decimal amount = decimal.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);
                DateTime transactionDate = DateTime.Parse(columns[3].Trim(), CultureInfo.InvariantCulture);
                string transactionType = columns[4].Trim();
                string currency = columns[5].Trim();
                string description = columns[6].Trim();
                converter.AmountCurrency(amount, currency);
                // Vérifications de validité
                if (!IsValidCardNumber(cardNumber))
                {
                    InsertAnomaly(accountId, amount, transactionDate, "Numéro de carte invalide", description);
                    continue;
                }

                if (amount <= 0)
                {
                    InsertAnomaly(accountId, amount, transactionDate, "Montant invalide", description);
                    continue;
                }
                // Insérer dans la table Transactions
                InsertTransaction(cardNumber, accountId, amount, transactionDate, transactionType, description, currency);
            }
            catch (Exception ex)
            {
                CurrencyConverter converter = new CurrencyConverter();
                int accountId = int.Parse(columns[1].Trim());
                decimal amount = decimal.Parse(columns[2].Trim(), CultureInfo.InvariantCulture);
                string currency = columns[5].Trim();
                converter.AmountCurrency(amount, currency);
                DateTime transactionDate = DateTime.Parse(columns[3].Trim(), CultureInfo.InvariantCulture);
                string description = columns[6].Trim();
                InsertAnomaly(accountId, amount, transactionDate, "Erreur d'analyse", description);
            }
        }
    }
    public class CurrencyConverter
    {
        // Dictionnaire des taux de change par rapport à une devise de référence (ex: EUR)
        private static readonly Dictionary<string, decimal> ExchangeRates = new Dictionary<string, decimal>
    {
        { "USD", 1.10m }, // Exemple : 1 EUR = 1.10 USD
        { "EUR", 1.00m }, // 1 EUR = 1 EUR
        { "GBP", 0.85m }, // 1 EUR = 0.85 GBP
        { "JPY", 125.00m } // 1 EUR = 125 JPY
    };

        /// <summary>
        /// Convertit un montant dans une devise cible.
        /// </summary>
        /// <param name="amount">Le montant à convertir.</param>
        /// <param name="currency">La devise cible (ex: "USD", "EUR").</param>
        /// <returns>Le montant converti ou une exception si la devise est inconnue.</returns>
        public decimal AmountCurrency(decimal amount, string currency)
        {
            if (string.IsNullOrEmpty(currency) || !ExchangeRates.ContainsKey(currency.ToUpper()))
            {
                throw new ArgumentException($"Devise inconnue : {currency}");
            }

            decimal exchangeRate = ExchangeRates[currency.ToUpper()];
            return amount * exchangeRate;
        }
    }
    private bool IsValidCardNumber(string cardNumber)
    {
        // Exemple de validation : vérifier si le numéro de carte contient exactement 16 chiffres
        return cardNumber.Length == 16 && long.TryParse(cardNumber, out _);
    }

    private void InsertTransaction(string cardNumber, int accountId, decimal amount, DateTime transactionDate, string transactionType, string description, string currency)
    {
        encryptedCardNumber = Encrypt(cardNumber);
        Console.WriteLine(encryptedCardNumber);


        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Transactions (AccountId, Amount, TransactionDate, TransactionType, Description,Currency,CardNumber) " +
                                  "VALUES (@AccountId, @Amount, @TransactionDate, @TransactionType, @Description,@Currency, @CardNumber )";

            AddParameter(command, "@CardNumber", encryptedCardNumber);
            AddParameter(command, "@AccountId", accountId);
            AddParameter(command, "@Amount", amount);
            AddParameter(command, "@Currency", currency);
            AddParameter(command, "@TransactionDate", transactionDate);
            AddParameter(command, "@TransactionType", transactionType);
            AddParameter(command, "@Description", description);

            command.ExecuteNonQuery();
        }
    }

    private void InsertAnomaly(int? accountId, decimal amount, DateTime transactionDate, string anomalyType, string description)
    {
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Anomalies (AccountId,Amount, AnomalieDate, AnomalieType, Description) " +
                                  "VALUES (@AccountId,@Amount, @AnomalieDate, @AnomalieType, @Description)";

            AddParameter(command, "@AccountId", accountId);
            AddParameter(command, "@Amount", amount);
            AddParameter(command, "@AnomalieDate", transactionDate);
            AddParameter(command, "@AnomalieType", anomalyType);
            AddParameter(command, "@Description", description);

            command.ExecuteNonQuery();
        }
    }

    private void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }



    public static string Encrypt(string plainText)
    {
        try
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Le texte à chiffrer ne peut pas être vide ou nul.", nameof(plainText));

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(plainText);
                        }
                    }
                    // Retourne le résultat du chiffrement en Base64
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
            
        } 
      
      
    }
}

