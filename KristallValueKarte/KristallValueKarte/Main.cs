using System;
using System.IO;

namespace KristallValueKarte
{
	class MainClass
	{
		private static bool cli_help()
		{
			try
			{
				
				TextReader help_reader = new StreamReader(".help.txt");
				string line;
				while ((line = help_reader.ReadLine()) != null)
				{
					Console.WriteLine(line);
				}
				help_reader.Close();
			}
			catch (FileNotFoundException e)
			{
				 Console.WriteLine (".help.txt does not exist! No help available.", e);
			}
			catch (Exception e)
			{
				Console.WriteLine("{0} Exception caught.", e);
			}
			return false;
		}
		public static void Main (string[] args)
		{
			if (args.Length == 0) 
			{
				Console.WriteLine ("Usage: kvk [options] (file) (datapiece) [value]");
				Console.WriteLine ("Use -h, or --help for help!");
//				KristallDataFile dataFile = new KristallDataFile ();
//				KristallDataPiece piece = new KristallDataPiece ("hello world");
//				piece.SetValue (KristallDataType.String, "Jacob Deitloff");
//				dataFile.AddPiece (piece);
//			
//				dataFile.Save ("hello world.kvk");
			} 
			else 
			{
				foreach (string arg in args) 
				{
					Console.WriteLine (arg);
					if (arg == "-h")
				}
				switch (arg)
				{
					case ("-h"):
					{
						cli_help ();
						break;
					}
					case ("--example_file"):
					{
						KristallDataFile dataFile = new KristallDataFile ();
						KristallDataPiece piece = new KristallDataPiece ("hello world");
						piece.SetValue (KristallDataType.String, "Jacob Deitloff, Kevin Chau");
						dataFile.AddPiece (piece);
			
						dataFile.Save ("hello world.kvk");
						break;
					}
				}
			}
		}
	}
}
