using Xunit;

namespace PseudoLocalizer.Tests
{
    public class PseudoLocalizerTests
    {
        [Fact]
        public void PseudoLocalize_TextIsNull_ReturnsNull()
        {
            var result = PseudoLocalizer.PseudoLocalize(null);

            Assert.Null(result);
        }

        [Fact]
        public void PseudoLocalize_TextIsEmpty_ReturnsEmptyString()
        {
            var result = PseudoLocalizer.PseudoLocalize(string.Empty);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void PseudoLocalize_TextIsNotNullOrEmpty_ReturnsTransformedText()
        {
            var text = "TXT";
            var expected = "[ŢẊŢ]";

            var result = PseudoLocalizer.PseudoLocalize(text);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PseudoLocalize_TextWithVowels_ReturnsTransformedTextWithDuplicatedVowels()
        {
            var text = "Hello";
            var expected = "[Ĥééļļöö]";

            var result = PseudoLocalizer.PseudoLocalize(text);

            Assert.Equal(expected, result);
            Assert.True(result.Length > text.Length);
        }

        [Fact]
        public void PseudoLocalize_TextWithPlaceholder_ReturnsTransformedTextWithPlaceholder()
        {
            var text = "Hello {0}";
            var expected = "[Ĥééļļöö {0}]";

            var result = PseudoLocalizer.PseudoLocalize(text);

            Assert.Equal(expected, result);
            Assert.True(result.Length > text.Length);
        }

        [Fact]
        public void PseudoLocalize_TextWithHtmlTag_ReturnsTransformedTextWithHtmlTag()
        {
            var text = "Hello <strong>John</strong>";
            var expected = "[Ĥééļļöö <strong>Ĵööĥñ</strong>]";

            var result = PseudoLocalizer.PseudoLocalize(text);

            Assert.Equal(expected, result);
            Assert.True(result.Length > text.Length);
        }
    }
}
