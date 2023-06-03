using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEngine;
using Utilities;
namespace Methods
{
	public class BpmTimeSignatureTextMethods : MonoBehaviour, IMethodsClass
	{
		private TMP_Text timeSigNumeratorText;
		private TMP_Text timeSigFracLineText;
		private TMP_Text timeSigDenominatorText;
		private TMP_Text timeSigNumeratorUnderBeatText;


		private TMP_Text plainBpmText;
		private TMP_Text curBpmText;
		
		[field: SerializeField] public bool IsUpdateEnabled { get; set; }

		public void Awake()
		{
			timeSigNumeratorText          = GameObject.Find("TimeSigNumeratorText").GetComponent<TMP_Text>();
			timeSigFracLineText           = GameObject.Find("TimeSigFracLineText").GetComponent<TMP_Text>();
			timeSigDenominatorText        = GameObject.Find("TimeSigDenominatorText").GetComponent<TMP_Text>();
			timeSigNumeratorUnderBeatText = GameObject.Find("TimeSigNumeratorUnderBeatText").GetComponent<TMP_Text>();

			plainBpmText = GameObject.Find("PlainBpmText").GetComponent<TMP_Text>();
			curBpmText   = GameObject.Find("CurBpmText").GetComponent<TMP_Text>();
		}

		public void OnMidiFileLoad()
		{
			
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			StartCoroutine(SetTimeSignatureText());
			StartCoroutine(SetBpmText());
		}
		public void OnPlaybackPaused()
		{
			StopCoroutine(SetTimeSignatureText());
			StopCoroutine(SetBpmText());
		}
		public void OnPlaybackStopped()
		{
			OnPlaybackPaused();
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			
		}

		private IEnumerator SetTimeSignatureText()
		{
			var prevTimeSig = MidiInfo.CurTimeSignature;
			var waitForChange = new WaitWhile(() => prevTimeSig == MidiInfo.CurTimeSignature);

			while(true) {
				yield return waitForChange;
				
				// timeSigNumeratorText.SetText(MidiInfo.CurTimeSignature.Numerator.ToString());
				// timeSigDenominatorText.SetText(MidiInfo.CurTimeSignature.Denominator.ToString());
				// timeSigNumeratorUnderBeatText.text = timeSigNumeratorText.text;
				StartCoroutine(timeSigNumeratorText.NumChangeAnim(MidiInfo.CurTimeSignature.Numerator, isValueInteger: true, easeType: EasingFunction.Ease.EaseOutExpo));
				StartCoroutine(timeSigDenominatorText.NumChangeAnim(MidiInfo.CurTimeSignature.Denominator, isValueInteger: true, easeType: EasingFunction.Ease.EaseOutExpo));
				StartCoroutine(timeSigNumeratorUnderBeatText.NumChangeAnim(MidiInfo.CurTimeSignature.Numerator, isValueInteger: true, easeType: EasingFunction.Ease.EaseOutExpo));
				prevTimeSig = MidiInfo.CurTimeSignature;
			}
		}

		private IEnumerator SetBpmText()
		{
			var prevBpm = MidiInfo.CurBpm;
			var waitForChange = new WaitWhile(() => prevBpm == MidiInfo.CurBpm);

			while(true) {
				yield return waitForChange;
				
				// curBpmText.SetText(MidiInfo.CurBpm.ToString(CultureInfo.InvariantCulture));
				StartCoroutine(curBpmText.NumChangeAnim((float)MidiInfo.CurBpm, easeType: EasingFunction.Ease.EaseOutExpo));
				prevBpm = MidiInfo.CurBpm;
			}
		}
	}
}