namespace EfNavGuard;

internal static class SourceGenHelpers
{
    public const string MarkerAttributeName = "GuardedNavigationAttribute";
    public const string GlobalNamespaceName = "<global namespace>";

    public const string GuardedNavigationAttributeCode =
        $$"""
          // <auto-generated>
          // This code was generated by a tool, any changes made to it will be lost.
          // </auto-generated>
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