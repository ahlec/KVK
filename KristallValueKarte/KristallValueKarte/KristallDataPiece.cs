using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace KristallValueKarte
{
    public class KristallDataPiece
    {
        Dictionary<string, KristallDataAttribute> _attributes = new Dictionary<string, KristallDataAttribute>();
        public KristallDataPiece(string code)
        {
            MatchCollection attributeCodes = Regex.Matches(code, "M(N[^M]*)");
            KristallDataAttribute dataAttribute;
            foreach (Match attribute in attributeCodes)
            {
                dataAttribute = new KristallDataAttribute(attribute.Value);
                _attributes.Add(dataAttribute.Name, dataAttribute);
            }

            code = Regex.Replace(code, "M(.*)M", "");
            string nameEncoded = Regex.Match(code, "N([0-9ABCDEF]*)").Value.Substring(1);
            _name = KristallDataFile.Decode(KristallDataType.String, nameEncoded) as string;
            Match valueMatch = Regex.Match(code, "V(.*)");
            _type = (KristallDataType)int.Parse(valueMatch.Value.Substring(1, 4), System.Globalization.NumberStyles.HexNumber);
            _value = KristallDataFile.Decode(_type, valueMatch.Value.Substring(5));
        }

        private KristallDataType _type;
        private string _name;
        private object _value;

        public KristallDataType Type { get { return _type; } }
        public string Name { get { return _name; } }
        public object Value { get { return _value; } }

        public KristallDataAttribute Attribute(string name)
        {
            if (_attributes.ContainsKey(name))
                return _attributes[name];
            return null;
        }
    }
}
