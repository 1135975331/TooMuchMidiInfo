using System;
using System.Globalization;
using Melanchall.DryWetMidi.Interaction;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEngine;
using Utilities;
namespace Methods
{
	public class ValueChangeAlertTextMethods : MonoBehaviour, IMethodsClass
	{
		private TMP_Text[] nextTimeSigTexts;
		private TMP_Text nextTimeSigNumeratorText, nextTimeSigFracLineText, nextTimeSigDenominatorText, nextTimeSigArrowText, nextTimeSigBeatLeftText;
		
		private TMP_Text[] nextBpmTexts;
		private TMP_Text nextBpmArrowText, nextBpmBeatLeftText, nextBpmText;
		
		public bool IsUpdateEnabled { get; set; }


		public void Awake()
		{
			// nextTimeSigTexts         = GameObject.Find("NextTimeSigTexts").GetComponentsInChildren<TMP_Text>();
			// nextBpmTexts = GameObject.Find("NextBpmTexts").GetComponentsInChildren<TMP_Text>();

			nextTimeSigNumeratorText   = GameObject.Find("NextTimeSigNumeratorText").GetComponent<TMP_Text>();
			nextTimeSigFracLineText    = GameObject.Find("NextTimeSigFracLineText").GetComponent<TMP_Text>();
			nextTimeSigDenominatorText = GameObject.Find("NextTimeSigDenominatorText").GetComponent<TMP_Text>();
			nextTimeSigArrowText       = GameObject.Find("NextTimeSigArrowText").GetComponent<TMP_Text>();
			nextTimeSigBeatLeftText    = GameObject.Find("NextTimeSigBeatLeftText").GetComponent<TMP_Text>();
			
			nextBpmArrowText    = GameObject.Find("NextBpmArrowText").GetComponent<TMP_Text>();
			nextBpmBeatLeftText = GameObject.Find("NextBpmBeatLeftText").GetComponent<TMP_Text>();
			nextBpmText         = GameObject.Find("NextBpmText").GetComponent<TMP_Text>();
			
			nextTimeSigTexts = new[] { nextTimeSigNumeratorText, nextTimeSigFracLineText, nextTimeSigDenominatorText, nextTimeSigArrowText, nextTimeSigBeatLeftText };
			nextBpmTexts = new[] { nextBpmArrowText, nextBpmBeatLeftText, nextBpmText };
		}
		
		public void OnMidiFileLoad()
		{
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
		}

		private void Update()
		{
			if(!IsUpdateEnabled)  return;
			
			UpdateNextTimeSigText(MidiInfo.NextTimeSig);
			UpdateNextBpmText(MidiInfo.NextTempo);
		}

		private bool fadeInTimeSig;
		private bool fadeOutTimeSig;
		private bool fadeInBpm;
		private bool fadeOutBpm; 
		private const int BEATS_LEFT_TO_START_ALERT = 12;
		private void UpdateNextTimeSigText(ValueChange<TimeSignature> nextTimeSigChange)
		{
			var beatsLeftToChange      = MidiUtil.MidiTimeToBeatsInt((int) (nextTimeSigChange.Time - MidiInfo.MidiTime.TimeSpan));
			var changeAlertDisplayCond = beatsLeftToChange is >= 0 and <= BEATS_LEFT_TO_START_ALERT;
		
			if(changeAlertDisplayCond) {
				nextTimeSigNumeratorText.text = nextTimeSigChange.Value.Numerator.ToString();
				nextTimeSigDenominatorText.text = nextTimeSigChange.Value.Denominator.ToString();
				nextTimeSigBeatLeftText.text    = beatsLeftToChange.ToString();
			}
			else 
				nextTimeSigBeatLeftText.text = "0";
		
			TMPTextUtil.ApplyFadeEffect(nextTimeSigTexts, ref fadeInTimeSig, ref fadeOutTimeSig, changeAlertDisplayCond);
		}


		private void UpdateNextBpmText(ValueChange<Tempo> nextBpmChange)
		{
			var beatsLeftToChange      = MidiUtil.MidiTimeToBeatsInt((int) (nextBpmChange.Time - MidiInfo.MidiTime.TimeSpan));
			var changeAlertDisplayCond = beatsLeftToChange is >= 0 and <= BEATS_LEFT_TO_START_ALERT;
		
			if(changeAlertDisplayCond) {
				nextBpmText.text         = nextBpmChange.Value.BeatsPerMinute.ToString(CultureInfo.CurrentCulture);
				nextBpmBeatLeftText.text = beatsLeftToChange.ToString();
			}
			else
				nextBpmBeatLeftText.text = "0";
		
			TMPTextUtil.ApplyFadeEffect(nextBpmTexts, ref fadeInBpm, ref fadeOutBpm, changeAlertDisplayCond);
		}

		public void OnPlaybackPaused()
		{
		}
		public void OnPlaybackStopped()
		{
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			fadeInTimeSig  = false;
			fadeOutTimeSig = false;
			fadeInBpm      = false;
			fadeOutBpm     = false;
			
			UnityMainThread.Wkr.AddJob(() => {
				foreach(var nextTimeSigText in nextTimeSigTexts) 
					nextTimeSigText.CrossFadeAlpha(0, 1f, true);
		
				foreach(var nextBpmText in nextBpmTexts)
					nextBpmText.CrossFadeAlpha(0, 1f, true);
				
				UpdateNextBpmText(MidiInfo.NextTempo);
				UpdateNextTimeSigText(MidiInfo.NextTimeSig);
			});
		}
		public void OnPlaybackSeek()
		{
			UpdateNextTimeSigText(MidiInfo.NextTimeSig);
			UpdateNextBpmText(MidiInfo.NextTempo);
		}
		
		
	}
}