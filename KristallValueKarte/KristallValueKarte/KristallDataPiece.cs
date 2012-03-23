using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace KristallValueKarte
{
    public class KristallDataPiece
    {
        Dictionary<string, KristallDataAttribute> _attributes = new Dictionary<string, KristallDataAttribute>();
		
		private KristallDataPiece() // Only to be used when we're parsing the piece from code
		{
		}
		
		public KristallDataPiece(string name)
		{
			this._name = name;
		}
		
        public static KristallDataPiece Parse(string code)
        {
			KristallDataPiece parsed = new KristallDataPiece();
            MatchCollection attributeCodes = Regex.Matches(code, "M(N[^M]*)");
            KristallDataAttribute dataAttribute;
            foreach (Match attribute in attributeCodes)
            {
                dataAttribute = KristallDataAttribute.Parse(attribute.Value);
                parsed._attributes.Add(dataAttribute.Name, dataAttribute);
            }

            code = Regex.Replace(code, "M(.*)M", "");
            string nameEncoded = Regex.Match(code, "N([0-9ABCDEF]*)").Value.Substring(1);
            parsed._name = KristallDataFile.Decode(KristallDataType.String, nameEncoded) as string;
            Match valueMatch = Regex.Match(code, "V(.*)");
            parsed._type = (KristallDataType)int.Parse(valueMatch.Value.Substring(1, 4), System.Globalization.NumberStyles.HexNumber);
            parsed._value = KristallDataFile.Decode(parsed._type, valueMatch.Value.Substring(5));
			
			return parsed;
        }
		
		public static string Encode(KristallDataPiece piece)
		{
			StringBuilder stringFactory = new StringBuilder();
			stringFactory.Append("KN");
			stringFactory.Append(KristallDataFile.Encode(KristallDataType.String, piece._name));
			foreach (KristallDataAttribute attribute in piece._attributes.Values)
			{
				stringFactory.Append(KristallDataAttribute.Encode(attribute));
			}
			if (piece._attributes.Count > 0)
				stringFactory.Append("M");
			stringFactory.Append("V");
			stringFactory.Append(String.Format("{0:x4}", (int)piece._type).ToUpper());
			stringFactory.Append(KristallDataFile.Encode(piece._type, piece._value));
			return stringFactory.ToString();
		}

        private KristallDataType _type;
        private string _name;
        private object _value;

        public KristallDataType Type { get { return _type; } }
        public string Name { get { return _name; } }
        public object Value { get { return _value; } }
	
		public void SetValue(KristallDataType dataType, object value)
		{
			_type = dataType;
			_value = value;
		}
		
		public void AddAttribute(KristallDataAttribute attribute)
		{
			if (_attributes.ContainsKey(attribute.Name))
				throw new ArgumentException("This piece already contains an attribute of the specified name.");
			_attributes.Add(attribute.Name, attribute);
		}
		
        public KristallDataAttribute Attribute(string name)
        {
            if (_attributes.ContainsKey(name))
                return _attributes[name];
            return null;
        }
    }
}
