using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingConsoleApp
{
    public class Client
    {
        public Client( string name, string city, string postalCode, string complement, string street, string contactMail)
        {
            Name = name;
            City = city;
            PostalCode = postalCode;
            Complement = complement;
            Street = street;
            ContactMail = contactMail;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
        public string Street { get; set; }

        public string ContactMail { get; set; }


        public override string ToString()
        {
            return $"Nom: {Name}, Adresse: {Street} - {Complement} - {PostalCode} - {City} , Mail :{ContactMail}";
        }
        public Client()
        {
            // Constructeur par défaut
        }
    }
}
