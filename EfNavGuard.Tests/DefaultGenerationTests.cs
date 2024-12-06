using System.Collections.Immutable;
using EfNavGuard.Tests.Utils;
using Microsoft.CodeAnalysis.Text;

namespace EfNavGuard.Tests;

public class DefaultGenerationTests : VerifyTestBase
{
    [Fact(DisplayName = "Should correctly generate code for navigation property with \"init\" initializer")]
    public async Task ShouldCorrectlyGenerateCodeForNavigationPropertyWithInitInitializer()
    {
        var sourceTexts = ImmutableArray.Create
        (
            SourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            SourceText.From(
                """
                using EfNavGuard;
                using SourceRootNamespace.SourceNamespace;

                namespace TargetRootNamespace.TargetNamespace
                {
                    public partial class MyClass
                    {
                        [GuardedNavigation]
                        public partial MyClass2 NavProp { get; init; }
                    }
                }
                """
            )
        );

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts)
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }

    [Fact(DisplayName = "Should correctly generate code for navigation property with \"set\" initializer")]
    public async Task ShouldCorrectlyGenerateCodeForNavigationPropertyWithSetInitializer()
    {
        var sourceTexts = ImmutableArray.Create
        (
            SourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            SourceText.From(
                """
                using EfNavGuard;
                using SourceRootNamespace.SourceNamespace;

                namespace TargetRootNamespace.TargetNamespace
                {
                    public partial class MyClass
                    {
                        [GuardedNavigation]
                        public partial MyClass2 NavProp { get; set; }
                    }
                }
                """
            )
        );

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts)
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }

    [Fact(DisplayName = "Should correctly generate nullable annotations for navigation property when NRT is enabled")]
    public async Task ShouldCorrectlyGenerateNullableAnnotationsForNavigationPropertyWhenNrtIsEnabled()
    {
        var sourceTexts = ImmutableArray.Create
        (
            SourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            SourceText.From(
                """
                using EfNavGuard;
                using SourceRootNamespace.SourceNamespace;

                namespace TargetRootNamespace.TargetNamespace
                {
                    public partial class MyClass
                    {
                        [GuardedNavigation]
                        public partial MyClass2 NavProp { get; set; }
                    }
                }
                """
            )
        );

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts, msbuildProperties: new Dictionary<string, string>
            {
                ["nullable"] = "enable"
            })
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }

    [Fact(DisplayName = "Should correctly generate nullable annotations for navigation property when NRT is disabled")]
    public async Task ShouldCorrectlyGenerateNullableAnnotationsForNavigationPropertyWhenNrtIsDisabled()
    {
        var sourceTexts = ImmutableArray.Create
        (
            SourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            SourceText.From(
                """
                using EfNavGuard;
                using SourceRootNamespace.SourceNamespace;

                namespace TargetRootNamespace.TargetNamespace
                {
                    public partial class MyClass
                    {
                        [GuardedNavigation]
                        public partial MyClass2 NavProp { get; set; }
                    }
                }
                """
            )
        );

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts, msbuildProperties: new Dictionary<string, string>
            {
                ["nullable"] = "disable"
            })
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }
}