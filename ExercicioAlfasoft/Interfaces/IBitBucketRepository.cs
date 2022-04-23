namespace ExercicioAlfasoft.Interfaces
{
    public interface IBitBucketRepository
    {
        public Task ExecuteRequestsAsync(List<string> usernames);
    }
}
