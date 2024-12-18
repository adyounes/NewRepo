using BankingConsoleApp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieProjetDotNet
{
    public class Anomalies
    {
        [Key]
        public int AnomalieId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime AnomalieDate { get; set; }
        public string AnomalieType { get; set; }
        public string Description { get; set; }
        public Account Account { get; set; }
    }
}
