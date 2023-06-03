using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
namespace Methods
{
	public class ProgressbarMethods : MonoBehaviour
	{
		private MainControl mainCtrl;
		
		private Image singleBarPbFillImg;
		private Image singleBeatPbImg;
		private Image singleBeatPbFillImg;
		private Image singleBeatPbSubFillImg;
		private Image playbackPbFillImg;

		private void Awake()
		{
			singleBarPbFillImg = GameObject.Find("SingleBarPb").transform.GetChild(0).GetComponent<Image>();
			singleBeatPbImg = GameObject.Find("SingleBeatPb").GetComponent<Image>();
			singleBeatPbFillImg = singleBeatPbImg.transform.GetChild(0).GetComponent<Image>();
			singleBeatPbSubFillImg = singleBeatPbImg.transform.GetChild(1).GetComponent<Image>();
			
			
			playbackPbFillImg = GameObject.Find("PlaybackPb").transform.GetChild(0).GetComponent<Image>();
		}
		
		private void OnEnable()
		{
			InitProgressBars();
		}
		
		private void Update()
		{
			UpdateProgressBars();
		}

		public void OnPlaybackFinished()
		{
			UnityMainThread.Wkr.AddJob(InitProgressBars);
		}

		private void InitProgressBars()
		{
			/*
			PlaybackPb.Maximum        = MidiPlaybackInfo.MetricDur.TotalSeconds;
			SeekbarSlider.Maximum     = MidiPlaybackInfo.MetricDur.TotalSeconds;
			SeekbarTickSlider.Maximum = MidiPlaybackInfo.MetricDur.TotalSeconds;
		
			SeekbarTickSlider.TickFrequency = 60;
		*/
			
			isSingleBarPbClockwise = true;
			isSingleBeatPbClockwise = true;
			prevIntBeats = 0;
			
			singleBarPbFillImg.fillAmount  = 0;
			singleBeatPbFillImg.fillAmount = 0;
			playbackPbFillImg.fillAmount   = 0;
		}
		
		
		private bool isSingleBarPbClockwise;
		private bool isSingleBeatPbClockwise;
		private int prevIntBeats;
		private void UpdateProgressBars()
		{
			var curNumerator = MidiInfo.CurTimeSignature.Numerator;
			var curBeats     = MidiInfo.BarBeatFracTime.Beats;
			var curBars = MidiInfo.BarBeatFracTime.Bars;
		
			// SeekbarSlider.Value = MidiPlaybackInfo.MetricRealTime.TotalSeconds;

			playbackPbFillImg.fillAmount    = (float)(MidiInfo.MetricRealTime.TotalSeconds / MidiInfo.MetricDur.TotalSeconds);
			
			
			
			
			singleBarPbFillImg.fillAmount   = (float)(curBeats / curNumerator);
			singleBeatPbFillImg.fillAmount  = (float)(curBeats % 1 / 1.0);
			singleBeatPbSubFillImg.fillAmount = 1f - (float)(curBeats % 1 / 1.0);
			
			var curBeatsInt = (int)Math.Floor(curBeats);
			singleBeatPbSubFillImg.SetAlpha(curBeatsInt / (float)curNumerator);
			singleBeatPbFillImg.SetAlpha((curBeatsInt + 1) / (float)curNumerator);
			
			
			
			/*
			isSingleBarPbClockwise = curBars % 2 == 0;
			if((int)curBeats != prevIntBeats) {
				isSingleBeatPbClockwise = !isSingleBeatPbClockwise;
				prevIntBeats = (int)curBeats;
			}
			
			singleBarPbFillImg.fillClockwise = isSingleBarPbClockwise;
			singleBeatPbFillImg.fillClockwise = isSingleBeatPbClockwise;

			var singleBarFillAmt = (float)(curBeats / curNumerator);
			var singleBeatFillAmt = (float)(curBeats % 1 / 1.0);
			singleBarPbFillImg.fillAmount = isSingleBarPbClockwise ? singleBarFillAmt : 1f - singleBarFillAmt;
			singleBeatPbFillImg.fillAmount = isSingleBeatPbClockwise ? singleBeatFillAmt : 1f - singleBeatFillAmt;
			*/
			
			// singleBarPbFillImg.fillAmount   = (float)(curBeats / curNumerator);
			// singleBeatPbFillImg.fillAmount  = (float)(curBeats % 1 / 1.0);
		}
		
	}
}