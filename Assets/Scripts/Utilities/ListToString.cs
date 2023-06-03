using System.Collections.Generic;
using System.Text;

namespace Utilities
{
	public static class ListToString
	{
		public static string BoolListToString(List<bool> boolList)
		{
			var builder = new StringBuilder();
			foreach(var bVal in boolList) {
				var strVal = bVal ? "1" : "0";
				builder.Append(" " + strVal + " ");
			}
			return builder.ToString();
		}
		
		public static string IntListToString(List<int> intList)
		{
			var builder = new StringBuilder();
			foreach(var iVal in intList) {
				builder.Append(" " + iVal + " ");
			}
			return builder.ToString();
		}
		
	}
}