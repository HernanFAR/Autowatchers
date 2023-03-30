namespace Autowatchers.Models;

internal record FileData(FileDataType Type, string FileName, string Text);

internal enum FileDataType : byte
{
    Attribute,
    Class,
    Subclass
}