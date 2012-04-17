using System;
using System.Text;
using System.Text.RegularExpressions;

namespace KristallValueKarte
{
    public class KristallDataAttribute
    {
        KristallDataType _type;
        object _value;
        string _name;
		
		private KristallDataAttribute() // used only for interpretting from within the code
		{
		}
		
		public KristallDataAttribute(string name)
		{
			this._name = _name;
		}
		
        public static KristallDataAttribute Parse(string code)
        {
			KristallDataAttribute parsed = new KristallDataAttribute();
            parsed._name = KristallDataFile.Decode(KristallDataType.String,
                Regex.Match(code, "N([0-9ABCDEF]*)").Value.Substring(1)) as string;
            Match valueMatch = Regex.Match(code, "V(.*)");
            parsed._type = (KristallDataType)int.Parse(valueMatch.Value.Substring(1, 4), System.Globalization.NumberStyles.HexNumber);
            parsed._value = KristallDataFile.Decode(parsed._type, valueMatch.Value.Substring(5));
			return parsed;
        }
		
		public static string Encode(KristallDataAttribute dataAttribute)
		{
			StringBuilder stringFactory = new StringBuilder();
			stringFactory.Append("MN");
			stringFactory.Append(KristallDataFile.Encode(KristallDataType.String, dataAttribute._name));
			stringFactory.Append("V");
			stringFactory.Append(String.Format("{0:x4}", (int)dataAttribute._type).ToUpper());
			stringFactory.Append(KristallDataFile.Encode(dataAttribute._type, dataAttribute._value));
			return stringFactory.ToString();
		}

        public KristallDataType Type { get { return _type; } }
        public object Value { get { return _value; } }
        public string Name { get { return _name; } }
    }
}
