namespace Autowatchers.Models;

internal record FileData(FileDataType Type, string FileName, string Text);

internal enum FileDataType : byte
{
    None,

    Attribute,

    Base,

    Builder,

    ArrayBuilder,

    IEnumerableBuilder,

    IListBuilder,

    ICollectionBuilder,

    IReadOnlyCollectionBuilder,

    IDictionaryBuilder
}