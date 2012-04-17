using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KristallValueKarte
{
    public class KristallDataGroup
    {
		private string _name;
		private KristallDataType _identifierType;
        private object _identifier;
        private Dictionary<string, KristallDataPiece> _dataPieces = new Dictionary<string, KristallDataPiece>();
		
		private KristallDataGroup() // only used when parsing from a file
		{
		}
		
		private KristallDataGroup(string name)
		{
			_name = name;
		}
		public KristallDataGroup(string name, int identifierValue) : this(name)
		{
			_identifierType = KristallDataType.Integer;
			_identifier = identifierValue;
		}
		public KristallDataGroup(string name, float identifierValue) : this(name)
		{
			_identifierType = KristallDataType.Float;
			_identifier = identifierValue;
		}
		public KristallDataGroup(string name, double identifierValue) : this(name)
		{
			_identifierType = KristallDataType.Double;
			_identifier = identifierValue;
		}
		public KristallDataGroup(string name, string identifierValue) : this(name)
		{
			_identifierType = KristallDataType.String;
			_identifier = identifierValue;
		}
		public KristallDataGroup(string name, bool identifierValue) : this(name)
		{
			_identifierType = KristallDataType.Boolean;
			_identifier = identifierValue;
		}
			
        public static KristallDataGroup Parse(string code)
        {
			KristallDataGroup parsed = new KristallDataGroup();
			Match nameMatch = Regex.Match(code, "N([0-9ABCDEF]*)");
			parsed._name = KristallDataFile.Decode(KristallDataType.String, nameMatch.Value.Substring(1)) as string;
			
			Match identifierMatch = Regex.Match(code, "I([0-9ABCDEF]*)");
			
			parsed._identifierType = (KristallDataType)int.Parse(identifierMatch.Value.Substring (1, 4));
            parsed._identifier = KristallDataFile.Decode(parsed._identifierType, identifierMatch.Value.Substring(5));

            MatchCollection pieceCodes = Regex.Matches(code, "K([^K]*)");
            KristallDataPiece dataPiece;
            foreach (Match piece in pieceCodes)
            {
                if (piece.Value.Length == 2) // must be the final K and G of the group
                    break;
                dataPiece = new KristallDataPiece(piece.Value);
                parsed._dataPieces.Add(dataPiece.Name, dataPiece);
            }
			
			return parsed;
        }
		
		public string Name { get { return _name; } }
		private KristallDataType IdentifierType { get { return _identifierType; } }
        public object Identifier { get { return _identifier; } }
		
		public T GetIdentifier<T>()
		{
			switch (_identifierType)
			{
				case KristallDataType.String:
				{
					if (typeof(T) != typeof(String))
						throw new InvalidDataTypeException();
					break;
				}
				case KristallDataType.Integer:
				{
					if (typeof(T) != typeof(Int32))
						throw new InvalidDataTypeException();
					break;
				}
				case KristallDataType.Boolean:
				{
					if (typeof(T) != typeof(Boolean))
						throw new InvalidDataTypeException();
					break;
				}
				case KristallDataType.Double:
				{
					if (typeof(T) != typeof(Double))
						throw new InvalidDataTypeException();
					break;
				}
				case KristallDataType.Float:
				{
					if (typeof(T) != typeof(float))
						throw new InvalidDataTypeException();
					break;
				}
				default:
				{
					throw new InvalidDataTypeException();
				}
			}
			
			return (T)_identifier;
		}
		
		public void AddPiece(KristallDataPiece piece)
		{
			if (_dataPieces.ContainsKey(piece.Name))
				throw new ArgumentException("A piece with this name already exists within this group.");
			_dataPieces.Add(piece.Name, piece);
		}

        public KristallDataPiece Piece(string name)
        {
            if (_dataPieces.ContainsKey(name))
                return _dataPieces[name];
            return null;
        }
    }
}
