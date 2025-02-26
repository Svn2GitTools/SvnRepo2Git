using SvnRepo2Git.Interfaces;

namespace SvnRepo2Git
{
    public class CommandLineParserService : ICommandLineParser
    {
        public CommandLineOptions Parse(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-v")
                {
                    options.Verbose = true;
                }
                else if (args[i] == "--git")
                {
                    options.ConvertToGit = true;
                    if (i + 1 < args.Length)
                    {
                        options.GitRepoPath = args[i + 1];
                        i++; // consume git repo path
                    }
                    else
                    {
                        Console.WriteLine("Error: --git requires a git repository path");
                        options.ShowHelp = true;
                        return options;
                    }
                }
                else if (args[i] == "--genauthors")
                {
                    options.AuthorsGeneration = true;
                    if (i + 1 < args.Length)
                    {
                        options.EMailDomain = args[i + 1];
                        i++; // consume e-mail domain
                    }
                    else
                    {
                        Console.WriteLine("Error: --genauthors requires a e-mail domain");
                        options.ShowHelp = true;
                        return options;
                    }
                }
                else if (options.SvnRepoUrl == null) // Only set the file path once
                {
                    options.SvnRepoUrl = args[i];
                }
                else
                {
                    ShowUsage();
                    options.ShowHelp = true;
                    return options;
                }
            }

            if (options.SvnRepoUrl == null)
            {
                ShowUsage();
                options.ShowHelp = true;
                return options;
            }

            return options;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage: SvnRepo2Git <svn_repo_url> [--git <git_repo_path>] [--getauthors <e-mail domain>] [-v]");
            Console.WriteLine("  --git           Convert to git repository");
            Console.WriteLine("  --genauthors       Generate authors file");   
            Console.WriteLine("  -v              Verbose output. Include partial file contents");
        }
    }
}
