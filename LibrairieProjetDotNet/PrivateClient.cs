using BankingConsoleApp;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibrairieProjetDotNet
{
    public class PrivateClient : Client
    {
      
        public string FirstName { get; set; } // Prénom
        public DateTime BirthDate { get; set; } // Date de naissance
        public string Sexe { get; set; } // Sexe (Masculin/Féminin)

        public int Id { get; internal set; }

        public PrivateClient(string name, string city, string postalCode, string complement, string street, string contactMail, string firstName, DateTime birthDate, string sexe)
          : base(name, city, postalCode, complement, street, contactMail)
        {
            FirstName = firstName;
            BirthDate = birthDate;
            Sexe = sexe;
        }
        // Constructeur par défaut
        public PrivateClient() { }
        public override string ToString()
        {
            return $"{base.ToString()}, Prénom: {FirstName}, Date de Naissance: {BirthDate.ToShortDateString()}, Sexe: {Sexe.ToString()}";
        }
    }
}
