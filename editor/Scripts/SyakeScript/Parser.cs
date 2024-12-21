using System.Collections.Generic;
using System.IO;

namespace Magro.Scripts.SyakeScript
{
    internal class Parser
    {
        public Parser()
        {
            var reader = new StreamReader("script.sk");
            var scanner = new Scanner(reader);
            // scanner.CurrentToken;
        }
    }
}
