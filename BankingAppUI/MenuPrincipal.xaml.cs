using BankingApp;
using BankingConsoleApp;
using BankingWpfApp;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankingAppUI
{
    /// <summary>
    /// Logique d'interaction pour MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        public MenuPrincipal(BankingManager manager)
        {

            InitializeComponent();
            //InitializeBankingData();

            _manager = manager;
        }
        private readonly BankingManager _manager;

        // Dictionnaire des utilisateurs pour l'authentification
        private readonly Dictionary<string, string> _users = new()
        {
            { "admin", "admin123" },
            { "user", "userpass" }
        };

        // Initialiser les données bancaires
        //private void InitializeBankingData()
        //{
        //    _manager.AddAccount(new Account { AccountId = 1, AccountType = "Compte A", Balance = 1000 });
        //    _manager.AddAccount(new Account { AccountId = 2, AccountType = "Compte B", Balance = 500 });
        //}

        // Action pour créer un compte privé
        private void CreatePriveeClient_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreatePriveeClientWindow(_manager);
            dialog.ShowDialog();
        }

        // Action pour créer un compte professionnel
        private void CreateProClient_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateProClientWindow(_manager);
            dialog.ShowDialog();
        }

        // Afficher tous les comptes
        private void DisplayAccounts_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Join("\n", _manager.RecupererTousLesComptes()));
        }

        // Afficher tous les clients (privés et professionnels)
        private void DisplayClients_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(string.Join("\n", _manager.RecupererTousContactsPro()));
            MessageBoxResult messageBoxResult2 = MessageBox.Show(string.Join("\n", _manager.RecupererTousContactsPrive()));
        }

        // Ajouter des transactions depuis un fichier JSON
        private void AddTransactionsFromJson_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Fichiers JSON (*.json)|*.json",
                Title = "Sélectionnez un fichier JSON"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _manager.AddTransactionsFromJson(openFileDialog.FileName);
                MessageBox.Show("Transactions ajoutées avec succès !");
            }
        }

        // Exporter en PDF
        private async void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = (Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow);

            var result = await metroWindow.ShowInputAsync(
                "Saisie de l'ID",
                "Entrez l'ID du compte :"
            );

            if (int.TryParse(result, out int accountId))
            {
                _manager.ExportToPdf(accountId);
                MessageBox.Show($"Export terminé avec succès pour le compte ID : {accountId}");
            }
            else
            {
                MessageBox.Show("L'ID du compte doit être un entier valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Exporter en XML
        private async void ExportToXml_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = (Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow);

            var result = await metroWindow.ShowInputAsync(
                "Saisie de l'ID",
                "Entrez l'ID du compte :"
            );

            if (int.TryParse(result, out int accountId))
            {
                _manager.ExportToXml(accountId);
                MessageBox.Show("Export XML réussi !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("L'ID du compte doit être un entier valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Effectuer un virement entre comptes
        private void PerformTransfer_Click(object sender, RoutedEventArgs e)
        {
            var transferDialog = new TransferFundsWindow(_manager);
            transferDialog.ShowDialog();
        }

        // Quitter l'application
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
