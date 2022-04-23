using ExercicioAlfasoft.Interfaces;

namespace ExercicioAlfasoft.Repositories
{
    public class BitBucketRepository : IBitBucketRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IFileRepository _fileRepository;

        public BitBucketRepository(IFileRepository fileRepository)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.bitbucket.org/2.0/users/")
            };

            _fileRepository = fileRepository;
        }

        public async Task ExecuteRequestsAsync(List<string> usernames)
        {

            try
            {
                if (usernames is null)
                    throw new Exception("The usernames list is null.");

                if (!await _fileRepository.CanExecuteRequestAsync())
                    throw new Exception("The request cannot be executed in an interval shorter than 60 seconds.");

                foreach (var username in usernames)
                {
                    Console.WriteLine($"\nRunning request for username '{username}'");
                    Console.WriteLine($"Request URL: '{_httpClient.BaseAddress + username}'");

                    var response = await _httpClient.GetAsync(username);
                    var content = await response.Content.ReadAsStringAsync();

                    await _fileRepository.SaveLastRequestDatetimeAsync();

                    Console.Write("Request response: ");
                    Console.WriteLine(content);

                    await _fileRepository.WriteLogAsync(content);
                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
        }
    }
}
