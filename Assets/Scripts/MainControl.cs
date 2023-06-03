using System;
using DefaultNamespace;
using Melanchall.DryWetMidi.Multimedia;
using Methods;
using TMPro;
using UnityEngine;


public class MainControl : MonoBehaviour
{
	public string MidiFilePath { get; set; }

	private void Awake()
	{
		PublicTextComponents.DebugConsole = GameObject.Find("DebugConsole").GetComponent<TMP_Text>();
		PublicTextComponents.OutputBox = GameObject.Find("OutputBox").GetComponent<TMP_Text>();
		
		Settings.PlaybackMoveMoreTime = 30;
		Settings.PlaybackMoveTime = 10;
	}

	private void OnApplicationQuit()
	{
		MidiInfo.CurPlayback.Stop();
		MidiInfo.CurPlayback.Dispose();
		MidiInfo.CurOutputDevice.Dispose();
	}
}