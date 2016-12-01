using CReview.Models;
using CReview.Tests.Helpers;
using NUnit.Framework;
using System.Linq;

namespace CReview.Tests
{

    [TestFixture]
    class FunctionDecoderTests : UnderTest<FunctionDecoder>
    {
        [TestCase("int Sum(int x, int y)")]
        [TestCase("int Multiply(int x, int y)")]
        [TestCase("int Multiply(double x, int y)")]
        [TestCase("int Increment(int x)")]
        [TestCase("void DoSomething()")]
        public void When_decoding_a_simple_function_it_should_return_function_signature(string function)
        {
            var codeTree = Subject.DecodeFunction(function);

            Assert.That(codeTree.Signature, Is.Not.Null);
            Assert.That(RemakeCode(codeTree.Signature), Is.EqualTo(function));
        }

        private string RemakeCode(FunctionSignature signature)
        {
            var arguments = string
                .Join(", ", signature.Arguments.Select(e => $"{e.Type} {e.Name}"))
                .Trim();

            return $"{signature.ReturnType} {signature.FunctionName}({arguments})";
        }
    }
}
