using GitImporter.Interfaces;

namespace SvnRepo2Git;

public class AuthorsListImporter: IAuthorsMap
{
    private const string DefaultDomain = "example.com"; // Default domain if not found in file

    private readonly Dictionary<string, string> _authors = new Dictionary<string, string>();

    public AuthorsListImporter(string fileName = null) // Constructor takes optional filename
    {
        if (!string.IsNullOrEmpty(fileName)) // Load if filename provided
        {
            LoadFromFile(fileName);
        }
    }

    public string GetAuthorEmail(string authorName)
    {
        if (_authors.ContainsKey(authorName))
        {
            return _authors[authorName];
        }

        return $"{authorName}@{DefaultDomain}"; // Default email if not found
    }

    public void PrintAuthors()
    {
        if (_authors.Count == 0)
        {
            Console.WriteLine("No authors loaded.");
            return;
        }

        Console.WriteLine("Loaded Authors:");
        foreach (var kvp in _authors)
        {
            Console.WriteLine($"  {kvp.Key} - {kvp.Value}");
        }
    }

    private bool LoadFromFile(string fileName)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine(
                    $"File not found: {fileName}. Using default email domain: {DefaultDomain}");
                return false; // Use default domain for all if file not found
            }

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length == 2)
                    {
                        string authorName = parts[0];
                        string authorEmail = parts[1];
                        _authors[authorName] = authorEmail;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return false;
    }
}