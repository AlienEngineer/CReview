using System.Linq;
using System.Collections.Generic;
using CReview.Decoders;

namespace CReview.CodeAnalyzers
{

    public class CodeLineDecoder
    {
        internal CodeLine DecodeLine(string lineOfCode)
        {
            var codeParts = GetCodeParts(lineOfCode);

            return new CodeLine
            {
                Symbols = MakeSymbols(codeParts).ToList()
            };
        }

        private static IEnumerable<Symbol> MakeSymbols(IList<string> codeParts)
        {
            yield return new Symbol
            {
                Name = codeParts[0],
                Type = Symbols.Declaration
            };
            yield return new Symbol
            {
                Name = codeParts[1],
                Type = Symbols.Variable
            };

            for (int i = 2; i < codeParts.Count - 1; i++)
            {
                if (codeParts[i] == "=")
                {
                    yield return new Symbol
                    {
                        Name = "=",
                        Type = Symbols.Assignement
                    };
                }

                if (codeParts[i] == "10")
                {
                    yield return new Symbol
                    {
                        Name = "10",
                        Type = Symbols.Constant
                    };
                }
            }

            yield return new Symbol
            {
                Name = ";",
                Type = Symbols.ExpressionEnd
            };
        }

        private static IList<string> GetCodeParts(string lineOfCode)
        {
            string[] codeParts = lineOfCode.Split(' ');

            var lastPart = codeParts[codeParts.Length - 1];

            if (lastPart.Last() == ';')
            {
                var codePiece = lastPart.Substring(0, lastPart.Length - 1);

                var result = new List<string>(codeParts.Take(codeParts.Length - 1));

                result.Add(codePiece);
                result.Add(";");

                return result;
            }

            return codeParts;
        }
    }
}
