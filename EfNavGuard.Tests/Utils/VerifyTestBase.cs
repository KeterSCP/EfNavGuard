namespace EfNavGuard.Tests.Utils;

public abstract class VerifyTestBase
{
    static VerifyTestBase()
    {
        Environment.SetEnvironmentVariable("DiffEngine_Disabled", "true");
        Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");
    }

    protected static VerifySettings GetSettings(params object?[] parameters)
    {
        var settings = new VerifySettings();
        settings.UseDirectory("VerifyResults");
        if (parameters.Length > 0)
            settings.UseParameters(parameters);
        return settings;
    }
}