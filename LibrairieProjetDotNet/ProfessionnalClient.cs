using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;

namespace BankingConsoleApp
{
    public class ProfessionnalClient : Client
    {
        public string Siret { get; set; } // 14 chiffres
        public string LegalStatus { get; set; } // SARL, SA, SAS, EURL
        public string ProCity { get; set; }
        public string ProPostalCode { get; set; }
        public string ProComplement { get; set; }
        public string ProStreet { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, SIRET: {Siret}, Statut Juridique: {LegalStatus}, Adresse: {ProCity} - {ProPostalCode} - {ProComplement} - {ProStreet}";
        }
        // Constructeur par défaut
        public ProfessionnalClient() { }
        public ProfessionnalClient(string name, string city, string postalCode, string complement, string street, string contactMail, string siret, string legalStatus, string proCity, string proPostalCode, string proComplement, string proStreet)
             : base( name, city, postalCode, complement, street, contactMail)
        {
            Siret = siret;
            LegalStatus = legalStatus;
            ProCity = proCity;
            ProPostalCode = proPostalCode;
            ProComplement = proComplement;
            ProStreet = proStreet;
        }

    }
}
