using SvnExporter.Lib;
using SvnExporter.Lib.Models;

using SvnRepo2Git.Interfaces;

namespace SvnRepo2Git
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ICommandLineParser commandLineParser = new CommandLineParserService();

            ISvnItemsReader itemsReader = new SvnItemsReader();

            try
            {
                var options = commandLineParser.Parse(args);

                if (options.ShowHelp)
                {
                    return;
                }

                if (options.AuthorsGeneration)
                {
                    //  Author List Options (No File Content, Minimal Data)
                    LogRetrievalOptions authorListOptions = new LogRetrievalOptions
                                                                {
                                                                    FileContentMode =
                                                                        EFileContentMode.None,
                                                                    IncludeChangedPaths =
                                                                        false, // We might not need changed paths for just authors
                                                                    IncludeRevisionProperties =
                                                                        false // Probably don't need revision properties for author list
                                                                };
                    Console.WriteLine("\n--- Author List Output (Minimal Data) ---");
                    AuthorsListExporter
                        authorsListExporter =
                            new AuthorsListExporter(); // Example AuthorListExporter
                    IEnumerable<SvnRevision> logEntriesForAuthors =
                        itemsReader.GetLogEntries(options.SvnRepoUrl, authorListOptions);
                    authorsListExporter.Export(logEntriesForAuthors);
                    authorsListExporter.WriteToFile("authors.txt", options.EMailDomain);
                }

                if (options.ConvertToGit)
                {
                    AuthorsListImporter authorsListImporter =
                        new AuthorsListImporter(options.AuthorsPath);

                    ISvnItemsExporter svnItemsExporter = new Svn2GitExporter(
                        options.GitRepoPath,
                        authorsListImporter);

                    LogRetrievalOptions gitExportOptions = new LogRetrievalOptions
                                                               {
                                                                   FileContentMode =
                                                                       EFileContentMode.Full,
                                                                   IncludeChangedPaths = true,
                                                                   IncludeRevisionProperties = true
                                                               };
                    Console.WriteLine(
                        "\n--- Git Export Output (Full Content) ---"); // Example - replace with GitExporter
                    IEnumerable<SvnRevision> logEntriesForGit =
                        itemsReader.GetLogEntries(options.SvnRepoUrl, gitExportOptions);
                    svnItemsExporter.Export(logEntriesForGit);
                }
                else
                {
                    ISvnItemsExporter svnItemsExporter = new ConsoleSvnItemsExporter();
                    // 1. Console Display Options (Preview Content)
                    LogRetrievalOptions consoleOptions = new LogRetrievalOptions
                                                             {
                                                                 FileContentMode =
                                                                     options.Verbose
                                                                         ? EFileContentMode.Preview
                                                                         : EFileContentMode.None,
                                                                 FileContentPreviewLength = 100,
                                                                 IncludeChangedPaths = true,
                                                                 IncludeRevisionProperties = true
                                                             };
                    Console.WriteLine("--- Console Display Output (Preview Content) ---");
                    IEnumerable<SvnRevision> logEntriesForConsole =
                        itemsReader.GetLogEntries(options.SvnRepoUrl, consoleOptions);
                    svnItemsExporter.Export(logEntriesForConsole);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
