using ExercicioAlfasoft;
using ExercicioAlfasoft.Interfaces;

Console.Write("Enter the path to the names file: ");
var filePath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("The file path is mandatory!");
    return;
}

IFileRepository fileRepository = new FileRepository(filePath);
IBitBucketRepository bitBucketRepository = new BitBucketRepository(fileRepository);

var nameList = await fileRepository.ReadFileAsync();
await bitBucketRepository.ExecuteRequestsAsync(nameList);