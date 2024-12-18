using System;
using System.Windows;
using System.Windows.Controls;

namespace BankingWpfApp
{
    public partial class CreateProClientWindow : Window
    {
        private readonly BankingManager _manager;

        public CreateProClientWindow(BankingManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupération des données depuis les champs de texte
                string name = NameTextBox.Text;
                string contactEmail = ContactEmailTextBox.Text;
                string siret = SiretTextBox.Text;
                string legalStatus = ((ComboBoxItem)LegalStatusComboBox.SelectedItem)?.Content.ToString() ?? throw new Exception("Le statut juridique est obligatoire.");
                string street = StreetTextBox.Text;
                string complement = ComplementTextBox.Text;
                string postalCode = PostalCodeTextBox.Text;
                string city = CityTextBox.Text;
                string headOfficeStreet = HeadOfficeStreetTextBox.Text;
                string headOfficeComplement = HeadOfficeComplementTextBox.Text;
                string headOfficePostalCode = HeadOfficePostalCodeTextBox.Text;
                string headOfficeCity = HeadOfficeCityTextBox.Text;

                // Appel à la méthode correspondante de BankingManager
                _manager.CreateProClient(name, city, postalCode, complement, street, contactEmail, siret, legalStatus, headOfficeStreet, headOfficeComplement, headOfficePostalCode, headOfficeCity);

                MessageBox.Show("Compte professionnel créé avec succès !");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
