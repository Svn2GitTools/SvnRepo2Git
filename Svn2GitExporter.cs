using GitImporter;
using GitImporter.Interfaces;
using GitImporter.Models;

using SvnExporter.Lib;
using SvnExporter.Lib.Models;

namespace SvnRepo2Git;

public class Svn2GitExporter : ISvnItemsExporter
{
    private readonly IAuthorsMap _authorsMap;

    private readonly string _gitRepoPath;

    public Svn2GitExporter(string gitRepoPath, IAuthorsMap authorsMap)
    {
        _gitRepoPath = gitRepoPath;
        _authorsMap = authorsMap;
    }

    public void Export(IEnumerable<SvnRevision> logEntries)
    {
        using (var compositionRoot =
               new CompositionRoot(_gitRepoPath, _authorsMap))
        {
            // Create the revision converter once, to be used for each revision.
            var revisionConverter = compositionRoot.GetRevisionConverter();

            foreach (var entry in logEntries)
            {
                Console.WriteLine(
                    "----------------------------------------------------------------");
                Console.WriteLine($"Revision: {entry.Revision}");
                Console.WriteLine($"Author: {entry.Author}");
                Console.WriteLine($"Date: {entry.Date}");
                Console.WriteLine($"Message: {entry.CommitMessage}");

                if (entry.Properties != null && entry.Properties.Count > 0)
                {
                    Console.WriteLine("Properties:");
                    foreach (var property in entry.Properties)
                    {
                        Console.WriteLine($"  {property.Key}: {property.Value}");
                    }
                }

                Console.WriteLine();

                GitRevision revision = SvnToGitModelConverter.ConvertRevision(entry);
                revisionConverter.ConvertRevision(revision);
            }

            compositionRoot.Checkout();
        }
    }
}
