using System;

namespace KristallValueKarte
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			KristallDataFile dataFile = new KristallDataFile();
			KristallDataPiece piece = new KristallDataPiece("hello world");
			piece.SetValue(KristallDataType.String, "Jacob Deitloff");
			dataFile.AddPiece(piece);
			
			dataFile.Save("hello world.kvk");
		}
	}
}
