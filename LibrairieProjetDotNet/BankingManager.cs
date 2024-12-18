using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using BankingConsoleApp;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Data.SqlClient;
using NLog.Internal;
using System.Configuration;
using LibrairieProjetDotNet;
using Newtonsoft.Json;
using System.Transactions;
using Microsoft.Identity.Client;
using System.Text;

public class BankingManager
{
    private string _connectionString;
    private readonly List<Client> clients = [];
    private readonly List<Account> accounts = [];
    private readonly List<BankingConsoleApp.Transaction> transactions = [];
    private readonly List<Account> _accounts;
    private readonly List<BankingConsoleApp.Transaction> _transactions;

    public BankingManager()
    {
        _accounts = accounts;
        _transactions = transactions;
    }
    public void CreatePriveeClient(string Name, string FirstName, string city, string postalCode, string complement, string street, string ContactMail, DateTime BirthDate, string sexe)
    {
        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
            ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);

        using DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();

        // Insérer un client et récupérer son ID
        int clientId;
        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO ClientsPrive (FirstName,BirthDate,Sexe,Name,City,PostalCode,Complement,Street,ContactMail) " +
                                  "VALUES ( @FirstName, @BirthDate, @Sexe,@Name,@city,@postalCode,@complement,@street, @ContactMail)" +
                                  "SELECT SCOPE_IDENTITY();"; // Récupère l'ID généré

            AddParameter(command, "@Name", Name);
            AddParameter(command, "@FirstName", FirstName);
            AddParameter(command, "@city", city);
            AddParameter(command, "@street", street);
            AddParameter(command, "@complement", complement);
            AddParameter(command, "@postalCode", postalCode);
            AddParameter(command, "@ContactMail", ContactMail);
            AddParameter(command, "@BirthDate", BirthDate);
            AddParameter(command, "@Sexe", sexe);


