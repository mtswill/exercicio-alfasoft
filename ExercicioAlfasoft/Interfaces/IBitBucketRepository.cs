namespace ExercicioAlfasoft.Interfaces
{
    public interface IBitBucketRepository
    {
        Task ExecuteRequestsAsync(List<string> usernames);
    }
}
