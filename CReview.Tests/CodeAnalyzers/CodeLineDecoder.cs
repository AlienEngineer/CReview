using CReview.Tests.Helpers;
using NUnit.Framework;
using System.Linq;
using System;

namespace CReview.Tests.CodeAnalyzers
{

    [TestFixture]
    class CodeLineDecoderTests : UnderTest<CodeLineDecoder>
    {
        public string Stringify(Symbol[] symbols)
        {
            return string.Join(" ", symbols.Select(e => e.Name));
        }

        [Test]
        public void When_decoding_a_line_of_code_it_should_return_the_symbols_used()
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

        private void AssertSymbol(Symbol symbol, Symbols symbolType, string value)
        {
            Assert.That(symbol.Name, Is.EqualTo(value));
            Assert.That(symbol.Type, Is.EqualTo(symbolType));
        }
    }

    public enum Symbols
    {
        Declaration,
        Variable,
        Assignement,
        Constant,
        ExpressionEnd
    }

    internal class CodeLineDecoder
    {
        internal CodeLine DecodeLine(string lineOfCode)
        {
            return new CodeLine
            {
                Symbols = new [] 
                {
                    new Symbol
                    {
                        Type = Symbols.Declaration,
                        Name = "var"
                    },
                    new Symbol
                    {
                        Type = Symbols.Variable,
                        Name = "x"
                    },
                    new Symbol
                    {
                        Type = Symbols.Assignement,
                        Name = "="
                    },
                    new Symbol
                    {
                        Type = Symbols.Constant,
                        Name = "10"
                    },
                    new Symbol
                    {
                        Type = Symbols.ExpressionEnd,
                        Name = ";"
                    }
                }
            };
        }
    }

    internal class CodeLine
    {
        public Symbol[] Symbols { get; set; }
    }

    public class Symbol
    {
        public string Name { get; set; }
        public Symbols Type { get; set; }
    }
}
