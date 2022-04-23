using ExercicioAlfasoft.Interfaces;

namespace ExercicioAlfasoft.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _filePath;
        private static readonly string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log_file.txt");
        private static readonly string _configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.txt");

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<string>> ReadFileAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("File path not exists.");
                    return null!;
                }

                var nameList = new List<string>();

                using var reader = new StreamReader(_filePath);

                string? line;
                while ((line = await reader.ReadLineAsync()) is not null)
                    nameList.Add(line);

                return nameList;
            }
            catch
            {
                Console.WriteLine("Could not read the file.");
                throw;
            }
        }

        public async Task WriteLogAsync(string logContent)
        {
            try
            {
                using var stream = new StreamWriter(_logFilePath);
                await stream.WriteLineAsync(logContent + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine("The log content could not be saved.");
                throw;
            }
        }

        public async Task SaveLastRequestDatetimeAsync()
        {
            try
            {
                using var stream = new StreamWriter(_configFilePath);
                await stream.WriteAsync(DateTime.UtcNow.ToString());
            }
            catch
            {
                Console.WriteLine("The last request datetime could not be saved.");
                throw;
            }
        }

        public async Task<bool> CanExecuteRequestAsync()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                    return true;

                using var reader = new StreamReader(_configFilePath);
                var content = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(content))
                    return true;

                if (DateTime.TryParse(content, out var datetime))
                {
                    if (DateTime.UtcNow < datetime.AddSeconds(60))
                        return false;
                }

                return true;
            }
            catch
            {
                Console.WriteLine("Could not read the config file.");
                throw;
            }
        }
    }
}
