namespace ExercicioAlfasoft.Interfaces
{
    public interface IFileRepository
    {
        Task<List<string>> ReadFileAsync();
        Task WriteLogAsync(string logContent);
        Task SaveLastRequestDatetimeAsync();
        Task<bool> CanExecuteRequestAsync();
    }
}
