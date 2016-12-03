using CReview.CodeAnalyzers.CSharp.Decoders;
using CReview.Decoders;
using CReview.Tests.Helpers;
using NUnit.Framework;

namespace CReview.Tests.CodeAnalyzers.CSharp.Decoders
{

    [TestFixture]
    class CSharpCodeLineDecoderTests : UnderTest<CSharpCodeLineDecoder>
    {
        private void AssertSymbol(Symbol symbol, SymbolType symbolType, string value)
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
            AssertSymbol(decodedLine.Symbols[0], SymbolType.Declaration, "int");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.Variable, "x");
            AssertSymbol(decodedLine.Symbols[2], SymbolType.ExpressionEnd, ";");
        }

        [Test]
        public void When_decoding_a_declaration_with_assignement_it_should_return_the_symbols_used()
        {
            var lineOfCode = "var x = 10;";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.Declaration, "var");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.Variable, "x");
            AssertSymbol(decodedLine.Symbols[2], SymbolType.Assignement, "=");
            AssertSymbol(decodedLine.Symbols[3], SymbolType.Constant, "10");
            AssertSymbol(decodedLine.Symbols[4], SymbolType.ExpressionEnd, ";");
        }
        
        [Test]
        public void When_decoding_a_declaration_with_assignement_of_call_function_it_should_return_the_symbols_used()
        {
            var lineOfCode = "var x = GetNumber();";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.Declaration, "var");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.Variable, "x");
            AssertSymbol(decodedLine.Symbols[2], SymbolType.Assignement, "=");
            AssertSymbol(decodedLine.Symbols[3], SymbolType.FunctionCall, "GetNumber()");
            AssertSymbol(decodedLine.Symbols[4], SymbolType.ExpressionEnd, ";");
        }
        
        [Test]
        public void When_decoding_call_function_it_should_return_the_symbols_used()
        {
            var lineOfCode = "ProcessNumber();";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.FunctionCall, "ProcessNumber()");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.ExpressionEnd, ";");
        }
        
        [Test]
        public void When_decoding_call_function_with_parameter_it_should_return_the_symbols_used()
        {
            var lineOfCode = "ProcessNumber(10);";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.FunctionCall, "ProcessNumber(10)");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.ExpressionEnd, ";");
        }
        
        [Test]
        public void When_decoding_instanciating_an_object_it_should_return_the_symbols_used()
        {
            var lineOfCode = "new StringBuilder()";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.Initialize, "new");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.FunctionCall, "StringBuilder()");
        }

        [Test]
        public void When_decoding_a_fluent_function_call_it_should_return_the_symbols_used()
        {
            var lineOfCode = ".AppendLine()";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.FunctionCall, "AppendLine()");
        }


        [Test]
        public void When_decoding_a_var_function_call_it_should_return_the_symbols_used()
        {
            var lineOfCode = "builder.AppendLine()";

            var decodedLine = Subject.DecodeLine(lineOfCode);

            Assert.That(decodedLine, Is.Not.Null);
            AssertSymbol(decodedLine.Symbols[0], SymbolType.Variable, "builder");
            AssertSymbol(decodedLine.Symbols[1], SymbolType.FunctionCall, "AppendLine()");
        }

    }
}
