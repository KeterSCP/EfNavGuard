namespace EfNavGuard;

internal class GeneratorOptions
{
    public bool NullableEnabled { get; }

    public GeneratorOptions(bool nullableEnabled)
    {
        NullableEnabled = nullableEnabled;
    }
}