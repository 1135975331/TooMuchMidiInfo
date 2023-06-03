
using System;
using UnityEngine;

namespace Utilities
{
	public static class RandomInRange
	{
		private static System.Random Random;
		
		/*
		/// <summary>
		/// 일정 범위 내 난수값을 반환하는 함수
		/// </summary>
		/// <param name="min">최소값(inclusive)</param>
		/// <param name="max">최대값(exclusive)</param>
		/// <returns>float형 난수값</returns>
		public static float RandomFloatInRange(float min, float max)
		{
			Random ??= new System.Random();
			return (float)(Random.NextDouble() * (max - min) + min);
		}*/
		
		/// <summary>
		/// 일정 범위 내 double 난수값을 반환하는 함수
		/// 다른 타입이 필요하다면 타입 캐스팅을 사용할 것
		/// </summary>
		/// <param name="min">최소값(inclusive)</param>
		/// <param name="max">최대값(exclusive)</param>
		/// <returns>double형 난수값</returns>
		public static double RandomDoubleInRange(double min, double max)
		{
			Random ??= new System.Random();
			return Random.NextDouble() * (max - min) + min;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="min"></param>
		/// <param name="offsetMin"></param>
		/// <param name="offsetMax"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static double RandomDoubleInRangeWithOffset(double min, double offsetMin, double offsetMax, double max)
		{
			double value;

			do { value = RandomDoubleInRange(min, max); }
			while(offsetMin < value && value < offsetMax); //offsetMin < value < offsetMax 인 경우 다시뽑는다
			
			return value;
		}
	}
}