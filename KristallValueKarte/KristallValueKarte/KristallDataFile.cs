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
        private Dictionary<string, KristallDataGroup> _dataGroups = new Dictionary<string, KristallDataGroup>();
		
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
                _dataGroups.Add(dataGroup.Name + "-" + dataGroup.Identifier.ToString(), dataGroup);
            }

            kvkCode = Regex.Replace(kvkCode, "G(.*?)G", "");
            MatchCollection pieceCodes = Regex.Matches(kvkCode, "K([^K]*)");
            KristallDataPiece dataPiece;
            foreach (Match piece in pieceCodes)
            {
                if (piece.Value.Length == 1) // must be the final K of the file
                    break;
                dataPiece = KristallDataPiece.Parse(piece.Value);
                _dataPieces.Add(dataPiece.Name, dataPiece);
            }
        }
		
		public void AddGroup(KristallDataGroup group)
		{
			if (_dataGroups.ContainsKey(group.Name + "-" + group.Identifier.ToString()))
				throw new ArgumentException("The data file already contains a group with this name and identifier.");
			_dataGroups.Add(group.Name + "-" + group.Identifier.ToString(), group);
		}
		
		public void AddPiece(KristallDataPiece piece)
		{
			if (_dataPieces.ContainsKey(piece.Name))
				throw new ArgumentException("The data file already contains a piece with this name.");
			_dataPieces.Add(piece.Name, piece);
		}
		
        public KristallDataPiece Piece(string name)
        {
            if (_dataPieces.ContainsKey(name))
                return _dataPieces[name];
            return null;
        }

        public KristallDataGroup Group(string name, object identifier)
        {
            if (_dataGroups.ContainsKey(name + "-" + identifier.ToString()))
                return _dataGroups[name + "-" + identifier.ToString()];
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
		public static string Encode(KristallDataType dataType, object value)
		{
			StringBuilder stringFactory = new StringBuilder();
			switch (dataType)
			{
				case (KristallDataType.String):
				{
					foreach (char character in (value as string))
					{
						stringFactory.Append(String.Format("{0:x4}", Convert.ToInt16(character)));
					}
					return stringFactory.ToString().ToUpper();
				}
			}
			throw new ArgumentException();
		}
		
		public void Save(string filename)
		{
			StringBuilder stringFactory = new StringBuilder();
			foreach (KristallDataPiece dataPiece in _dataPieces.Values)
				stringFactory.Append(KristallDataPiece.Encode(dataPiece));
			stringFactory.Append("K");
			
			FileStream stream = new FileStream(filename, FileMode.Create);
			TextWriter writer = new StreamWriter(stream);
			writer.WriteLine(stringFactory.ToString());
			writer.Close();
			stream.Close();
		}
    }
}
