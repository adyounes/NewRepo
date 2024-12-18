using System;
using System.Windows;

namespace BankingApp
{
    public partial class TransferFundsWindow : Window
    {
        private readonly BankingManager _bankingManager;

        public TransferFundsWindow(BankingManager bankingManager)
        {
            InitializeComponent();
            _bankingManager = bankingManager;
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            // Validation des saisies
            if (!int.TryParse(SourceAccountIdTextBox.Text, out int sourceAccountId))
            {
                MessageBox.Show("L'ID du compte source doit être un entier valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(TargetAccountIdTextBox.Text, out int targetAccountId))
            {
                MessageBox.Show("L'ID du compte cible doit être un entier valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Le montant doit être un nombre positif.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Appel de la méthode de transfert
                _bankingManager.TransferFunds(sourceAccountId, targetAccountId, amount);
                MessageBox.Show("Virement effectué avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du virement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
