using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Methods;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
namespace Utilities
{
	public static class TMPTextUtil
	{
		public static void ApplyFadeEffect(IEnumerable<TMP_Text> textBlocksArr, ref bool fadeIn, ref bool fadeOut, bool changeAlertDisplayCond)
		{
			if(changeAlertDisplayCond) {
				if(fadeIn) return;
			
				foreach(var textBlock in textBlocksArr)
					textBlock.CrossFadeAlpha(1.0f, 1.0f, true);
				// StartCoroutine(textBlock.TMPTextFadeEffect(0, 1, 1f));

				fadeIn  = true;
				fadeOut = false;
			}
			else {
				if(fadeOut) return;
			
				foreach(var textBlock in textBlocksArr)
					textBlock.CrossFadeAlpha(0f, 1.0f, true);
				// StartCoroutine(textBlock.TMPTextFadeEffect(1, 0, 1f));

				fadeOut = true;
				fadeIn  = false;
			}
		}

		public static IEnumerator NumChangeAnim(this TMP_Text tmpText, float numberTo, bool isValueInteger = false, float duration = 1f, string durationUnit = "Second", EasingFunction.Ease easeType = EasingFunction.Ease.Linear)
		{
			const int FRAMERATE       = 60;
			var waitForFrame    = new WaitForSeconds(FloorRoundCeil.FloorFrom(1f / FRAMERATE, 3));
			var easeValVariance = durationUnit switch {
				"Second" => 1f / FRAMERATE / duration,
				"Bar"    => 1f / FRAMERATE / (float)(TimeConverter.ConvertTo<MetricTimeSpan>(new BarBeatFractionTimeSpan(1), MidiInfo.CurTempoMap).TotalSeconds * duration),
				"Beat"   => 1f / FRAMERATE / (float)(TimeConverter.ConvertTo<MetricTimeSpan>(new BarBeatFractionTimeSpan(0, 1), MidiInfo.CurTempoMap).TotalSeconds * duration),
				_        => throw new ArgumentOutOfRangeException(nameof(durationUnit), durationUnit, "Invalid duration unit")
			};
			
			var easeFunc = EasingFunction.GetEasingFunction(easeType);
			var easeValue = 0f;
			var initialDisplayedText = float.Parse(tmpText.text);
			
			while(true) {
				// var easeValVariance = 1f / duration * Time.deltaTime;
				
				var validEaseValue = easeValue is >= 0f and < 1f;
				var result = validEaseValue ? easeFunc(initialDisplayedText, numberTo, easeValue) : numberTo;
				tmpText.SetText(isValueInteger ? $"{result:N0}" : $"{result:F1}");
			
				if(!validEaseValue)
					break;
				
				easeValue += easeValVariance;
				yield return waitForFrame;
			}
		}
		
		private static float GetAnimDeltaFloat(float v1, float v2)
		{
			//println("v1: $v1, v2: $v2")
			var valueDelta  = Mathf.Abs(v1 - v2);
			var deltaResult = valueDelta * 0.25f;
			//		var deltaResult = Math.sqrt(valueDelta) * velocity
		
			return deltaResult switch {
				   0.0f  => 0.0f,
				<  0.1f  => 0.1f,
				_        => deltaResult
			};
		}		
		private static int GetAnimDeltaInt(int v1, int v2)
		{
			var valueDelta = Mathf.Abs(v1 - v2);
			var velocity   = 0.25;
        
			return (valueDelta * velocity) switch {
				< 1 => 1,
				_   => (int) (valueDelta * velocity)
			};
		}
	}
}