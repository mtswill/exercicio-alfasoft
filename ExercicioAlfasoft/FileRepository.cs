namespace ExercicioAlfasoft
{
    public class FileRepository
    {
        private readonly string _filePath;
        private static readonly string _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log_file.txt");

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<string>> ReadFileAsync()
        {
            try
            {
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
                await File.AppendAllTextAsync(_logFilePath, logContent + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine("The log content could not be saved.");
                throw;
            }
        }
    }
}
