using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;

class Program
{
    static void Main(string[] args)
    {
        ImportDonnes();
        ExportDonnees();
    }

    public static void ImportDonnes()
    {
        string _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);
        using DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        // Initialiser la connexion à la base de données

        try
        {
            // Ouvrir la connexion
            dbConnection.Open();

            // Initialiser le TransactionProcessor
            var transactionProcessor = new TransactionProcessor(dbConnection);

            // Spécifiez le chemin du fichier de transactions
            string filePath = "C:\\Users\\Younes\\source\\repos\\PjExoFinal2\\TraitementTransactions\\import.txt";

            // Appeler la méthode pour traiter les transactions
            transactionProcessor.ProcessTransactions(filePath);

            Console.WriteLine("Traitement des transactions terminé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }

    public static void ExportDonnees()
        string _connectionString = "BDDConnect";
    {
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);
        using DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        try
        {
            // Ouvrir la connexion
            dbConnection.Open();

            // Initialiser le TransactionProcessor
            var exporter = new TransactionExporter(dbConnection);

            // Spécifiez le chemin du fichier de transactions
            string filePath = "C:\\Users\\Younes\\source\\repos\\PjExoFinal2\\TraitementTransactions\\export.json";

            // Appeler la méthode pour traiter les transactions
            exporter.ExportTransactionsToJson(filePath);

            Console.WriteLine("Traitement des transactions terminé.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");
        }
    }
}