using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BankingApp;
using BankingAppUI;
using BankingConsoleApp;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace BankingWpfApp
{
    public partial class MainWindow : Window
    {
        private readonly BankingManager _bankingManager = new BankingManager();
        private readonly Dictionary<string, string> _users = new()
        {
            { "admin", "admin123" },
            { "user", "userpass" }
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeBankingData();
        }

        private void InitializeBankingData()
        {
            _bankingManager.AddAccount(new Account { AccountId = 1, AccountType = "Compte A", Balance = 1000 });
            _bankingManager.AddAccount(new Account { AccountId = 2, AccountType = "Compte B", Balance = 500 });
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Supposons que les identifiants sont "admin" et "password"
            string correctLogin = "admin";
            string correctPassword = "admin123";

            // Récupérer les valeurs saisies par l'utilisateur
            string enteredLogin = LoginTextBox.Text;
            string enteredPassword = PasswordBox.Password;

            // Vérifier si les informations sont correctes
            if (enteredLogin == correctLogin && enteredPassword == correctPassword)
            {
                // Si l'authentification est réussie
                MessageBox.Show("Authentification réussie");

                // Créer et afficher la nouvelle fenêtre du menu principal
                MenuPrincipal menuPrincipal = new MenuPrincipal(_bankingManager);
                menuPrincipal.Show();

                // Fermer la fenêtre de connexion
                this.Close();
            }
            else
            {
                // Si l'authentification échoue
                MessageBox.Show("Identifiants incorrects, veuillez réessayer.");
            }
        }

      
    }
}
