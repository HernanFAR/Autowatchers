using Autowatchers.Models;

namespace Autowatchers.FileGenerators;

internal interface IFileGenerator
{
    FileData GenerateFile();
}

internal interface IFilesGenerator
{
    IReadOnlyList<FileData> GenerateFiles();
}
