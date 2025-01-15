using System.Collections.Immutable;
using EfNavGuard.Tests.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EfNavGuard.Tests;

public class DefaultGenerationTests : VerifyTestBase
{
    public static TheoryData<LanguageVersion> OlderThanCSharp13Versions =
    [
        LanguageVersion.CSharp1,
        LanguageVersion.CSharp2,
        LanguageVersion.CSharp3,
        LanguageVersion.CSharp4,
        LanguageVersion.CSharp5,
        LanguageVersion.CSharp6,
        LanguageVersion.CSharp7,
        LanguageVersion.CSharp7_1,
        LanguageVersion.CSharp7_2,
        LanguageVersion.CSharp7_3,
        LanguageVersion.CSharp8,
        LanguageVersion.CSharp9,
        LanguageVersion.CSharp10,
        LanguageVersion.CSharp11,
        LanguageVersion.CSharp12
    ];

    [Fact(DisplayName = "Should correctly generate code for navigation property with \"init\" initializer")]
    public async Task ShouldCorrectlyGenerateCodeForNavigationPropertyWithInitInitializer()
    {
        var sourceTexts = ImmutableArray.Create
        (
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
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
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
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
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
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

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts, NullableContextOptions.Enable)
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }

    [Fact(DisplayName = "Should correctly generate nullable annotations for navigation property when NRT is disabled")]
    public async Task ShouldCorrectlyGenerateNullableAnnotationsForNavigationPropertyWhenNrtIsDisabled()
    {
        var sourceTexts = ImmutableArray.Create
        (
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
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

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts, nullableContextOptions: NullableContextOptions.Disable)
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings());
    }

    [Fact(DisplayName = "Should skip readonly navigation property when generating code")]
    public async Task ShouldSkipReadonlyNavigationPropertyWhenGeneratingCode()
    {
        var sourceTexts = ImmutableArray.Create
        (
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
                """
                using EfNavGuard;
                using SourceRootNamespace.SourceNamespace;

                namespace TargetRootNamespace.TargetNamespace
                {
                    public partial class MyClass
                    {
                        [GuardedNavigation]
                        public partial MyClass2 NavProp { get; }
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

    [Theory(DisplayName = "Should not generate code if project language version is less than C# 13.0")]
    [MemberData(nameof(OlderThanCSharp13Versions))]
    public async Task ShouldNotGenerateCodeIfProjectLanguageVersionIsLessThanCSharp13(LanguageVersion languageVersion)
    {
        var sourceTexts = ImmutableArray.Create
        (
            CSharpSourceText.From(
                """
                namespace SourceRootNamespace.SourceNamespace
                {
                    public class MyClass2{}
                }
                """
            ),
            CSharpSourceText.From(
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

        var generatedClassesTexts = TestHelper.GetAllSyntaxTrees(sourceTexts, languageVersion: languageVersion)
            .Select(st => st.GetText().ToString())
            .ToList();

        await Verify(generatedClassesTexts, GetSettings()).UseParameters(languageVersion);
    }
}