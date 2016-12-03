using CReview.Tests.Helpers;
using NUnit.Framework;

namespace CReview.Tests.CodeAnalyzers
{

    [TestFixture]
    class CodeLineDecoderTests : UnderTest<CodeLineDecoder>
    {
        private void AssertSymbol(Symbol symbol, Symbols symbolType, string value)
        {
            Assert.That(symbol.Name, Is.EqualTo(value));
            Assert.That(symbol.Type, Is.EqualTo(symbolType));
        }

        [Test]
        public void When_decoding_a_variable_declaration_it_should_return_the_symbols_used()
        {
            var lineOfCode = "int x;";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], Symbols.Declaration, "int");
            AssertSymbol(decodedLine.Symbols[1], Symbols.Variable, "x");
            AssertSymbol(decodedLine.Symbols[2], Symbols.ExpressionEnd, ";");
        }

        [Test]
        public void When_decoding_a_declaration_with_assignement_it_should_return_the_symbols_used()
        {
            var lineOfCode = "var x = 10;";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], Symbols.Declaration, "var");
            AssertSymbol(decodedLine.Symbols[1], Symbols.Variable, "x");
            AssertSymbol(decodedLine.Symbols[2], Symbols.Assignement, "=");
            AssertSymbol(decodedLine.Symbols[3], Symbols.Constant, "10");
            AssertSymbol(decodedLine.Symbols[4], Symbols.ExpressionEnd, ";");
        }
    }
}
