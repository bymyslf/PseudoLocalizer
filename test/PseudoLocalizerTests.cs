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
    }
}
