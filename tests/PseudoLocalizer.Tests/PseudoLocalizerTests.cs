using Shouldly;
using Xunit;

namespace PseudoLocalizer.Tests
{
    public class PseudoLocalizerTests
    {
        [Fact]
        public void PseudoLocalize_TextIsNull_ReturnsNull()
        {
            var result = PseudoLocalizer.PseudoLocalize(null);
            result.ShouldBeNull();
        }

        [Fact]
        public void PseudoLocalize_TextIsEmpty_ReturnsEmptyString()
        {
            var result = PseudoLocalizer.PseudoLocalize(string.Empty);
            result.ShouldBeEmpty();
        }

        [Theory]
        [InlineData("TXT", "[ŢẊŢ]")]
        [InlineData("Some text to transform", "[Šööɱéé ţééẋţ ţöö ţŕååñšƒööŕɱ]")]
        public void PseudoLocalize_TextIsNotNullOrEmpty_ReturnsTransformedText(string text, string expected)
        {
            var result = PseudoLocalizer.PseudoLocalize(text);
            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData("Hello", "[Ĥééļļöö]")]
        [InlineData("Hello John", "[Ĥééļļöö Ĵööĥñ]")]
        public void PseudoLocalize_TextWithVowels_ReturnsTransformedTextWithDuplicatedVowels(string text, string expected)
        {
            var result = PseudoLocalizer.PseudoLocalize(text);
            result.ShouldBe(expected);
            result.Length.ShouldBeGreaterThan(text.Length);
        }

        [Theory]
        [InlineData("Hello {0}", "[Ĥééļļöö {0}]")]
        [InlineData("Use {0:d/M/yyyy HH:mm:ss} datetime format", "[ÛÛšéé {0:d/M/yyyy HH:mm:ss} ðååţééţîîɱéé ƒööŕɱååţ]")]
        public void PseudoLocalize_TextWithPlaceholder_ReturnsTransformedTextWithPlaceholder(string text, string expected)
        {
            var result = PseudoLocalizer.PseudoLocalize(text);
            result.ShouldBe(expected);
            result.Length.ShouldBeGreaterThan(text.Length);
        }

        [Theory]
        [InlineData("Hello <strong>John</strong>", "[Ĥééļļöö <strong>Ĵööĥñ</strong>]")]
        [InlineData("Click <a href=\"https://www.google.com/\">here</a> and google it.", "[Çļîîçķ <a href=\"https://www.google.com/\">ĥééŕéé</a> ååñð ĝööööĝļéé îîţ·]")]
        [InlineData("Line <br /> break", "[Ļîîñéé <br /> ƀŕééååķ]")]
        public void PseudoLocalize_TextWithHtmlTag_ReturnsTransformedTextWithHtmlTag(string text, string expected)
        {
            var result = PseudoLocalizer.PseudoLocalize(text);
            result.ShouldBe(expected);
            result.Length.ShouldBeGreaterThan(text.Length);
        }

        [Fact]
        public void PseudoLocalize_PreservesWords()
        {
            var text = "Hello John. How are you?";
            var result = PseudoLocalizer.PseudoLocalize(text);
            result.Split(' ').Length.ShouldBe(5);
        }
    }
}
