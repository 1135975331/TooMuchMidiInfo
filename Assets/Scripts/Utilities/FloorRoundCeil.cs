using System;
using UnityEngine;

namespace Utilities
{
	public static class FloorRoundCeil
	{
		public static float FloorFrom(float value, int pos)
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return Mathf.Floor(value*powOf10) / powOf10;
		}

		public static float RoundFrom(float value, int pos)
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return Mathf.Round(value*powOf10) / powOf10;
		}

		public static float CeilFrom(float value, int pos)		
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return Mathf.Ceil(value*powOf10) / powOf10;
		}
		
		
		public static double FloorFrom(double value, int pos)
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return (double)Mathf.Floor((float)value*powOf10) / powOf10;
		}

		public static double RoundFrom(double value, int pos)
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return (double)Mathf.Round((float)value*powOf10) / powOf10;
		}

		public static double CeilFrom(double value, int pos)		
		{
			if(pos < 1)
				throw new ArgumentException("\'pos\' should be at least 1.");

			var powOf10 = Mathf.Pow(10, pos-1);

			return (double)Mathf.Ceil((float)value*powOf10) / powOf10;
		}
	}
}