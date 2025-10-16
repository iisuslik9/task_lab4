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
      

        public async Task<int> CountTotalSpacesInFilesParallel(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            var tasks = files.Select(async file =>
            {
                var content = await fileReader.ReadAllTextAsync(file);
                return spaceCounter.CountSpaces(content);
            });
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
                var lineTasks = lines.Select(line => Task.Run(() => spaceCounter.CountSpaces(line)));
                var counts = await Task.WhenAll(lineTasks);
                total += counts.Sum();
            }

            return total;
        }

    }
}
