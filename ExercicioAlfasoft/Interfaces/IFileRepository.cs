namespace ExercicioAlfasoft.Interfaces
{
    public interface IFileRepository
    {
        public Task<List<string>> ReadFileAsync();
        public Task WriteLogAsync(string logContent);
    }
}
