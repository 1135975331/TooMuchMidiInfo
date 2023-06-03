using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using UnityEngine;
using static Methods.MidiInfo;
namespace Methods.MidiInfoMethodsClasses
{
	public class MidiInfoNoteMethods : MonoBehaviour, IMethodsClass
	{
		public bool IsUpdateEnabled { get; set; }
		
		public void OnMidiFileLoad()
		{
			PlayedNoteCount = 0;
			TotalNotesCount = CurMidiFile.GetNotes().Count;
			NotesPlayedPerSecond = 0;
			NotesPlayedPerBar = 0;
			NotesPlayedPerBeat = 0;
			NotesCurPlaying = 0;
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			StartCoroutine(CountNotePerSecond());
			StartCoroutine(CountNotePerBar());
			StartCoroutine(CountNotesPerBeat());
		}
		public void Update()
		{
			
		}

		public void OnPlaybackPaused()
		{
			StopCoroutine(CountNotePerSecond());
			StopCoroutine(CountNotePerBar());
			StopCoroutine(CountNotesPerBeat());
		}
		public void OnPlaybackStopped()
		{
			PlayedNoteCount = 0;
			NotesCurPlaying = 0;
			NotesPlayedPerSecond = 0;
			NotesPlayedPerBar = 0;
			NotesPlayedPerBeat = 0;
			CurNotesPlayedPerSecond = 0;
			CurNotesPlayedPerBar = 0;
			CurNotesPlayedPerBeat = 0;
			
			OnPlaybackPaused();
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
			var notes = CurMidiFile.GetNotes();
			// notes.Sort((a, b) => a.Time < b.Time ? -1 : 1);
			PlayedNoteCount = notes.Count(note => note.Time < MidiTime);

			NotesCurPlaying = 0;
		}
		
		
		public static void OnNotesPlaybackStarted(object senderNote, NotesEventArgs e)
		{
			PlayedNoteCount++;
			CurNotesPlayedPerSecond++;
			CurNotesPlayedPerBar++;
			CurNotesPlayedPerBeat++;
			NotesCurPlaying++;
		}

		public static void OnNotesPlaybackFinished(object senderNote, NotesEventArgs e)
		{
			NotesCurPlaying--;
		}

		private static IEnumerator CountNotePerSecond()
		{
			var prevPlayedNoteCount = PlayedNoteCount;
			var waitForASecond = new WaitForSeconds(1f);
			
			while(true) {
				yield return waitForASecond;
				
				NotesPlayedPerSecond = PlayedNoteCount - prevPlayedNoteCount;
				CurNotesPlayedPerSecond = 0;
				prevPlayedNoteCount = PlayedNoteCount;
			}
		}

		private static IEnumerator CountNotePerBar()
		{
			var prevPlayedNoteCount = PlayedNoteCount;
			var prevBar = BarBeatFracTime.Bars;
			var waitForBar = new WaitWhile(() => prevBar == BarBeatFracTime.Bars);
			
			while(true) {
				yield return waitForBar;
				
				NotesPlayedPerBar = PlayedNoteCount - prevPlayedNoteCount;
				prevPlayedNoteCount = PlayedNoteCount;
				CurNotesPlayedPerBar = 0;
				prevBar = BarBeatFracTime.Bars;
			}
			
		}

		private static IEnumerator CountNotesPerBeat()
		{
			var prevPlayedNoteCount = PlayedNoteCount;
			var prevBeat = (int)BarBeatFracTime.Beats;
			var waitForBeat = new WaitWhile(() => prevBeat == (int)BarBeatFracTime.Beats);
			
			while(true) {
				yield return waitForBeat;
				
				NotesPlayedPerBeat = PlayedNoteCount - prevPlayedNoteCount;
				prevPlayedNoteCount = PlayedNoteCount;
				CurNotesPlayedPerBeat = 0;
				prevBeat = (int)BarBeatFracTime.Beats;
			}
		}
 	}
}