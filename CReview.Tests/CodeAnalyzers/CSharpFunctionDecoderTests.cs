using CReview.CodeAnalyzers.CSharp.Decoders;
using CReview.CodeAnalyzers.Models;
using CReview.Tests.Helpers;
using NUnit.Framework;
using System.Linq;

namespace CReview.Tests
{

    [TestFixture]
    class CSharpFunctionDecoderTests : UnderTest<CSharpFunctionDecoder>
    {

        private string RemakeSignature(FunctionSignature signature)
        {
            var arguments = string
                .Join(", ", signature.Arguments.Select(e => $"{e.Type} {e.Name}"))
                .Trim();

            return $"{signature.ReturnType} {signature.FunctionName}({arguments})";
        }

        [TestCase("int Sum(int x, int y)")]
        [TestCase("int Multiply(int x, int y)")]
        [TestCase("int Multiply(double x, int y)")]
        [TestCase("int Increment(int x)")]
        [TestCase("void DoSomething()")]
        public void When_decoding_a_simple_function_it_should_return_function_signature(string signature)
        {
            var codeTree = Subject.DecodeFunction(signature);

            Assert.That(codeTree.Signature, Is.Not.Null);
            Assert.That(RemakeSignature(codeTree.Signature), Is.EqualTo(signature));
        }

        [Test]
        public void When_decoding_a_function_with_empty_body_it_should_return_an_empty_body()
        {
            var function = @"
void DoSomething()
{
    
}";

            var codeTree = Subject.DecodeFunction(function);

            Assert.That(codeTree.Body, Is.Not.Null);
            Assert.That(codeTree.Body.LineCount, Is.EqualTo(3));
            Assert.That(codeTree.Body.Lines[0], Is.EqualTo("{"));
            Assert.That(codeTree.Body.Lines[1], Is.EqualTo("    "));
            Assert.That(codeTree.Body.Lines[2], Is.EqualTo("}"));
        }

        [Test]
        public void When_decoding_a_function_with_1_line_of_code_it_should_return_a_body_with_that_line_of_code()
        {
            var function = @"
void DoSomething()
{
    int x = 10;
}";

            var codeTree = Subject.DecodeFunction(function);

            Assert.That(codeTree.Body, Is.Not.Null);
            Assert.That(codeTree.Body.LineCount, Is.EqualTo(3));
            Assert.That(codeTree.Body.Lines[0], Is.EqualTo("{"));
            Assert.That(codeTree.Body.Lines[1], Is.EqualTo("    int x = 10;"));
            Assert.That(codeTree.Body.Lines[2], Is.EqualTo("}"));
        }

    }
}
