using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class SpaceCount
    {
        private readonly IFileReader fileReader;
        private readonly ISpaceCounter spaceCounter;

        public SpaceCount(IFileReader fileReader, ISpaceCounter spaceCounter)
        {
            this.fileReader = fileReader;
            this.spaceCounter = spaceCounter;
        }

        public async Task<int> CountTotalSpaces(string folderPath, Func<string, Task<int>> countFuncAsync)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            var tasks = new List<Task<int>>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(async () =>
                {
                    return await countFuncAsync(file);
                }));
            }

            var counts = await Task.WhenAll(tasks);
            return counts.Sum();
        }



        public async Task<int> CountTotalSpacesInFilesParallel(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            var tasks = new List<Task<int>>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var content = await fileReader.ReadAllTextAsync(file);
                    return spaceCounter.CountSpaces(content);
                }));
            }

            int[] counts = await Task.WhenAll(tasks);
            return counts.Sum();
        }

        public async Task<int> CountTotalSpacesByLines(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            int total = 0;

            foreach (var file in files)
            {
                var lines = await fileReader.ReadLinesAsync(file);
                var lineTasks = new List<Task<int>>();

                foreach (var line in lines)
                {
                    lineTasks.Add(Task.Run(() => spaceCounter.CountSpaces(line)));
                }

                int[] counts = await Task.WhenAll(lineTasks);


                int fileTotal = 0;
                foreach (var count in counts)
                {
                    fileTotal += count;
                }

                total += fileTotal;
            }

            return total;
        }

    }
}
