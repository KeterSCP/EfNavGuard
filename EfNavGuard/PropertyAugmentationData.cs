namespace EfNavGuard;

internal class PropertyAugmentationData
{
    public string FileName { get; }
    public string ContainingTypeNameStr { get; }
    public string PropertyFullTypeStr { get; }
    public string PropertyNameStr { get; }
    public string NamespaceNameStr { get; }
    public string PropertyInitializer { get; }

    public PropertyAugmentationData(string fileName, string containingTypeNameStr, string propertyFullTypeStr, string propertyNameStr, string namespaceNameStr, string propertyInitializer)
    {
        FileName = fileName;
        ContainingTypeNameStr = containingTypeNameStr;
        PropertyFullTypeStr = propertyFullTypeStr;
        PropertyNameStr = propertyNameStr;
        NamespaceNameStr = namespaceNameStr;
        PropertyInitializer = propertyInitializer;
    }
}