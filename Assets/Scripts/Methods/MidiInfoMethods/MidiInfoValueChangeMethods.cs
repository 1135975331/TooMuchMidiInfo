using System;
using System.Linq;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using static Methods.MidiInfo;
namespace Methods.MidiInfoMethodsClasses
{
	public class MidiInfoValueChangeMethods : MonoBehaviour, IMethodsClass
	{
		private static int _timeSigChangeIndex = 0;
		private static int _tempoChangeIndex = 0;
		
		public bool IsUpdateEnabled { get; set; }
		public void OnMidiFileLoad()
		{
			InitChangeEnumerables();
		}

		private void Update()
		{
			if(!IsUpdateEnabled)  return;
			
			UpdateTempoMapInfo();
		}
		
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			
		}
		public void OnPlaybackPaused()
		{
			
		}
		public void OnPlaybackStopped()
		{
			InitChangeEnumerables();
			CorrectChangeIndex();
			
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			CorrectChangeIndex();
		}
		
		private static void InitChangeEnumerables()
		{
			_timeSigChangeIndex = 0;
			_tempoChangeIndex   = 0;

			TimeSigChanges = CurTempoMap.GetTimeSignatureChanges().ToArray();
			TempoChanges   = CurTempoMap.GetTempoChanges().ToArray();

			if(TimeSigChanges.Length != 0)
				NextTimeSig = TimeSigChanges[_timeSigChangeIndex];
			if(TempoChanges.Length != 0)
				NextTempo = TempoChanges[_tempoChangeIndex];
		}
		
		private static void UpdateTempoMapInfo() 
		{
			if(TimeSigChanges.Length != 0 && MidiTime.TimeSpan >= NextTimeSig.Time) {
				CurTimeSignature = NextTimeSig.Value;
			
				if(_timeSigChangeIndex + 1 != TimeSigChanges.Length)
					NextTimeSig = TimeSigChanges[++_timeSigChangeIndex];
			}
			if(TempoChanges.Length != 0 && MidiTime.TimeSpan >= NextTempo.Time) {
				CurBpm = NextTempo.Value.BeatsPerMinute;
			
				if(_tempoChangeIndex + 1 != TempoChanges.Length)
					NextTempo = TempoChanges[++_tempoChangeIndex];
			}	
		}
		
		
		public static void CorrectChangeIndex()
		{
			CorrectChangeIndex(TimeSigChanges, out var nextTimeSigChange, ref _timeSigChangeIndex);
			CorrectChangeIndex(TempoChanges, out var nextTempoChange, ref _tempoChangeIndex);
			NextTimeSig = nextTimeSigChange;
			NextTempo = nextTempoChange;
		}

		private static void CorrectChangeIndex<TValue>(ValueChange<TValue>[] valueChange, out ValueChange<TValue> nextChange, ref int valueChangeIndex)
		{
			for(var i = 0; i < valueChange.Length; i++) {
				if(valueChange[i].Time < MidiTime.TimeSpan)
					continue;
				
				valueChangeIndex = i;
				nextChange = valueChange[i];
				break;
			}
			
			nextChange = valueChange[^1];
		}
		
	}
}