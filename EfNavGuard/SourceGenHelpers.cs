namespace EfNavGuard;

internal static class SourceGenHelpers
{
    public const string MarkerAttributeName = "GuardedNavigationAttribute";

    public const string GuardedNavigationAttributeCode =
        $$"""
          namespace EfNavGuard
          {
              /// <summary>
              /// Accessing navigation property marked with this attribute will throw an exception if it is not loaded.
              /// </summary>
              [System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
              public sealed class {{MarkerAttributeName}} : System.Attribute
              {
              }
          }
          """;
}