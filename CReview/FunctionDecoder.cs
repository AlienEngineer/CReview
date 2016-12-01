using System.Linq;
using System.Collections.Generic;
using CReview.Models;

namespace CReview
{

    public class FunctionDecoder
    {
        public Function DecodeFunction(string function)
        {
            return new Function
            {
                Signature = DecodeSignature(function)
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
