using System;
using System.Text.RegularExpressions;

namespace KristallValueKarte
{
    public class KristallDataAttribute
    {
        KristallDataType _type;
        object _value;
        string _name;

        public KristallDataAttribute(string code)
        {
            _name = KristallDataFile.Decode(KristallDataType.String,
                Regex.Match(code, "N([0-9ABCDEF]*)").Value.Substring(1)) as string;
            Match valueMatch = Regex.Match(code, "V(.*)");
            _type = (KristallDataType)int.Parse(valueMatch.Value.Substring(1, 4), System.Globalization.NumberStyles.HexNumber);
            _value = KristallDataFile.Decode(_type, valueMatch.Value.Substring(5));
        }

        public KristallDataType Type { get { return _type; } }
        public object Value { get { return _value; } }
        public string Name { get { return _name; } }
    }
}
