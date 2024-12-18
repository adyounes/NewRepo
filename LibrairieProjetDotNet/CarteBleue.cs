using BankingConsoleApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieProjetDotNet
{
    public class CarteBleue
    {
        public int CarteBleueId {  get; set; }
        public string CardNumber { get; set; }

        public Account AccountId23 { get; set; }

        public CarteBleue(string cardNumber)
        {
            CardNumber = cardNumber;
        }
    }
}
