using System;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using UnityEngine;
using static Methods.MidiInfo;
namespace Methods.MidiInfoMethodsClasses
{
	public class MidiInfoTempoMapMethods : MonoBehaviour, IMethodsClass
	{
		public bool IsUpdateEnabled { get; set; }
		public void OnMidiFileLoad()
		{
			InitTempoMap();
			InitBpmTimeSignature();
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			
		}
		public void OnPlaybackPaused()
		{
			
		}
		public void OnPlaybackStopped()
		{
			InitBpmTimeSignature();
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			
		}
		
		private static void InitTempoMap()
		{
			CurTempoMap = CurPlayback.TempoMap;
		}
		
		private static void InitBpmTimeSignature()
		{
			CurTimeSignature = CurTempoMap.GetTimeSignatureAtTime(new MetricTimeSpan(0));
			CurBpm           = CurTempoMap.GetTempoAtTime(new MetricTimeSpan(0)).BeatsPerMinute;
		}
	}
}