using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KristallValueKarte
{
    public class KristallDataGroup
    {
		private string _name;
        private object _identifier;
        private Dictionary<string, KristallDataPiece> _dataPieces = new Dictionary<string, KristallDataPiece>();

        public KristallDataGroup(string code)
        {
            Match identifierMatch = Regex.Match(code, "I([0-9ABCDEF]*)");
            _identifier = KristallDataFile.Decode((KristallDataType)int.Parse(identifierMatch.Value.Substring(1, 4)),
                identifierMatch.Value.Substring(5));

            MatchCollection pieceCodes = Regex.Matches(code, "K([^K]*)");
            KristallDataPiece dataPiece;
            foreach (Match piece in pieceCodes)
            {
                if (piece.Value.Length == 2) // must be the final K and G of the group
                    break;
                dataPiece = new KristallDataPiece(piece.Value);
                _dataPieces.Add(dataPiece.Name, dataPiece);
            }
        }
		
		public string Name { get { return _name; } }
        public object Identifier { get { return _identifier; } }

        public KristallDataPiece Piece(string name)
        {
            if (_dataPieces.ContainsKey(name))
                return _dataPieces[name];
            return null;
        }
    }
}
