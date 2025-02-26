namespace SvnRepo2Git
{
    public class CommandLineOptions
    {
        public bool AuthorsGeneration { get; set; }

        public bool ConvertToGit { get; set; }

        public string EMailDomain { get; set; }

        public string GitRepoPath { get; set; }

        public string AuthorsPath { get; set; } = "authors.txt";

        public bool ShowHelp { get; set; }

        public string SvnRepoUrl { get; set; }

        //public bool FullInMemory { get; set; }

        public bool Verbose { get; set; }
    }
}
