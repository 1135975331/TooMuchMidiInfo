using System;
using System.Collections;
using System.Collections.Generic;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEngine;
using Utilities;
using static Methods.MidiInfo;
namespace Methods
{
	public class NoteCountTextMethods : MonoBehaviour, IMethodsClass
	{
		private TMP_Text curNotesT, curNotesLeftT, noteCountFracLineT, totalNotesT, notesCurPlayingT,
		                 notesPerSecondT, notesPerBarT, notesPerBeatT, 
		                 curNotesPerSecondT, curNotesPerBarT, curNotesPerBeatT; 

		[field: SerializeField] public bool IsUpdateEnabled { get; set; }

		public void Awake()
		{
			var comps = Util.InitializeComponentVars<TMP_Text>(new[] {
				"CurNotesText", "CurNotesLeftText", "NoteCountFracLineText", "TotalNotesText", "NotesCurPlayingText",
				"NotesPerSecondText", "NotesPerBarText", "NotesPerBeatText",
				"CurNotesPerSecondText", "CurNotesPerBarText", "CurNotesPerBeatText",
			});
			
			curNotesT          = comps[0];
			curNotesLeftT      = comps[1];
			noteCountFracLineT = comps[2];
			totalNotesT        = comps[3];
			notesCurPlayingT   = comps[4];
			
			notesPerSecondT    = comps[5];
			notesPerBarT       = comps[6];
			notesPerBeatT      = comps[7];
			
			curNotesPerSecondT    = comps[8];
			curNotesPerBarT       = comps[9];
			curNotesPerBeatT      = comps[10];
		}

		public void Start()
		{
		}

		public void Update()
		{
			if(!IsUpdateEnabled) return;
			
			SetPlayedNotesCountText();
			SetNotesPerTexts();
		}

		public void OnMidiFileLoad()
		{
			totalNotesT.SetText($"{TotalNotesCount:N0}");
			SetPlayedNotesCountText();
			SetNotesPerTexts();
		}
		public void OnPlaybackStarted(object sender, EventArgs e)
		{
			StartCoroutine(DisplayNotesPerSecond());
			StartCoroutine(DisplayNotesPerBar());
			StartCoroutine(DisplayNotesPerBeat());
		}
		public void OnPlaybackPaused()
		{
		}
		public void OnPlaybackStopped()
		{
			SetPlayedNotesCountText();
			SetNotesPerTexts();
			
			notesPerSecondT.SetText("0");
			notesPerBarT.SetText("0");
			notesPerBeatT.SetText("0");
		}
		public void OnPlaybackFinished(object sender, EventArgs e)
		{
			// Debug.Log("NoteCountTextMethod.cs -> OnPlaybackFinished");
			UnityMainThread.Wkr.AddJob(OnPlaybackStopped);
		}
		public void OnPlaybackSeek()
		{
		}


		private void SetPlayedNotesCountText()
		{
			curNotesLeftT.SetText($"-{TotalNotesCount - PlayedNoteCount:N0}");
			curNotesT.SetText($"{PlayedNoteCount:N0}");
		}

		private void SetNotesPerTexts()
		{
			curNotesPerSecondT.SetText($"{CurNotesPlayedPerSecond:N0}");
			curNotesPerBarT.SetText($"{CurNotesPlayedPerBar:N0}");
			curNotesPerBeatT.SetText($"{CurNotesPlayedPerBeat:N0}");
			notesCurPlayingT.SetText($"{NotesCurPlaying:N0}");
		}
		
		
		private IEnumerator DisplayNotesPerSecond()
		{
			var waitForASecond = new WaitForSeconds(1f);
			
			while(true) {
				yield return waitForASecond;
				
				if(!IsUpdateEnabled) {
					notesPerSecondT.SetText($"0");
					yield break;
				}
				// notesPerSecondT.SetText($"{NotesPlayedPerSecond:N0}");
				StartCoroutine(notesPerSecondT.NumChangeAnim(NotesPlayedPerSecond, true, 0.5f, easeType: EasingFunction.Ease.EaseOutExpo));
			}
		}
		
		private IEnumerator DisplayNotesPerBar()
		{
			var prevBar = BarBeatFracTime.Bars;
			var waitForBar = new WaitWhile(() => prevBar == BarBeatFracTime.Bars);
			
			while(true) {
				yield return waitForBar;

				if(!IsUpdateEnabled) {
					notesPerBarT.SetText($"0");
					yield break;
				}

				// notesPerBarT.SetText($"{NotesPlayedPerBar:N0}");
				StartCoroutine(notesPerBarT.NumChangeAnim(NotesPlayedPerBar, true, 0.5f, "Bar", EasingFunction.Ease.EaseOutExpo));
				prevBar = BarBeatFracTime.Bars;
			}
		}
		
		private IEnumerator DisplayNotesPerBeat()
		{
			var prevBeat = (int)BarBeatFracTime.Beats;
			var waitForBeat = new WaitWhile(() => prevBeat == (int)BarBeatFracTime.Beats);
			
			while(true) {
				yield return waitForBeat;
				
				if(!IsUpdateEnabled) {
					notesPerBeatT.SetText($"0");
					yield break;
				}
				// notesPerBeatT.SetText($"{NotesPlayedPerBeat:N0}");
				StartCoroutine(notesPerBeatT.NumChangeAnim(NotesPlayedPerBeat, true, 0.5f, "Beat", EasingFunction.Ease.EaseOutExpo));
				prevBeat = (int)BarBeatFracTime.Beats;
			}
		}
	}
}