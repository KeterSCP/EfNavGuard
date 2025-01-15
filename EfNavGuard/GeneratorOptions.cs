namespace EfNavGuard;

internal class GeneratorOptions
{
    public bool NullableEnabled { get; }
    public int LanguageVersion { get; }

    public GeneratorOptions(bool nullableEnabled, int languageVersion)
    {
        NullableEnabled = nullableEnabled;
        LanguageVersion = languageVersion;
    }
}