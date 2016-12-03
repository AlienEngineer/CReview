using System.Linq;
using System.Collections.Generic;
using CReview.Decoders;
using System;

namespace CReview.CodeAnalyzers
{

    public class CodeLineDecoder : ICodeLineDecoder
    {
        static IDictionary<SymbolType, SymbolType> _translationTable = new Dictionary<SymbolType, SymbolType>
        {
            { SymbolType.Declaration, SymbolType.Variable },
            { SymbolType.Assignement, SymbolType.Constant }
        };

        public CodeLine DecodeLine(string lineOfCode)
        {
            var codeParts = GetCodeParts(lineOfCode);

            return new CodeLine
            {
                Symbols = MakeSymbols(codeParts).ToList()
            };
        }

        private static IEnumerable<Symbol> MakeSymbols(IList<string> codeParts)
        {
            var symbols = GetSymbols(codeParts);

            for (int position = 0; position < symbols.Count; position++)
            {
                var currentSymbol = symbols[position];
                currentSymbol.Type = ResolveSymbolType(currentSymbol, position, symbols);
            }

            return symbols;
        }

        private static SymbolType ResolveSymbolType(Symbol currentSymbol, int position, IList<Symbol> symbols)
        {
            if (currentSymbol.Name.Contains("("))
            {
                ReevaluatePreviousSymbolType(position, symbols);

                return SymbolType.FunctionCall;
            }

            if (currentSymbol.Name == "new")
            {
                return SymbolType.Initialize;
            }

            if (position == 0)
            {
                return SymbolType.Declaration;
            }

            if (position == symbols.Count - 1)
            {
                return SymbolType.ExpressionEnd;
            }

            if (currentSymbol.Name == "=")
            {
                return SymbolType.Assignement;
            }

            return ResolveSymbolBasedOnPrevious(position, symbols);
        }

        private static void ReevaluatePreviousSymbolType(int position, IList<Symbol> symbols)
        {
            if (position == 0)
            {
                return;
            }

            Symbol previousSymbol = symbols[position - 1];
            if (previousSymbol.Type == SymbolType.Assignement)
            {
                return;
            }

            previousSymbol.Type = ResolveSymbolBasedOnPrevious(position, symbols);
        }

        private static SymbolType ResolveSymbolBasedOnPrevious(int position, IList<Symbol> symbols)
        {
            SymbolType previousSymbol = symbols[position - 1].Type;
            
            return _translationTable.TryGetValue(previousSymbol, out var translatedSymbol)
                ? translatedSymbol
                : previousSymbol;
        }

        private static IList<Symbol> GetSymbols(IList<string> codeParts)
        {
            return codeParts
                .Select(codePart => new Symbol { Name = codePart })
                .ToList();
        }

        private static IEnumerable<string> MakeCodeParts(string lineOfCode)
        {
            string[] codeParts = lineOfCode.Split(' ');
            foreach (var part in codeParts)
            {
                if (part.Contains("."))
                {
                    foreach (var subPart in part.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        yield return subPart;
                    }
                }
                else if (part.EndsWith(";"))
                {
                    yield return part.Substring(0, part.Length - 1);
                    yield return ";";
                }
                else
                {
                    yield return part;
                }
            }
        }

        private static IList<string> GetCodeParts(string lineOfCode)
        {
            return MakeCodeParts(lineOfCode).ToList();
        }


    }
}
