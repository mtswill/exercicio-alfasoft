using ExercicioAlfasoft;

var filePath = Console.ReadLine();

if (string.IsNullOrWhiteSpace(filePath))
{
    Console.WriteLine("The file path is mandatory!");
    return;
}

var fileRepository = new FileRepository(filePath);

var nameList = await fileRepository.ReadFileAsync();

await Task.Delay(5000);