using System;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEngine;
namespace Methods
{
	public class TimeSpanTextMethods : MonoBehaviour, IMethodsClass
	{
		private long totalBars;

		private TMP_Text metricTimeText;
		private TMP_Text metricTimeSlashText;
		private TMP_Text metricTimeLeftText;
		private TMP_Text metricDurationText;
		private TMP_Text metricTimePercentageText;
		
		public bool IsUpdateEnabled { get; set; }

		private void Awake()
		{
			metricTimeText           = GameObject.Find("MetricTimeText").GetComponent<TMP_Text>();
			metricTimeSlashText      = GameObject.Find("MetricTimeSlashText").GetComponent<TMP_Text>();
			metricTimeLeftText       = GameObject.Find("MetricTimeLeftText").GetComponent<TMP_Text>();
			metricDurationText       = GameObject.Find("MetricDurationText").GetComponent<TMP_Text>();
			metricTimePercentageText = GameObject.Find("MetricTimePercentageText").GetComponent<TMP_Text>();
		}
		public void OnMidiFileLoad()
		{
			metricDurationText.SetText(MidiUtil.FormatMetricTime(MidiInfo.MetricDur));
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			
		}

		private void Update()
		{
			if(!IsUpdateEnabled) return;
			
			UpdateTexts();
		}
		
		public void OnPlaybackPaused()
		{
			
		}
		public void OnPlaybackStopped()
		{
			UpdateTexts();
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			
		}
		
		private void UpdateTexts()
		{
			var curMetricTime        = MidiInfo.MetricRealTime;
			var metricDuration       = MidiInfo.MetricDur;
			var metricTimeLeft       = metricDuration - curMetricTime;
			var metricTimePercentage = curMetricTime.TotalMilliseconds / metricDuration.TotalMilliseconds;

			metricTimeText.SetText(MidiUtil.FormatMetricTime(MidiInfo.MetricRealTime));
			metricTimeLeftText.SetText(MidiUtil.FormatMetricTime(metricTimeLeft));
			metricTimePercentageText.SetText($"{metricTimePercentage:P2}");
		}
	}
}