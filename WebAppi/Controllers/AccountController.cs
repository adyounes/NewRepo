using Microsoft.AspNetCore.Mvc;
using BankingConsoleApp;
using WebAPI.Models;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using System.IO;

using WebAPI.Data;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly BankingDbContext _context;

        // Constructor for injecting DbContext
        public AccountsController(BankingDbContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public IActionResult GetAccounts()
        {
            var accounts = _context.Accounts.ToList();
            return Ok(accounts);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        // POST: api/Accounts
        [HttpPost]
        public IActionResult AddAccount([FromBody] WebAPI.Models.Account account)
        {
            // Validate the input account
            if (account == null)
            {
                return BadRequest("Le compte fourni est nul.");
            }

            // Convert API model Account to DB model Account
            var bankingAccount = new BankingConsoleApp.Account
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                AccountType = account.AccountType,
                
            };

            // Add the account to the DbContext
            _context.Accounts.Add(bankingAccount);
            _context.SaveChanges();

            // Return a CreatedAtAction response
            return CreatedAtAction(nameof(GetAccount), new { id = bankingAccount.AccountId }, bankingAccount);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return NoContent();
        }
        // GET: api/Accounts/export/pdf
        [HttpGet("export/pdf")]
        public IActionResult ExportToPdf()
        {
            // Récupérer tous les comptes
            var accounts = _context.Accounts.ToList();

            // Création d'un flux mémoire pour le PDF
            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdfDocument = new PdfDocument(writer);
            using var document = new Document(pdfDocument);

            // Titre du PDF
            document.Add(new Paragraph("Liste des Comptes").SetFontSize(18));

            // Ajouter les comptes au PDF
            foreach (var account in accounts)
            {
                document.Add(new Paragraph(
                    $"ID: {account.AccountId}, " +
                    $"Solde: {account.Balance}, Type: {account.AccountType}"));
            }

            document.Close();

            // Retourner le fichier PDF au client
            return File(memoryStream.ToArray(), "application/pdf", "Accounts.pdf");
        }

    }
}
