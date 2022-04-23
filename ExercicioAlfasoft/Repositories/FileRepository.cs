using ExercicioAlfasoft.Interfaces;

namespace ExercicioAlfasoft.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _filePath;
        private static readonly string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log_file.txt");
        private static readonly string _datetimeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "datetime.txt");

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
                throw new Exception("Could not read the file.");
            }
        }

        public async Task WriteLogAsync(string logContent)
        {
            try
            {
                using var stream = new StreamWriter(_logFilePath, true);
                await stream.WriteAsync($"{DateTime.UtcNow} - {logContent + Environment.NewLine}");
            }
            catch
            {
                throw new Exception("The log content could not be saved.");
            }
        }

        public async Task SaveLastRequestDatetimeAsync()
        {
            try
            {
                using var stream = new StreamWriter(_datetimeFilePath);
                await stream.WriteAsync(DateTime.UtcNow.ToString());
            }
            catch
            {
                throw new Exception("The last request datetime could not be saved.");
            }
        }

        public async Task<bool> CanExecuteRequestAsync()
        {
            try
            {
                if (!File.Exists(_datetimeFilePath))
                    return true;

                using var reader = new StreamReader(_datetimeFilePath);
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
                throw new Exception("Could not read the datetime file.");
            }
        }
    }
}
