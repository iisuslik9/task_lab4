using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class FileReader : IFileReader
    {
        public Task<string> ReadAllTextAsync(string path)
        {
            return File.ReadAllTextAsync(path);
        }

        public Task<IEnumerable<string>> ReadLinesAsync(string path)
        {
            var lines = File.ReadLines(path).AsEnumerable();
            return Task.FromResult(lines);
        }
    }
}
