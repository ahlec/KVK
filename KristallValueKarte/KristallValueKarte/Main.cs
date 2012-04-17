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
			KristallDataFile dataFile = new KristallDataFile();
			dataFile.AddPiece(new KristallDataPiece("full name", "Jacob Deitloff"));
			dataFile.AddPiece(new KristallDataPiece("age", 19));
			dataFile.AddPiece(new KristallDataPiece("is male", true));
			dataFile.Save("hello world.kvk");
			
			KristallDataFile parsed = new KristallDataFile("hello world.kvk");
			parsed.ToString();
			
			/*if (args.Length == 0) 
			{
				Console.WriteLine ("Usage: kvk [options] (file) (datapiece) [value]");
				Console.WriteLine ("Use -h, or --help for help!");
			} 
			else 
			{
				foreach (string arg in args) 
				{
					Console.WriteLine (arg);
					if (arg == "-h")
					{
					}
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
			}*/
		}
	}
}
