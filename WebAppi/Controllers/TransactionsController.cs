using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Text;
using BankingConsoleApp;
using System.Net.Http;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly BankingDbContext _context;

        public TransactionsController(BankingDbContext context)
        {
            _context = context;
        }

        // GET: api/Transactions/export/json
        [HttpGet("export/json")]
        public IActionResult ExportTransactionsToJson()
        {
            try
            {
                // Récupérer toutes les transactions
                var transactions = _context.Transactions.ToList();

                if (!transactions.Any())
                {
                    return NotFound("Aucune transaction trouvée.");
                }

                // Convertir les transactions en JSON
                var json = System.Text.Json.JsonSerializer.Serialize(transactions, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true // Format JSON lisible
                });

                // Convertir en tableau d'octets pour téléchargement
                var fileBytes = System.Text.Encoding.UTF8.GetBytes(json);

                // Retourner le fichier JSON
                return File(fileBytes, "application/json", "Transactions.json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur est survenue : {ex.Message}");
            }
        }


        // POST: api/Transactions/import/txt
        [HttpPost("import/txt")]
        public IActionResult ImportTransactionsFromTxt([FromBody] ImportRequest request)
        {
            if (string.IsNullOrEmpty(request.FilePath))
            {
                return BadRequest("FilePath cannot be null or empty.");
            }

            try
            {
                // Lecture des transactions depuis le fichier
                var lines = System.IO.File.ReadAllLines(request.FilePath);
                var transactions = new List<Transaction>();

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length == 5)
                    {
                        transactions.Add(new Transaction
                        {
                            CardNumber = data[0],
                            Amount = decimal.Parse(data[1]),
                            TransactionType = data[2],
                            TransactionDate = DateTime.Parse(data[3]),
                            Currency = data[4]
                        });
                    }
                }

                // Ajout des transactions dans la base de données
                _context.Transactions.AddRange(transactions);
                _context.SaveChanges();

                return Ok($"Transactions imported successfully from file: {request.FilePath}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // GET: api/Transactions/export/pdf
        [HttpGet("export/pdf")]
        public IActionResult ExportTransactionsToPdf()
        {
            var transactions = _context.Transactions.ToList();
            if (!transactions.Any()) return NotFound("No transactions available.");

            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdfDocument = new PdfDocument(writer);
            using var document = new Document(pdfDocument);

            document.Add(new Paragraph("Liste des Transactions").SetFontSize(18));
            foreach (var t in transactions)
            {
                document.Add(new Paragraph($"Carte: {t.CardNumber}, Montant: {t.Amount}, Type: {t.TransactionType}, Date: {t.TransactionDate:dd/MM/yyyy}, Devise: {t.Currency}"));
            }

            document.Close();
            return File(memoryStream.ToArray(), "application/pdf", "Transactions.pdf");
        }
    }
}