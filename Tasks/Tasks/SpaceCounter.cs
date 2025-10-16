using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class SpaceCounter : ISpaceCounter
    {
        public int CountSpaces(string text)
        {
            return text.Count(c => c == ' ');
        }
    }

}
