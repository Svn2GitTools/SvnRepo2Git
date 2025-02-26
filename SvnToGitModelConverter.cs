using GitImporter;
using GitImporter.Models;

using SharpSvn;

using SvnExporter.Lib.Models;


namespace SvnRepo2Git;

public static class SvnToGitModelConverter
{
    private static readonly HashSet<string> SpecialDirectories =
        new HashSet<string> { "branches", "tags", "trunk" };

    public static GitNodeChange ConvertChangeInfo(SvnChangeInfo svnChangeInfo)
    {
        var gitNodeChange = new GitNodeChange
                                {
                                    Action = MapChangeAction(svnChangeInfo.Action),
                                    Kind = MapNodeKind(svnChangeInfo.NodeKind),
                                    Path = ModifyPath(svnChangeInfo.Path, svnChangeInfo.NodeKind),
                                    CopyFromPath = ModifyPath(
                                        svnChangeInfo.CopyFromPath,
                                        svnChangeInfo.NodeKind),
                                    CopyFromRevision =
                                        (int?)svnChangeInfo
                                            .CopyFromRevision // Cast to int? if Git NodeChange.CopyFromRevision is int?
                                };

        // Handle FileInfo if available (for file changes)
        if (svnChangeInfo.FileInfo != null)
        {
            if (svnChangeInfo.FileInfo.IsBinary)
            {
                gitNodeChange.BinaryContent = svnChangeInfo.FileInfo.BinaryContent;
            }
            else
            {
                gitNodeChange.TextContent = svnChangeInfo.FileInfo.Content;
            }
        }

        // NodeChange Properties (if needed - your NodeChange doesn't seem to use properties from SvnChangeInfo directly)
        // If SvnChangeInfo had properties to copy, you would do it here similar to Revision properties

        return gitNodeChange;
    }

    public static GitRevision ConvertRevision(SvnExporter.Lib.Models.SvnRevision svnRevision)
    {
        var gitRevision = new GitRevision
                              {
                                  Author = svnRevision.Author,
                                  Date = svnRevision.Date,
                                  LogMessage = svnRevision.CommitMessage,
                                  Number = svnRevision.Revision,
                              };

        // Copy Revision Properties
        foreach (var prop in svnRevision.Properties)
        {
            gitRevision.Properties.Add(prop.Key, prop.Value);
        }

        // Convert and Add ChangeInfo to NodeChanges
        foreach (var svnChangeInfo in svnRevision.ChangeInfo)
        {
            gitRevision.AddNode(ConvertChangeInfo(svnChangeInfo));
        }

        return gitRevision;
    }

    // Helper methods to map enums
    private static EChangeAction MapChangeAction(SvnChangeAction svnAction)
    {
        switch (svnAction)
        {
            case SvnChangeAction.Add:
                return EChangeAction.Add;
            case SvnChangeAction.Modify:
                return EChangeAction.Modify;
            case SvnChangeAction.Delete:
                return EChangeAction.Delete;
            case SvnChangeAction.Replace:
                return
                    EChangeAction
                        .Replace; // Or handle Replace specifically if EChangeAction has a Replace
            // Add other mappings as needed based on your EChangeAction enum
            default:
                return
                    EChangeAction.None; // Or throw an exception if unmapped actions are unexpected
        }
    }

    private static ENodeKind MapNodeKind(SvnNodeKind svnKind)
    {
        switch (svnKind)
        {
            case SvnNodeKind.File:
                return ENodeKind.File;
            case SvnNodeKind.Directory:
                return ENodeKind.Directory;
            default:
                return ENodeKind.Unknown; // Or throw an exception if unmapped kinds are unexpected
        }
    }

    private static string? ModifyPath(string? path, SvnNodeKind nodeKind)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        // Remove leading "/"
        path = path.TrimStart('/');

        // Add trailing "/" for specific directories
        if (nodeKind == SvnNodeKind.Directory && !path.EndsWith("/"))
        {
            int lastSlashIndex = path.LastIndexOf('/');
            string lastSegment = lastSlashIndex >= 0 ? path.Substring(lastSlashIndex + 1) : path;

            if (SpecialDirectories.Contains(lastSegment))
            {
                path += "/";
            }
        }

        return path;
    }
}
