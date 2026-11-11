using System;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models
{
    public class CustomAction
    {
        public string Name { get; set; }

        public string ExtensionFilter { get; set; }

        public string FilePath { get; set; }

        public string Arguments { get; set; }

        public bool RunPerFile { get; set; }

        public bool Matches(string fileName)
        {
            string filter = (ExtensionFilter ?? string.Empty).Trim();

            if (filter.Length == 0 || filter == "*" || filter == "*.*")
            {
                return true;
            }

            string extension = Path.GetExtension(fileName);

            if (extension.Length == 0)
            {
                return false;
            }

            foreach (string rawToken in filter.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string token = rawToken.Trim().TrimStart('*');

                if (token.Length == 0 || token == ".*")
                {
                    return true;
                }

                if (string.Equals(token, extension, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool MatchesAll(IEnumerable<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (!Matches(fileName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
