using System;

namespace HtmlAnalyzer.CLI.Interactors
{
	///<summary>
	/// Contains methods for an interaction with user through console
	///</summary>
	public abstract class Interactor
	{
		public string Read() => Console.ReadLine();

		public void Print(string message, string end = "\n")
		{
			Console.Write($"{message}{end}");
		}

		///<summary>
		/// Prompts user with a given message and takes an input
		///</summary>
		///<returns>
		/// User input
		///</returns>
		public string AskForInput(string message)
		{
			Print($"{message}: ");
			return Console.ReadLine();
		}

		///<summary>
		/// Prompts user with a given message and takes a yes/no input
		///</summary>
		///<returns>
		/// User input converted to bool
		///</returns>
		public bool AskYesNo(string message)
		{
			while (true)
			{
				string ans = AskForInput($"{message} (y/n)");

				if (IsYes(ans)) return true;
				if (IsNo(ans)) return false;
			}
		}

		///<summary>
		/// Converts given 'yes' string to bool
		///</summary>
		public bool IsYes(string str)
		{
			str = str.ToLower();
			if (str == "y" || str == "yes")
				return true;

			return false;
		}

		///<summary>
		/// Converts given 'no' string to bool
		///</summary>
		public bool IsNo(string str)
		{
			str = str.ToLower();
			if (str == "n" || str == "no")
				return true;

			return false;
		}
	}
}
