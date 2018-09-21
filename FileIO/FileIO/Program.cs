using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace FileIO
{
	class Rogue
	{
		private int hp;
		private string name;
		private int damage;

		public override string ToString()
		{
			return this.name + "," + this.hp + "," + this.damage;
		}

		public Rogue(string name, int hp, int damage)
		{
			this.hp = hp;
			this.damage = damage;
			this.name = name;
		}
	}

	[Serializable]
	class RogueSerializable
	{
		private int hp;
		private string name;
		private int damage;

		public override string ToString()
		{
			return this.name + "," + this.hp + "," + this.damage;
		}

		public RogueSerializable(string name, int hp, int damage)
		{
			this.hp = hp;
			this.damage = damage;
			this.name = name;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			if (!Directory.Exists("saves"))
			{
				Directory.CreateDirectory("saves");
				Console.WriteLine("Created the directory");		
			}
			if (!File.Exists("saves/save.game"))
			{
				File.Create("saves/save.game").Close();
				Console.WriteLine("Created the file");
			}
			FileStream saveFile = new FileStream("saves/save.game", FileMode.Open, FileAccess.ReadWrite);
			StreamReader reader = new StreamReader(saveFile);
			StreamWriter writer = new StreamWriter(saveFile);
			if (saveFile.Length != 0)
			{
				//Read the data, then append more data
				string line = reader.ReadLine();
				while (line != null)
				{
					string[] rogue = line.Split(',');
					Rogue newRogue = new Rogue(rogue[0], Convert.ToInt32(rogue[1]), Convert.ToInt32(rogue[2]));
					Console.WriteLine(newRogue.ToString());
					line = reader.ReadLine();
				}
				Console.WriteLine("Data read");
				//Now line is null, we're at end of file, let's write
				Rogue[] rogueArr = { new Rogue("Tyrion", 600, 10), new Rogue("Jon", 100, 0), new Rogue("Sam", 2000, 3) };
				for (int k = 0; k < rogueArr.Length; k++)
				{
					writer.WriteLine(rogueArr[k].ToString());
				}
				Console.WriteLine("Data written");
				writer.Close();
				saveFile.Close();
			}
			else
			{
				//Just write some data
				Rogue[] rogueArr = {new Rogue("Vincent", 750, 7), new Rogue("Gregor",1000, 15), new Rogue("Elena",500, 9)};
				for (int k = 0; k < rogueArr.Length; k++)
				{
					writer.WriteLine(rogueArr[k].ToString());
				}
				Console.WriteLine("Data written");
				writer.Close();
				saveFile.Close();
			}

			FileStream serializedFile = new FileStream("saves/serialsave.game", FileMode.OpenOrCreate, FileAccess.ReadWrite);
			BinaryFormatter bFormat = new BinaryFormatter();
			if (serializedFile.Length != 0)
			{
				//Read the data, then append more data
				while (serializedFile.Position < serializedFile.Length)
				{
					RogueSerializable temp = (RogueSerializable)bFormat.Deserialize(serializedFile);
					Console.WriteLine(temp.ToString());
				}
				Console.WriteLine("Data read");
				serializedFile.Close();
			}
			else
			{
				//Just write some data
				RogueSerializable[] rogueArr = { new RogueSerializable("Vincent", 750, 7), new RogueSerializable("Gregor", 1000, 15), new RogueSerializable("Elena", 500, 9) };
				for (int k = 0; k < rogueArr.Length; k++)
				{
					bFormat.Serialize(serializedFile, rogueArr[k]);
				}
				Console.WriteLine("Data written");
				serializedFile.Close();
			}

		}
	}
}
