using Microsoft.Extensions.DependencyInjection;


namespace Tasks
{
    public interface IFileReader
    {
        Task<string> ReadAllTextAsync(string path);
        Task<IEnumerable<string>> ReadLinesAsync(string path);
    }

    public interface ISpaceCounter
    {
        int CountSpaces(string text);
    }

    
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<IFileReader, FileReader>();
            services.AddTransient<ISpaceCounter, SpaceCounter>();
            services.AddTransient<SpaceCount>();

            var serviceProvider = services.BuildServiceProvider();

            // экземпляр SpaceCountчерез DI
            var manager = serviceProvider.GetRequiredService<SpaceCount>();


            string folderPath = null;
            Console.WriteLine("input path:");
            while (folderPath == null)
            {
                folderPath = Console.ReadLine();
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("no directory, try again");
                    folderPath = null;
                }
            }

            //IFileReader fileReader = new FileReader();
            //ISpaceCounter spaceCounter = new SpaceCounter();
            //var manager = new SpaceCount(fileReader, spaceCounter);
         

            var (timeWholeFiles, resultsWholeFiles) = await RunTimeMeasure.MeasureAsync(
                   () => manager.CountTotalSpacesInFilesParallel(folderPath));
            int totalSpacesWholeFiles = await manager.CountTotalSpacesInFilesParallel(folderPath);

            Console.WriteLine($"пробелов (чтение целых файлов): {totalSpacesWholeFiles}");
            Console.WriteLine($"Время выполнения: {timeWholeFiles.TotalMilliseconds} мс\n");

            var (timeByLines, resultsByLines) = await RunTimeMeasure.MeasureAsync(
                () => manager.CountTotalSpacesByLines(folderPath));
            int totalSpacesByLines = await manager.CountTotalSpacesByLines(folderPath);

            Console.WriteLine($"пробелов (чтение по строкам): {totalSpacesByLines}");
            Console.WriteLine($"Время выполнения: {timeByLines.TotalMilliseconds} мс");

        }


    }

}
