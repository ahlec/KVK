using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace KristallValueKarte
{
    public class KristallDataFile
    {
        private Dictionary<string, KristallDataPiece> _dataPieces = new Dictionary<string, KristallDataPiece>();
        private Dictionary<object, KristallDataGroup> _dataGroups = new Dictionary<object, KristallDataGroup>();
		
		public KristallDataFile()
		{
		}
		
        public KristallDataFile(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open);
            TextReader reader = new StreamReader(stream);
            string kvkCode = reader.ReadToEnd();
            reader.Close();
            stream.Close();

            MatchCollection groupCodes = Regex.Matches(kvkCode, "G(.*?)G");
            KristallDataGroup dataGroup;
            foreach (Match group in groupCodes)
            {
                dataGroup = new KristallDataGroup(group.Value);
                _dataGroups.Add(dataGroup.Identifier, dataGroup);
            }

            kvkCode = Regex.Replace(kvkCode, "G(.*?)G", "");
            MatchCollection pieceCodes = Regex.Matches(kvkCode, "K([^K]*)");
            KristallDataPiece dataPiece;
            foreach (Match piece in pieceCodes)
            {
                if (piece.Value.Length == 1) // must be the final K of the file
                    break;
                dataPiece = new KristallDataPiece(piece.Value);
                _dataPieces.Add(dataPiece.Name, dataPiece);
            }
        }

        public KristallDataPiece Piece(string name)
        {
            if (_dataPieces.ContainsKey(name))
                return _dataPieces[name];
            return null;
        }

        public KristallDataGroup Group(object identifier)
        {
            if (_dataGroups.ContainsKey(identifier))
                return _dataGroups[identifier];
            return null;
        }

        public static object Decode(KristallDataType dataType, string value)
        {
            switch (dataType)
            {
                case (KristallDataType.String):
                    {
                        StringBuilder stringFactory = new StringBuilder();
                        for (int offset = 0; offset < value.Length; offset += 4)
                        {
                            stringFactory.Append(Convert.ToChar(int.Parse(value.Substring(offset, 4),
                                System.Globalization.NumberStyles.HexNumber)));
                        }
                        return stringFactory.ToString();
                    }
                case (KristallDataType.Boolean):
                    {
                        return int.Parse(value) == 1 ? true : false;
                    }
            }
            throw new ArgumentException();
        }
		
		public void Save(string filename)
		{
			throw new NotImplementedException();
		}
    }
}
