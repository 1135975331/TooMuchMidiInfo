using System;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using UnityEngine;
using static Methods.MidiInfo;
using static Methods.Settings;
namespace Methods.MidiInfoMethodsClasses
{
	public class MidiInfoTimeSpanMethods : MonoBehaviour, IMethodsClass
	{

		public bool IsUpdateEnabled { get; set; }
		public void OnMidiFileLoad()
		{
			InitDurations();
			InitCurTimes();
			GetMidiTimePerQuarterNote();
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			
		}

		private void Update()
		{
			if(!IsUpdateEnabled) return;
			
			UpdatePlaybackTime();
		}
		
		public void OnPlaybackPaused()
		{
			
		}
		public void OnPlaybackStopped()
		{
			UpdatePlaybackTime();
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			
		}
		
		
		private static void InitDurations()
		{
			MetricDur = CurPlayback.GetDuration<MetricTimeSpan>();
			MidiDur   = CurPlayback.GetDuration<MidiTimeSpan>();
			// MusicalDur      = CurPlayback.GetDuration<MusicalTimeSpan>();
			BarBeatFracDur = CurPlayback.GetDuration<BarBeatFractionTimeSpan>();
			// BarBeatTicksDur = CurPlayback.GetDuration<BarBeatTicksTimeSpan>();
		}

		private static void InitCurTimes()
		{
			MetricRealTime = CurPlayback.GetCurrentTime<MetricTimeSpan>();
			MidiTime = TimeConverter.ConvertTo<MidiTimeSpan>(MetricRealTime, CurTempoMap);
			BarBeatFracTime = TimeConverter.ConvertTo<BarBeatFractionTimeSpan>(MetricRealTime, CurTempoMap);
		}
		
		private static void UpdatePlaybackTime()  
		{
			MetricRealTime = CurPlayback.GetCurrentTime<MetricTimeSpan>();
			var displayTime = MetricRealTime.TotalMilliseconds - UiOffsetTimeSpan.TotalMilliseconds >= 0 ?  MetricRealTime - UiOffsetTimeSpan : new MetricTimeSpan(0);
			MidiTime        = TimeConverter.ConvertTo<MidiTimeSpan>(displayTime, CurTempoMap);
			BarBeatFracTime = TimeConverter.ConvertTo<BarBeatFractionTimeSpan>(displayTime, CurTempoMap);
			
			MetricTimeLeft = MetricDur - MetricRealTime;
			MetricTimePercentage = (float)(MetricRealTime.TotalMilliseconds / MetricDur.TotalMilliseconds);
		}


		public static void GetMidiTimePerQuarterNote()
		{
			var array              = CurMidiFile.GetNotes().ToArray();
			var CurTempoMap           = CurMidiFile.GetTempoMap();
			var quarterLengthFound = false;
		
			for(var searchCount = 1; !quarterLengthFound; searchCount++) {
				var targetBeat        = Math.Pow(2, searchCount-1);
				var targetBarBeatFrac = new BarBeatFractionTimeSpan(0, targetBeat);
          
				foreach(var note in array) {
					if(note.LengthAs<BarBeatFractionTimeSpan>(CurTempoMap) != targetBarBeatFrac) continue;
					
					var midiTimeOnTarBeat     = note.LengthAs<MidiTimeSpan>(CurTempoMap);
					var curTimeSigDenominator = CurTempoMap.GetTimeSignatureAtTime(note.TimeAs<MidiTimeSpan>(CurTempoMap)).Denominator;
						
					MidiTimePerQuarterNote = (int)(midiTimeOnTarBeat / targetBeat) * (curTimeSigDenominator / 4);
					quarterLengthFound     = true;
					break;
				}
			
				if(targetBeat >= 32) {
					MidiTimePerQuarterNote = 480;
					break;
				}
			}
		}
	}
}