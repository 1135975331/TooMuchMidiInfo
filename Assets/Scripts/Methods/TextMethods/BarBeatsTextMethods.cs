using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Melanchall.DryWetMidi.Interaction;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEngine;
using Utilities;
namespace Methods
{
	public class BarBeatsTextMethods : MonoBehaviour, IMethodsClass
	{
		[field: SerializeField] public bool IsUpdateEnabled { get; set; }
		private TMP_Text curBarsT, barsLeftT, totalBarsT, curBeatsT; 
		// private TMP_Text beatLineT;
		
		private long totalBars;
		
		public void Awake()
		{
			var comps = Util.InitializeComponentVars<TMP_Text>(new[] {
				"CurBarsText", "BarsLeftText", "TotalBarsText", "CurBeatsText"
			});
			
			curBarsT = comps[0];
			barsLeftT = comps[1];
			totalBarsT = comps[2];
			curBeatsT = comps[3];
		}

		public void Start()
		{
		}

		public void Update()
		{
			if(!IsUpdateEnabled) return;
			
			// Bars, Beats
			var curBars = MidiInfo.BarBeatFracTime.Bars + 1;
			curBarsT.SetText(curBars.ToString());
			barsLeftT.SetText((totalBars - curBars).ToString());
			curBeatsT.SetText(((int)Math.Floor(MidiInfo.BarBeatFracTime.Beats + 1)).ToString(CultureInfo.InvariantCulture));
		}

		public void OnMidiFileLoad()
		{
			totalBars = MidiInfo.BarBeatFracDur.Bars + 1;
			totalBarsT.SetText($"{totalBars}");
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
	}
}