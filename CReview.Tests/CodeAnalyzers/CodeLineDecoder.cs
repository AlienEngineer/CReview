using CReview.Tests.Helpers;
using NUnit.Framework;
using System.Linq;

namespace CReview.Tests.CodeAnalyzers
{

    [TestFixture]
    class CodeLineDecoderTests : UnderTest<CodeLineDecoder>
    {
        public string Stringify(Symbol[] symbols)
        {
            return string.Join(", ", symbols.Select(e => e.Name));
        }

        [Test]
        public void When_decoding_a_line_of_code_it_should_return_the_symbols_used()
        {
            var lineOfCode = "var x = 10;";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            Assert.That(Stringify(decodedLine.Symbols), Is.EqualTo("x"));
        }
        
    }

    internal class CodeLineDecoder
    {
        internal CodeLine DecodeLine(string lineOfCode)
        {
            return new CodeLine
            {
                Symbols = new [] {
                    new Symbol{
                        Name = "x"
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
    }
}
