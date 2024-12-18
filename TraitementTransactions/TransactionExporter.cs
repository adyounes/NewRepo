 using System.Data.Common;
using System.Text.Json;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text.Json;
using BankingConsoleApp;
using System.Security.Cryptography;
using System.Text;

public class TransactionExporter
{
    private static readonly string Key = "YourSecureKey123"; // 16 caractères
    private static readonly string IV = "YourInitVector12"; // 16 caractères
    private readonly DbConnection _dbConnection;

    public TransactionExporter(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public void ExportTransactionsToJson(string filePath)
    {
        var transactions = new List<Transaction>();

        // Requête pour extraire toutes les transactions
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = "SELECT CardNumber, AccountId, Amount, TransactionDate, TransactionType, Description, Currency FROM Transactions";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Créer un objet Transaction avec les données de la base
                    var transaction = new Transaction
                    {
                        CardNumber = Decrypt(reader["CardNumber"].ToString()),
                        AccountId = Convert.ToInt32(reader["AccountId"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                        TransactionType = reader["TransactionType"].ToString(),
                        Description = reader["Description"].ToString(),
                        Currency = reader["Currency"].ToString()
                    };

                    transactions.Add(transaction);
                }
            }
        }

        // Sérialiser les transactions en JSON
        string jsonString = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });

        // Écrire dans un fichier JSON
        File.WriteAllText(filePath, jsonString);

        Console.WriteLine($"Exportation terminée : {filePath}");
    }

    public static string Decrypt(string cipherText)
    {
        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
     
    }
}

