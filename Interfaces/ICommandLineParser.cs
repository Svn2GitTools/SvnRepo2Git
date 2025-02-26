namespace SvnRepo2Git.Interfaces;

public interface ICommandLineParser
{
    CommandLineOptions Parse(string[] args);
}