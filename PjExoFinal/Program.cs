using System;
using System.Collections.Generic;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace BankingConsoleApp
{
    class Program
    {
        static void Main()
        {
            // Appel de la fonction d'authentification avant d'afficher le menu
            if (AuthenticateUser())
            {
                var bankingManager = new BankingManager(); // Gestionnaire de comptes
                var client = new Client();
                var account = new Account();
                bankingManager.RecupererTousContactsPro();
                bankingManager.RecupererTousContactsPrive();
                bankingManager.RecupererTousLesComptes();
                bankingManager.RecupererToutesLesTransactions();
                // Ajouter quelques comptes pour le test
                bankingManager.AddAccount(new Account { AccountId = 1, AccountType = "Compte A", Balance = 1000 });
                bankingManager.AddAccount(new Account { AccountId = 2, AccountType = "Compte B", Balance = 500 });
                while (true) // Boucle principale pour l'interaction utilisateur
                {
                    Console.Clear();
                    Console.WriteLine("1. Créer un compte privé");
                    Console.WriteLine("2. Créer un compte professionnel");
                    Console.WriteLine("3. Afficher les comptes");
                    Console.WriteLine("4. Ajouter des transactions depuis un fichier JSON");
                    Console.WriteLine("5. Exporter les mouvements bancaires en PDF");
                    Console.WriteLine("6. Exporter les transactions en XML");
                    Console.WriteLine("7. Effectuer un virement entre comptes");
                    Console.WriteLine("8. Quitter");
                    Console.Write("Choisissez une option : ");
                    var choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            CreatePriveeClient(bankingManager); // Création d'un compte privé
                            break;

                        case "2":
                            CreateProClient(bankingManager); // Création d'un compte professionnel
                            break;

                        case "3":
                            bankingManager.DisplayAccounts(); // Afficher tous les comptes
                            break;

                        case "4":
                            AddTransactionsFromJson(bankingManager); // Ajouter des transactions depuis un fichier JSON
                            break;

                        case "5":
                            ExportToPdf(bankingManager); // Exporter les mouvements bancaires en PDF
                            break;

                        case "6":
                            ExportToXml(bankingManager); // Exporter les anomalies en XML
                            break;
                        case "7":
                            PerformTransfer(bankingManager);
                            break;
                        case "8":
                            Console.WriteLine("Merci d'avoir utilisé le programme. Au revoir !");
                            return; // Quitter le programme

                        default:
                            Console.WriteLine("Option invalide. Veuillez réessayer.");
                            break;
                    }
                }
            }

            // Méthode pour créer un compte privé
            static void CreatePriveeClient(BankingManager manager)
            {

                Console.Write("Nom du propriétaire : ");
                string Name = Console.ReadLine();

                Console.Write("Prénom : ");
                string firstName = Console.ReadLine();

                Console.Write("Mail de contact : ");
                string ContactMail = Console.ReadLine();
                Console.Write("Date de naissance (jj/mm/aaaa) : ");
                DateTime birthDate;
                while (!DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    Console.WriteLine("Format de date invalide. Réessayez (jj/mm/aaaa) : ");
                }

                Console.Write("Sexe (Masculin/Féminin) : ");
                string sexe = Console.ReadLine();
                do
                {

                    if (sexe != "Masculin" && sexe != "Féminin")
                    {
                        Console.WriteLine("Sexe invalide. Entrez Masculin ou Féminin : ");
                    }
                } while (sexe.ToString() != "Masculin" && sexe.ToString() != "Féminin");


                Console.WriteLine("Adresse du postal : ");
                Console.WriteLine("Libellé : ");
                string street = Console.ReadLine();
                Console.WriteLine("Complément : ");
                string complement = Console.ReadLine();
                Console.WriteLine("Code Postal : ");
                string postalCode = Console.ReadLine();
                Console.WriteLine("Ville : ");
                string city = Console.ReadLine();

                manager.CreatePriveeClient(Name, firstName,city, postalCode, complement, street, ContactMail, birthDate, sexe);
            }

            // Méthode pour créer un compte professionnel
            static void CreateProClient(BankingManager manager)
            {

                Console.Write("Nom du propriétaire : ");
                string Name = Console.ReadLine();
                Console.Write("Email de contact : ");
                string ContactMail = Console.ReadLine();

                Console.Write("SIRET (14 chiffres) : ");
                string siret;
                do
                {
                    siret = Console.ReadLine();
                    if (siret.Length != 14 || !long.TryParse(siret, out _))
                    {
                        Console.WriteLine("Le SIRET doit comporter exactement 14 chiffres. Réessayez : ");
                    }
                } while (siret.Length != 14 || !long.TryParse(siret, out _));

                Console.Write("Statut Juridique (SARL, SA, SAS, EURL) : ");
                string legalStatus;
                do
                {
                    legalStatus = Console.ReadLine();
                    if (legalStatus != "SARL" && legalStatus != "SA" && legalStatus != "SAS" && legalStatus != "EURL")
                    {
                        Console.WriteLine("Statut invalide. Entrez SARL, SA, SAS ou EURL : ");
                    }
                } while (legalStatus != "SARL" && legalStatus != "SA" && legalStatus != "SAS" && legalStatus != "EURL");

                Console.WriteLine("Adresse postale : ");
                Console.Write("Libellé : ");
                string street = Console.ReadLine();
                Console.Write("Complément : ");
                string complement = Console.ReadLine();
                Console.Write("Code Postal : ");
                string postalCode = Console.ReadLine();
                Console.Write("Ville : ");
                string city = Console.ReadLine();


                Console.WriteLine("Adresse du siège : ");
                Console.Write("Libellé : ");
                string street2 = Console.ReadLine();
                Console.Write("Complément : ");
                string complement2 = Console.ReadLine();
                Console.Write("Code Postal : ");
                string postalCode2 = Console.ReadLine();
                Console.Write("Ville : ");
                string city2 = Console.ReadLine();

                manager.CreateProClient(Name, city, postalCode, complement, street, ContactMail, siret, legalStatus,street2,complement2,postalCode2,city2);
            }
            // Méthode pour ajouter des transactions depuis un fichier JSON
            static void AddTransactionsFromJson(BankingManager manager)
            {
                Console.Write("Entrez le chemin du fichier JSON : ");
                string jsonFilePath = Console.ReadLine();

                manager.AddTransactionsFromJson(jsonFilePath);
            }

            // Méthode pour exporter les mouvements bancaires en PDF
            static void ExportToPdf(BankingManager manager)
            {
                Console.Write("Entrez l'ID du compte : ");
                int accountId = int.Parse(Console.ReadLine());

                manager.ExportToPdf(accountId);
            }

            // Méthode pour exporter les anomalies en XML
            static void ExportToXml(BankingManager manager)
            {
                Console.Write("Entrez l'ID du compte : ");
                int accountId = int.Parse(Console.ReadLine());

                manager.ExportToXml(accountId);
                Console.WriteLine("Données exportées avec succès en XML.");
            }
        }
        static void PerformTransfer(BankingManager manager)
        {
            Console.Write("Entrez l'ID du compte source : ");
            int fromAccountId = int.Parse(Console.ReadLine());

            Console.Write("Entrez l'ID du compte cible : ");
            int toAccountId = int.Parse(Console.ReadLine());

            Console.Write("Entrez le montant à transférer : ");
            decimal amount = decimal.Parse(Console.ReadLine());

            manager.TransferFunds(fromAccountId, toAccountId, amount);
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        // Fonction d'authentification
        static bool AuthenticateUser()
        {
                // Dictionnaire contenant les utilisateurs et leurs mots de passe (simplification)
                var users = new Dictionary<string, string>
                {  
                    { "admin", "admin123" }, // Exemple d'utilisateur
                    { "user", "userpass" }    // Un autre exemple d'utilisateur
                };

                Console.WriteLine("Veuillez vous authentifier");
                Console.Write("Login : ");
                string login = Console.ReadLine();

                Console.Write("Mot de passe : ");
                string password = ReadPassword();  // Utilisation de ReadPassword pour masquer la saisie

                // Vérification des identifiants
                if (users.ContainsKey(login) && users[login] == password)
                {
                    Console.WriteLine("Authentification réussie !");
                    return true;
                }
                else
                {
                    Console.WriteLine("Identifiants incorrects. Veuillez réessayer.");
                    return false;
                }

        }

        // Méthode pour lire un mot de passe sans afficher les caractères
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Efface le dernier caractère affiché
                }
                else if (!char.IsControl(key.KeyChar))  // Ignorer les touches de contrôle comme Enter
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Affiche un astérisque pour chaque caractère
                }
            }
            Console.WriteLine();  // Ajoute un retour à la ligne après la saisie du mot de passe
            return password;
        }

    }
}