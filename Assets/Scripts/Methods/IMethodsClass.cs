using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
namespace Methods.MidiInfoMethodsClasses
{
	public interface IMethodsClass
	{
		public bool IsUpdateEnabled { get; set; }
		
		public void OnMidiFileLoad();
		public void OnPlaybackStarted(object sender, EventArgs e);
		public void OnPlaybackPaused();
		public void OnPlaybackStopped();
		public void OnPlaybackFinished(object sender, EventArgs e);
		public void OnPlaybackSeek();

		
	}
}