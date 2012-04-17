using System;

namespace KristallValueKarte
{
	public interface IKristallDataType
	{
		string EncodeToKVK();
		IKristallDataType DecodeFromKVK(string valueCode);
	}
}

