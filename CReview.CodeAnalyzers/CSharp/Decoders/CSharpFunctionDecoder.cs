using System.Linq;
using System.Collections.Generic;
using System;
using CReview.CodeAnalizers.Models;

namespace CReview.CodeAnalizers.CSharp.Decoders
{

    public class CSharpFunctionDecoder : IFunctionDecoder
    {
        public Function DecodeFunction(string function)
        {
            var functionLines = BreakFunctionIntoLines(function);

            return new Function
            {
                Signature = DecodeSignature(functionLines[0]),
                Body = DecodeBody(functionLines)
            };
        }

        private string[] BreakFunctionIntoLines(string function)
        {
            return function.Split(
                new[] { "\r\n" }, 
                StringSplitOptions.RemoveEmptyEntries
            );
        }

        private FunctionBody DecodeBody(string[] function)
        {
            var lines = function.Skip(1)
                .ToArray();
            return new FunctionBody
            {
                LineCount = lines.Length,
                Lines = lines
            };
        }

        private FunctionSignature DecodeSignature(string signatureLine)
        {
            var tokens = signatureLine.Split(' ', '(', ',', ')');

            return new FunctionSignature
            {
                ReturnType = tokens[0],
                FunctionName = tokens[1],
                Arguments = MakeArguments(tokens).ToArray()
            };
        }

        private IEnumerable<FunctionArgument> MakeArguments(string[] token)
        {
            const int NUMBER_OF_ENTRIES_PER_ARGUMENT = 3;
            const int START_AFTER_NAME = 2;

            for (int i = START_AFTER_NAME; i < token.Length; i += NUMBER_OF_ENTRIES_PER_ARGUMENT)
            {
                yield return new FunctionArgument
                {
                    Type = token[i],
                    Name = token[i + 1]
                };
            }
        }
    }
}
