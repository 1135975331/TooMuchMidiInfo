using System;
using System.Globalization;
using DefaultNamespace;
using Melanchall.DryWetMidi.Interaction;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
namespace Methods
{
	public class TextMainMethods : MonoBehaviour
	{
		// todo 아래의 동작들을 각각의 Text 부모들의 스크립트로 분리한다.

		private void Awake()
		{





			/*nextTimeSigTexts = new[] { 
				GameObject.Find("NextTimeSigNumeratorText").GetComponent<Text>(), 
				GameObject.Find("NextTimeSigFracLineText").GetComponent<Text>(), 
				GameObject.Find("NextTimeSigDenominatorText").GetComponent<Text>(),
				GameObject.Find("NextTimeSigBeatLeftText").GetComponent<Text>(),
				GameObject.Find("NextTimeSigArrowTex").GetComponent<Text>()t
			};
		
			nextBpmTexts = new[] {
				GameObject.Find("NextBpmText").GetComponent<Text>(),
				GameObject.Find("NextBpmBeatLeftText").GetComponent<Text>(),
				GameObject.Find("NextBpmArrowText").GetComponent<Text>()t
			};*/
		}

		public void OnEnable()
		{

		}


		private void Update() => Updates();
		private void Updates()
		{
		}

		public void OnPlaybackFinished()
		{
			UnityMainThread.Wkr.AddJob(() => {
				Updates();
			});
		}

	}
}