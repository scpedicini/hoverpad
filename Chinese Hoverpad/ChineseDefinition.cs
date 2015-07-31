using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chinese_Hoverpad
{
    public class ChineseDefinition
    {
        // don't put original words in since this will be linked across multiple hash sets for simplified and traditional
        public string EnglishDefinition;

        public ChineseDefinition(string def)
        {
            EnglishDefinition = def;
        }
    }
}
