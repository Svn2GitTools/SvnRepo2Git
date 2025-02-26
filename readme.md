# SvnRepo2Git: Migrate Your Live SVN Repository to Git

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![GitHub release](https://img.shields.io/github/v/release/Svn2GitTools/SvnRepo2Git)](https://github.com/Svn2GitTools/SvnRepo2Git/releases/latest)

**SvnRepo2Git** is a command-line tool written in C# designed to migrate a live Subversion (SVN) repository to a Git repository. Unlike tools that rely on SVN dump files, `SvnRepo2Git` directly interacts with an SVN repository URL to extract revision history and convert it into a Git repository. It leverages libraries like [SvnExporter](https://github.com/your-svn-exporter-repo-link) (replace with the actual link) to read SVN data and [GitImporter](https://github.com/your-git-importer-repo-link) (or your Git interaction library) to create the Git repository.

This tool offers functionalities for direct SVN to Git conversion and author list generation, providing flexibility for different migration needs.

## Features

*   **Live SVN Repository to Git Migration:** Converts a live SVN repository directly to a Git repository, eliminating the need for an SVN dump file.
*   **Direct SVN URL Input:** Takes the URL of your SVN repository as direct input.
*   **Git Repository Creation:** Creates a fully functional Git repository mirroring the SVN history.
*   **Author List Generation:**  Generates a simple author list from the SVN repository history, useful for creating author mapping files.
*   **Verbose Output Mode:** Provides detailed console output, including previewing file contents (in non-Git mode), using the `-v` flag.
*   **Author Email Domain Configuration:**  Allows setting a default email domain when generating author lists, ensuring all authors have valid email addresses in the output.

## Getting Started

### Prerequisites

*   [.NET SDK](https://dotnet.microsoft.com/download) (version 9.0 or higher - specify the minimum required version)

### Installation

1.  **Clone the repository:**

    ```bash
    git clone <repository-url>
    cd SvnRepo2Git
    ```

2.  **Build the project:**

    ```bash
    dotnet build
    ```

    This will create the executable in the `SvnRepo2Git/bin/Debug/net9.0` or `SvnRepo2Git/bin/Release/net9.0` directory (or similar, depending on your build configuration and .NET version).

### Usage

Navigate to the output directory containing the executable (e.g., `SvnRepo2Git/bin/Debug/net9.0`).

```bash
cd SvnRepo2Git/bin/Debug/net9.0
```

Run the `SvnRepo2Git` tool with the following command-line arguments:

```bash
SvnRepo2Git.exe <svn_repo_url> [--git <git_repo_path>] [--genauthors <e-mail domain>] [-v]
```

**Command-Line Switches:**

*   `<svn_repo_url>`: **(Required)** The URL of the Subversion repository to export. This is the source repository for the conversion (e.g., `https://svn.example.com/repo`).
*   `--git <git_repo_path>`: **(Optional)** Specifies the directory where the new Git repository will be created. If not provided, the output will be a console display of the SVN revisions instead of Git conversion. (e.g., `--git ./my-git-repo`).
*   `--genauthors <e-mail domain>`: **(Optional)** Enables author list generation mode.  Requires specifying an `<e-mail domain>` which will be used as the default domain for authors without explicitly defined email addresses.  This generates an `authors.txt` file in the current directory. (e.g., `--genauthors example.com`).
*   `-v`: **(Optional)** Enables verbose output.  This includes previewing file contents in the console output (if not converting to Git) and provides more detailed information during the process.

**Examples:**

1.  **Console Output (Preview Content, no Git conversion):**

    ```bash
    SvnRepo2Git.exe https://svn.example.com/repo
    ```

2.  **Convert to Git Repository:**

    ```bash
    SvnRepo2Git.exe https://svn.example.com/repo --git ./my-git-repo
    ```

3.  **Generate Authors List:**

    ```bash
    SvnRepo2Git.exe https://svn.example.com/repo --genauthors example.com
    ```

4.  **Verbose Output with Git Conversion:**

    ```bash
    SvnRepo2Git.exe https://svn.example.com/repo --git ./my-git-repo -v
    ```

5.  **Generate Authors List and Verbose Output:**
    ```bash
    SvnRepo2Git.exe https://svn.example.com/repo --genauthors example.com -v
    ```

## Workflow

For a smooth SVN to Git migration, it's recommended to follow these steps:

1.  **Generate Author List:**
    *   First, generate an initial author list from your SVN repository. This will create an `authors.txt` file containing a list of SVN authors and their email addresses (using the provided domain as a default).
    *   Use the `--genauthors <e-mail domain>` switch:
        ```bash
        SvnRepo2Git.exe <svn_repo_url> --genauthors yourdomain.com
        ```
    *   This step helps you identify all unique authors in your SVN history.

2.  **Review and Correct Author List (Optional but Recommended):**
    *   Examine the generated `authors.txt` file.
    *   **Correct Email Addresses:**  If any authors have incorrect or missing email addresses (e.g., using the default domain when a real email exists), manually edit the `authors.txt` file to provide accurate email addresses.
    *   **Consolidate Authors (Optional):** If you need to merge multiple SVN author names into a single Git author (e.g., due to name variations or service accounts), you can edit the `authors.txt` file to map these entries to the desired Git author email.
    *   **Example `authors.txt` format:**
        ```
        svnuser1 svnuser1@yourdomain.com
        another_svn_user real.email@example.org
        service_account service@internal.net
        ```

3.  **Perform SVN to Git Conversion:**
    *   Once you have reviewed and (optionally) corrected your author list, you can proceed with the SVN to Git conversion.
    *   Use the `--git <git_repo_path>` switch to specify the output Git repository path.
        ```bash
        SvnRepo2Git.exe <svn_repo_url> --git ./my-git-repo
        ```
By following this workflow, you can ensure a more accurate and well-prepared migration of your SVN repository to Git.
