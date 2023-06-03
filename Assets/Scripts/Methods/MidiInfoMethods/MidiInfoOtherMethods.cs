using System;
using System.Linq;
using DefaultNamespace;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using static Methods.MidiInfo;
using static Methods.Settings;

namespace Methods.MidiInfoMethodsClasses
{
	public class MidiInfoOtherMethods : MonoBehaviour, IMethodsClass
	{
		private void Start()
		{
			// Debug.Log("MidiPlaybackInfoMethods Start Executed");
		}



		public bool IsUpdateEnabled { get; set; }
		
		public void OnMidiFileLoad()
		{
			// Debug.Log("MidiPlaybackInfoMethods OnEnable Executed");
			/*
			InitDurations(CurPlayback);
			InitTempoMap(CurPlayback);
			InitCurTimes(CurPlayback, CurTempoMap);
			InitBpmTimeSignature(CurTempoMap);
			GetMidiTimePerQuarterNote(CurMidiFile);
			*/
			/*InitDurations(CurPlayback);
			InitTempoMap(CurPlayback);
			InitCurTimes(CurPlayback, CurTempoMap);
			InitChangeEnumerables(CurTempoMap);
			InitBpmTimeSignature(CurTempoMap);
			GetMidiTimePerQuarterNote(CurMidiFile);
			InitUiOffsetMs();	*/
			
			LogMidiPlaybackInfo();
		}
		
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			
		}
		public void OnPlaybackPaused()
		{
			
		}
		public void OnPlaybackStopped()
		{
			
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			
		}
		public void OnPlaybackSeek()
		{
			
		}
		
		
		private static string GetLogMidiPlaybackInfo()
		{
			return "The midi file has been Loaded." + 
				$"CurMidiFile: {CurMidiFile}\n" +
				$"CurPlayback: {CurPlayback}\n" +
				$"\n" +
				// $"MetricRealTime: {MetricRealTime}\n" +
				$"MetricDur: {MetricDur}\n" +
				$"\n" +
				$"\n" +
				$"CurTempoMap: {CurTempoMap}\n" +
				$"\n" +
				$"CurTimeSig: {CurTimeSignature}\n" +
				$"NextTimeSig: {NextTimeSig}\n" +
				$"TimeSigChanges: {TimeSigChanges}\n" +
				$"\n" +
				$"CurBpm: {CurBpm}\n" +
				$"NextTempo: {NextTempo}\n" +
				$"TempoChanges: {TempoChanges}\n" +
				$"\n" +
				$"MidiTimePerQuarter: {MidiTimePerQuarterNote}\n";
		}

		private static void LogMidiPlaybackInfo()
			=> PublicTextComponents.DebugConsole.SetText(GetLogMidiPlaybackInfo());
	}
}