            // Exécution de la commande
            command.ExecuteNonQuery();
            // Exécute la commande et récupère l'ID du client inséré
            clientId = Convert.ToInt32(command.ExecuteScalar());
        }
        int AccountId;
        // Insérer un compte en utilisant l'ID du client
        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Accounts ( Balance,AccountType, OpenDateAccount, ClientId) " +
                                  "VALUES (@Balance,@AccountType,@OpenDateAccount,@ClientId)" +
                                  "SELECT SCOPE_IDENTITY();";

            AddParameter(command, "@OpenDateAccount", DateTime.Now);
            AddParameter(command, "@Balance", 1000);
            AddParameter(command, "@AccountType", "Private");
            AddParameter(command, "@ClientId", clientId); // Utiliser l'ID récupéré

            // Exécution de la commande
            command.ExecuteNonQuery();

            AccountId = Convert.ToInt32(command.ExecuteScalar());
        }

       
        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO CarteBleue (CardNumber,AccountId23AccountId) " +
                                  "VALUES (@CardNumber, @AccountId)";


            AddParameter(command, "@CardNumber", GenerateCardNumber());
            AddParameter(command, "@AccountId", AccountId);

            // Exécution de la commande
            command.ExecuteNonQuery();
        }

        dbConnection.Close();
    }

    public void CreateProClient(string Name, string city, string postalCode, string complement, string street, string ContactMail, string siret, string legalStatus,string street2,string complement2,string postalCode2,string city2)
    {
        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
            ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);
        // Insérer un client et récupérer son ID
        int clientId;
        using DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();

        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO ClientsPro (siret,legalStatus,Procity,PropostalCode,Procomplement,Prostreet,Name,city,postalcode,complement,street,ContactMail) " +
                                  "VALUES ( @siret,@legalStatus,@Procity,@PropostalCode,@Procomplement,@Prostreet,@Name,@city,@postalcode,@complement,@street,@ContactMail )" +
                                  "SELECT SCOPE_IDENTITY();"; // Récupère l'ID généré



            AddParameter(command, "@Name", Name);
            AddParameter(command, "@city", city);
            AddParameter(command, "@street", street);
            AddParameter(command, "@complement", complement);
            AddParameter(command, "@postalCode", postalCode);
            AddParameter(command, "@ContactMail", ContactMail);
            AddParameter(command, "@siret", siret);
            AddParameter(command, "@legalStatus", legalStatus);
            AddParameter(command, "@Procity", city2);
            AddParameter(command, "@Prostreet", street2);
            AddParameter(command, "@Procomplement", complement2);
            AddParameter(command, "@PropostalCode", postalCode2);
            // Exécution de la commande
            command.ExecuteNonQuery();
            // Exécute la commande et récupère l'ID du client inséré
            clientId = Convert.ToInt32(command.ExecuteScalar());
        }
        int AccountId;
        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Accounts (OpenDateAccount,Balance,AccountType,clientId) " +
                                 "VALUES (@OpenDateAccount, @Balance,@AccountType,@ClientId)" +
                                  "SELECT SCOPE_IDENTITY()";


            AddParameter(command, "@OpenDateAccount", DateTime.Now);
            AddParameter(command, "@Balance", 1000);
            AddParameter(command, "@AccountType", "Pro");
            AddParameter(command, "@ClientId", clientId); // Utiliser l'ID récupéré

            // Exécution de la commande
            command.ExecuteNonQuery();

            AccountId = Convert.ToInt32(command.ExecuteScalar());
        }
        using (DbCommand command = dbConnection.CreateCommand())
        {
            command.CommandText = "INSERT INTO CarteBleue (CardNumber,AccountId23AccountId) " +
                                  "VALUES (@CardNumber, @AccountId)";


            AddParameter(command, "@CardNumber", GenerateCardNumber());
            AddParameter(command, "@AccountId", AccountId);

            // Exécution de la commande
            command.ExecuteNonQuery();
        }

        dbConnection.Close();
    }
    // Récupérer tous les contacts
    public List<Client> RecupererTousContactsPro()
    {


        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);

        DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();
        DbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT Id,Siret,ProCity,ProPostalCode,ProComplement,ProStreet,Name,City,PostalCode,Complement,Street, ContactMail FROM CLIENTSPRO";

        DbDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            clients.Add(new ProfessionnalClient
            {
                Id = (int)rdr["Id"],
                Siret = (string)rdr["Siret"],
                ProCity = rdr["ProCity"] as string,
                ProPostalCode = rdr["ProPostalCode"] as string,
                ProComplement = rdr["ProStreet"] as string,
                Name = rdr["Name"] as string,
                City = rdr["City"] as string,
                PostalCode = rdr["PostalCode"] as string,
                Complement = rdr["Complement"] as string,
                Street = rdr["Street"] as string,
                ContactMail = rdr["ContactMail"] as string
            }); ;


        }

        dbConnection.Close();
        return clients;

    }
    public List<Account> RecupererTousLesComptes()
    {
        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);

        DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();
        DbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT AccountId, Balance,AccountType,OpenDateAccount,ClientId FROM Accounts";

        DbDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            accounts.Add(new Account
            {
                AccountId = (int)rdr["AccountId"],
                Balance = (decimal)rdr["Balance"],
                AccountType = rdr["AccountType"] as string,
                OpenDateAccount = (DateTime)rdr["OpenDateAccount"],
                ClientID = (int)rdr["ClientId"]
            });
        }

        dbConnection.Close();
        return accounts;
    }
    public List<BankingConsoleApp.Transaction> RecupererToutesLesTransactions()
    {
        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);

        DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();
        DbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT TransactionId, AccountId,Amount,TransactionDate,TransactionType,Description FROM Transactions";

        DbDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            transactions.Add(new BankingConsoleApp.Transaction
            {
                TransactionId = (int)rdr["TransactionId"],
                AccountId = (int)rdr["AccountId"],
                Amount = (decimal)rdr["Amount"],
                TransactionDate = (DateTime)rdr["TransactionDate"],
                TransactionType = (string)rdr["TransactionType"],
                Description = (string)rdr["Description"]

            });
        }

        dbConnection.Close();
        return transactions;
    }

    public List<Client> RecupererTousContactsPrive()
    {


        _connectionString = "BDDConnect";
        DbProviderFactories.RegisterFactory(
        ConfigurationManager.ConnectionStrings[_connectionString].ProviderName, SqlClientFactory.Instance);
        DbProviderFactory dbpf = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[_connectionString].ProviderName);

        DbConnection dbConnection = dbpf.CreateConnection();
        dbConnection.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString;
        dbConnection.Open();
        DbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT Id, FirstName,BirthDate,Sexe,Name,City,PostalCode,Complement,Street, ContactMail FROM ClientsPrive";

        DbDataReader rdr = command.ExecuteReader();
        while (rdr.Read())
        {
            clients.Add(new PrivateClient
            {
                Id = (int)rdr["Id"],
                FirstName = rdr["FirstName"] as string,
                BirthDate = (DateTime)rdr["BirthDate"],
                Sexe = rdr["Sexe"] as string,
                Name = rdr["Name"].ToString(),
                City = rdr["City"] as string,
                PostalCode = rdr["postalCode"] as string,
                Complement = rdr["complement"] as string,
                Street = rdr["street"] as string,
                ContactMail = rdr["ContactMail"] as string
            });


        }
        dbConnection.Close();
        return clients;
    }

    public void DisplayAccounts()
    {
       
        Console.WriteLine("\n--- Liste des comptes ---");
        foreach (var account in clients)
        {
            if (account is PrivateClient privClient)
            {
                Console.WriteLine($"[Compte Privé] {privClient}");
            }
            else if (account is ProfessionnalClient proClient)
            {
                Console.WriteLine($"[Compte Professionnel] {proClient}");
            }
        }
    }

    // Méthode pour ajouter des transactions depuis un fichier JSON
    public void AddTransactionsFromJson(string jsonFilePath)
    {
        try
        {
            // Lire le fichier JSON
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("Le fichier spécifié n'existe pas.");
                return;
            }

            string jsonContent = File.ReadAllText(jsonFilePath);

            // Désérialiser les transactions
            var transactionsFromFile = JsonConvert.DeserializeObject<List<BankingConsoleApp.Transaction>>(jsonContent);

            if (transactionsFromFile == null )
            {
                Console.WriteLine("Aucune transaction n'a été trouvée dans le fichier JSON.");
                return;
            }

            // Ajouter les transactions à la liste et mettre à jour les comptes
            foreach (var transaction in transactionsFromFile)
            {
                transactions.Add(transaction);

                // Trouver le compte associé
                var account = accounts.FirstOrDefault(a => a.AccountId == transaction.AccountId);
                if (account != null)
                {
                    account.Balance += transaction.Amount; // Mise à jour du solde
                    UpdateAccountBalanceInDatabase(account.AccountId, account.Balance);
                }
                else
                {
                    Console.WriteLine($"Aucun compte trouvé pour l'ID {transaction.AccountId}. La transaction sera ignorée.");
                }
            }

            Console.WriteLine("Transactions ajoutées avec succès et soldes mis à jour.");
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier JSON : " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Une erreur est survenue : " + ex.Message);
        }
    }
    private void UpdateAccountBalanceInDatabase(int accountId, decimal newBalance)
    {
        string connectionString = "BDDConnect"; // Remplacez par votre chaîne de connexion SQL
        string updateQuery = "UPDATE Accounts SET Balance = @Balance WHERE AccountId = @AccountId";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Ajouter les paramètres SQL
                    command.Parameters.AddWithValue("@Balance", newBalance);
                    command.Parameters.AddWithValue("@AccountId", accountId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Solde mis à jour pour le compte ID {accountId}.");
                    }
                    else
                    {
                        Console.WriteLine($"La mise à jour du compte ID {accountId} a échoué.");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Erreur SQL : " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur inattendue : " + ex.Message);
        }
    }

    public void ExportToPdf(int clientId)
    {

        // Rechercher le client dans la liste
        Account client = _accounts.FirstOrDefault(c => c.AccountId == clientId);
        if (client == null)
        {
            Console.WriteLine($"Client avec l'ID {clientId} introuvable.");
            Console.ReadKey();
            return;
        }

        var transactions = _transactions.Where(t => t.AccountId == client.AccountId).ToList();
        var NomClient = accounts.Where(a => a.AccountId == client.AccountId).Join(clients, a => a.ClientID, c => c.Id, (a, c) => c.Name).FirstOrDefault();
        if (transactions.Count == 0)
        {
            Console.WriteLine($"Aucune transaction valide pour le client ID {NomClient}.");
            Console.ReadKey();
            return;
        }

        string fileName = $"Transactions_Client_{NomClient}.pdf";

        // Générer le PDF avec iText
        using (var writer = new PdfWriter(fileName))
        using (var pdfDoc = new PdfDocument(writer))
        using (var document = new Document(pdfDoc))
        {
            document.Add(new Paragraph($"Transactions valides pour {NomClient}").SetFontSize(18));

            // Ajouter une table pour afficher les transactions
            var table = new Table(4);
            table.AddHeaderCell("Date");
            table.AddHeaderCell("Type");
            table.AddHeaderCell("Description");
            table.AddHeaderCell("Montant");

            foreach (var transaction in transactions)
            {
                table.AddCell(transaction.TransactionDate.ToString("dd/MM/yyyy"));
                table.AddCell(transaction.TransactionType.ToString());
                table.AddCell(transaction.Description);
                table.AddCell(transaction.Amount.ToString("C"));
            }

            document.Add(table);
        }

        Console.WriteLine($"PDF généré avec succès : {fileName} -  {Environment.CurrentDirectory}");
        Console.ReadKey();
    }

    public void ExportToXml(int clientId)
    {
        var client = _accounts.FirstOrDefault(c => c.AccountId == clientId);
        // Récupérer les transactions invalides
        var anomalies = _transactions.Where(t => t.AccountId == client.AccountId).ToList();

        if (anomalies.Count == 0)
        {
            Console.WriteLine("Aucune transaction disponible.");
            Console.ReadKey();
            return;
        }

        string fileName = "Transactions.xml";

        // Sérialisation des anomalies en XML
        var xmlSerializer = new XmlSerializer(typeof(List<BankingConsoleApp.Transaction>));
        using (var stream = new FileStream(fileName, FileMode.Create))
        {
            xmlSerializer.Serialize(stream, anomalies);
        }

        Console.WriteLine($"Fichier XML généré avec succès : {fileName}");
        Console.ReadKey();
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        DbParameter param = command.CreateParameter();
        param.ParameterName = name;
        param.Value = value ?? DBNull.Value; // Gestion des valeurs nulles
        command.Parameters.Add(param);
    }
    public static string GenerateCardNumber()
    {
        string template = "497401850223XXXX";
        if (string.IsNullOrEmpty(template))
            throw new ArgumentException("Le modèle ne peut pas être vide.");

        // Remplace les 'X' par des chiffres aléatoires sauf le dernier (contrôle Luhn)
        StringBuilder cardNumber = new StringBuilder();
        Random random = new Random();

        foreach (char c in template)
        {
            if (c == 'X')
                cardNumber.Append(random.Next(0, 10));
            else if (char.IsDigit(c))
                cardNumber.Append(c);
            else
                throw new ArgumentException("Le modèle contient des caractères non valides.");
        }

        // Calcul de la clé Luhn pour le dernier chiffre
        string partialNumber = cardNumber.ToString();
        int checksum = CalculateLuhnChecksum(partialNumber);
        int controlDigit = (10 - (checksum % 10)) % 10;

        cardNumber[^1] = controlDigit.ToString()[0];

        return cardNumber.ToString();
    }

    // Calcul du checksum Luhn
    private static int CalculateLuhnChecksum(string number)
    {
        int sum = 0;
        bool doubleDigit = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';

            if (doubleDigit)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return sum;
    }

    // Méthode pour ajouter des comptes à titre de test
    public void AddAccount(Account account)
    {
        accounts.Add(account);
    }

    // Méthode pour effectuer un virement entre deux comptes
    public bool TransferFunds(int fromAccountId, int toAccountId, decimal amount)
    {
        var fromAccount = accounts.Find(a => a.AccountId == fromAccountId);
        var toAccount = accounts.Find(a => a.AccountId == toAccountId);

        if (fromAccount == null || toAccount == null)
        {
            Console.WriteLine("L'un des comptes n'existe pas.");
            return false;
        }

        if (fromAccount.Balance < amount)
        {
            Console.WriteLine("Fonds insuffisants pour effectuer le virement.");
            return false;
        }

        // Effectuer le virement
        fromAccount.Balance -= amount;
        toAccount.Balance += amount;

        Console.WriteLine($"Virement de {amount} EUR effectué avec succès !");
        Console.WriteLine($"Nouveau solde du compte source ({fromAccountId}): {fromAccount.Balance} EUR");
        Console.WriteLine($"Nouveau solde du compte cible ({toAccountId}): {toAccount.Balance} EUR");

        return true;
    }
}
