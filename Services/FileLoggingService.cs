using ExtractInfoIdentityDocument.Services.Interface;

using System.Text;

namespace ExtractInfoIdentityDocument.Services
{
    public class FileLoggingService : IFileLoggingService
    {
        private readonly IUserContextService _userContextService;
        private readonly string _logDirectory = "logs";

        public FileLoggingService(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        public async Task LogActionAsync(string action, string entityName, string details)
        {
            try
            {
                // 1. Obținem utilizatorul curent (pentru numele fișierului)
                string currentUserEmail = _userContextService.GetCurrentUserEmail();

                // Dacă acțiunea e făcută de un anonim (ex: login eșuat sau sistem), punem "System" sau "Anonymous"
                if (string.IsNullOrEmpty(currentUserEmail))
                {
                    currentUserEmail = "Anonymous";
                }

                // 2. Construim numele fișierului: zi_luna_an_currentuser.txt
                string datePart = DateTime.Now.ToString("dd_MM_yyyy");

                // Curățăm emailul de caractere invalide pentru fișiere, dacă e cazul, deși emailurile sunt de obicei ok
                string safeUserEmail = currentUserEmail.Replace(Path.GetInvalidFileNameChars().ToString(), "_");

                string fileName = $"{datePart}_{safeUserEmail}.txt";

                // 3. Verificăm și creăm folderul "logs" dacă nu există
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _logDirectory);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // 4. Calea completă a fișierului
                string filePath = Path.Combine(directoryPath, fileName);

                // 5. Formatăm linia de log
                string logLine = $"[{DateTime.Now:HH:mm:ss}] ACTIUNE: {action} | ENTITATE: {entityName} | DETALII: {details}{Environment.NewLine}";

                // 6. Scriem în fișier (Append)
                await File.AppendAllTextAsync(filePath, logLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // În caz de eroare la scrierea log-ului, putem scrie în consolă pentru a nu bloca aplicația
                Console.WriteLine($"Eroare la scrierea log-ului: {ex.Message}");
            }
        }
    }
}
