using System;
using System.Windows;
using System.Windows.Controls;

namespace BankingWpfApp
{
    public partial class CreatePriveeClientWindow : Window
    {
        private readonly BankingManager _bankingManagerPriv;

        public CreatePriveeClientWindow(BankingManager bankingManager2)
        {
            InitializeComponent();
            _bankingManagerPriv = bankingManager2;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string contactMail = MailTextBox.Text;
            DateTime birthDate = BirthDatePicker.SelectedDate ?? DateTime.MinValue;
            string gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string street = StreetTextBox.Text;
            string complement = ComplementTextBox.Text;
            string postalCode = PostalCodeTextBox.Text;
            string city = CityTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(contactMail) ||
                birthDate == DateTime.MinValue || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(postalCode) || string.IsNullOrWhiteSpace(city))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            _bankingManagerPriv.CreatePriveeClient(name, firstName, city, postalCode, complement, street, contactMail, birthDate, gender);
            MessageBox.Show("Compte privé créé avec succès !");
            Close();
        }
    }
}
