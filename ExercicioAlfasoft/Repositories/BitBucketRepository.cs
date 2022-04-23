using ExercicioAlfasoft.Interfaces;
using System.Web;

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
            if (usernames is null)
                return;

            if (!await _fileRepository.CanExecuteRequestAsync())
            {
                Console.WriteLine("The request cannot be executed in an interval shorter than 60 seconds.");
                return;
            }

            foreach (var username in usernames)
            {
                try
                {
                    Console.WriteLine($"\nRunning request for username '{username}'");
                    Console.WriteLine($"Request URL: '{_httpClient.BaseAddress + username}'");

                    var response = await _httpClient.GetAsync(username);
                    var content = await response.Content.ReadAsStringAsync();

                    await _fileRepository.SaveLastRequestDatetimeAsync();

                    Console.Write("Request response: ");
                    Console.WriteLine(content);
                    Console.WriteLine("\n");

                    await _fileRepository.WriteLogAsync(content);
                }
                finally
                {
                    await Task.Delay(5000);
                }
            }
        }
    }
}
