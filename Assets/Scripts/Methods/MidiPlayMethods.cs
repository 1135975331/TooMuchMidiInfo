#define DEBUG
// #undef DEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnotherFileBrowser.Windows;
using DefaultNamespace;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Methods.MidiInfoMethodsClasses;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using static ErrorWhileConversion;
using static ErrorWhileConversionMethods;
using ReadingSettings = Melanchall.DryWetMidi.Core.ReadingSettings;
namespace Methods
{
	public class MidiPlayMethods : MonoBehaviour
	{
		private MainControl mainCtrl;
		private TextMainMethods textMainMethods;
		private ProgressbarMethods pbMethods;
		private MidiInfoOtherMethods midiInfoOtherMethods;
		private IMethodsClass[] iMethodsClassComps;
		
		private MonoBehaviour[] methodsCompArr;
		
		private TMP_Dropdown outputDeviceSelector;
		private TMP_Text midiFilePathTextBox;

		private void Awake()
		{
			mainCtrl = GameObject.Find("MainControlObj").GetComponent<MainControl>();
			
			var methodsObjParentTransform = GameObject.Find("MethodsObjects").transform;
			// midiInfoOtherMethods = methodsObjParentTransform.GetChild(0).GetComponent<MidiInfoOtherMethods>();
			// textMainMethods   = methodsObjParentTransform.GetChild(1).GetComponent<TextMainMethods>();
			pbMethods     = methodsObjParentTransform.GetChild(0).GetComponent<ProgressbarMethods>();
			
			iMethodsClassComps = GameObject.Find("IMethodsClassObj").GetComponentsInChildren<IMethodsClass>();
			/*
			var retList     = new List<IMethodsClass>();
			var canvasObjects = GameObject.FindGameObjectsWithTag("IMethodsClass");
			foreach(var canvasObj in canvasObjects) 
				retList.AddRange(canvasObj.GetComponentsInChildren<IMethodsClass>());
			iMethodsClassComps = retList.ToArray();
			*/
			
			
			

			// methodsCompArr = new MonoBehaviour[] { textMainMethods, pbMethods, midiInfoOtherMethods };
			methodsCompArr = new MonoBehaviour[] { pbMethods };
			

			outputDeviceSelector = GameObject.Find("OutputDeviceSelector").GetComponent<TMP_Dropdown>();
			
			midiFilePathTextBox = GameObject.Find("MidiFilePathTextBox").GetComponent<TMP_Text>();
			
			InitMidiDeviceSelector();
			
#if DEBUG
			var path = @"D:\Library\Write\MIDI\MidiFiles\th06_15_owen_arrange.mid";
			// var path = @"D:\Library\Write\MIDI\MidiFiles\ForMidiDisplayerTest.mid";
			LoadMidiFile(path);
#endif
		}
		
		public void InitMidiDeviceSelector()
		{
			outputDeviceSelector.options.Clear();
			
			for(var i = 0; i < OutputDevice.GetDevicesCount(); i++) 
				 outputDeviceSelector.options.Add(new TMP_Dropdown.OptionData(OutputDevice.GetByIndex(i).Name));

			for(var i = 0; i < OutputDevice.GetDevicesCount(); i++) {
				if(OutputDevice.GetByIndex(i).Name.Contains("Virtual"))
					outputDeviceSelector.value = i;
			}
		}
	
		public void FileSelect()
		{
			var browserProp = new BrowserProperties {
				title            = "Select MIDI File...",
				filter           = "MIDI files (*.mid)|*.mid;*.midi|All files (*.*)|*.*",
				filterIndex      = 1,
				restoreDirectory = true
			};
			
			var filePath = new FileBrowser().OpenFileBrowser(browserProp);
			if(filePath != null)
				LoadMidiFile(filePath);
			
			/*
			if(openFileDialog.ShowDialog() == true)  //Get the path of specified file
				LoadMidiFile(openFileDialog.FileName);
		*/
		}
    	
		public void LoadMidiFile(string filePath)
		{
			mainCtrl.MidiFilePath    = filePath;
			midiFilePathTextBox.text = filePath;
    
			if(!MidiUtil.GetFileExtensionFromPath(filePath).Equals("mid")) {
				PublicTextComponents.OutputBox.text = GetErrorMessage(FILE_NOT_VALID); 
				return;
			}
    	
			var midiReadingSetting1 = new ReadingSettings {
				ReaderSettings = {
					NonSeekableStreamBufferSize                       = 256,  // default: 1024
					NonSeekableStreamIncrementalBytesReadingThreshold = 4096, // default: 16384
					NonSeekableStreamIncrementalBytesReadingStep      = 512,  // default: 2048
					BufferSize                                        = 2048,
					BufferingPolicy                                   = BufferingPolicy.UseFixedSizeBuffer, // default: UseFixedSizeBuffer
					BytesPacketMaxLength                              = 1024,                          // default: 4096
				}
			};
			
			var midiReadingSetting2 = new ReadingSettings {
				ReaderSettings = {
					NonSeekableStreamBufferSize                       = 2018,  // default: 1024
					NonSeekableStreamIncrementalBytesReadingThreshold = 32767, // default: 16384
					NonSeekableStreamIncrementalBytesReadingStep      = 4096,  // default: 2048
					BufferSize                                        = 4096,
					BufferingPolicy                                   = BufferingPolicy.UseFixedSizeBuffer, // default: UseFixedSizeBuffer
					BytesPacketMaxLength                              = 8192,                          // default: 4096
				}
			};
			
		
			try { MidiInfo.CurMidiFile = MidiFile.Read(mainCtrl.MidiFilePath, midiReadingSetting1); }
			catch(ArgumentException) { PublicTextComponents.OutputBox.text          = GetErrorMessage(FILE_NOT_FOUND); return; }
			catch(PathTooLongException) { PublicTextComponents.OutputBox.text       = GetErrorMessage(FILE_PATH_TOO_LONG); return; }
			catch(DirectoryNotFoundException) { PublicTextComponents.OutputBox.text = GetErrorMessage(FILE_NOT_FOUND); return; }
			catch(UnknownFileFormatException) { PublicTextComponents.OutputBox.text = GetErrorMessage(FILE_NOT_VALID); return; }
			catch(Exception e) { PublicTextComponents.OutputBox.text                = GetErrorMessage(UNKNOWN_ERROR, exception: e); return; }
    		
			if(MidiInfo.CurOutputDevice != null)
				MidiInfo.CurOutputDevice.Dispose();
			
			SetOutputDeviceToCurSelection();
			
			
			MidiInfo.CurPlayback          =  MidiInfo.CurMidiFile.GetPlayback(MidiInfo.CurOutputDevice);
			MidiInfo.CurPlayback.Started  += OnMidiPlaybackStarted;
			MidiInfo.CurPlayback.Stopped  += OnMidiPlaybackPaused;
			MidiInfo.CurPlayback.Finished += OnMidiPlaybackFinished;
			MidiInfo.CurPlayback.NotesPlaybackStarted += MidiInfoNoteMethods.OnNotesPlaybackStarted;
			MidiInfo.CurPlayback.NotesPlaybackFinished += MidiInfoNoteMethods.OnNotesPlaybackFinished;

			
			MidiInfo.CurPlayback.InterruptNotesOnStop = true;
			
			foreach(var methodClass in iMethodsClassComps) {
				methodClass.OnMidiFileLoad();
				MidiInfo.CurPlayback.Started += methodClass.OnPlaybackStarted;
				// MidiInfo.CurPlayback.Stopped += methodClass.OnPlaybackPaused;
				MidiInfo.CurPlayback.Finished += methodClass.OnPlaybackFinished;
			}
		}


		
		public void OnPlayButtonClicked()
		{
			if(MidiInfo.CurPlayback == null)
				return;
				
			var offsetInputField = GameObject.Find("UIOffsetSettingInputField").GetComponent<TMP_InputField>();
			
			Settings.UiOffsetMs = int.TryParse(offsetInputField.text.Trim(), out var offsetMs) ? offsetMs : 130;
			Settings.UiOffsetTimeSpan = new MetricTimeSpan(Settings.UiOffsetMs * 1000);
			
			MidiInfo.CurPlayback.Start();
			
		}
		
		private void OnMidiPlaybackStarted(object sender, EventArgs e) // OutputDevice에서 실제로 시작했을 때 호출됨
		{
			ActivationControl(true);
		}
		

		public void OnPauseButtonClicked()
		{
			if(MidiInfo.CurPlayback == null)
				return;
				
			MidiInfo.CurPlayback.Stop();
			
		}
		
		private void OnMidiPlaybackPaused(object sender, EventArgs e)
		{
			foreach(var methodClass in iMethodsClassComps)
				methodClass.OnPlaybackPaused();
				
			ActivationControl(false);
		}

		public void OnStopButtonClicked()
		{
			if(MidiInfo.CurPlayback == null)
				return;
			
			MidiInfo.CurPlayback.Stop();
			MidiInfo.CurPlayback.MoveToStart();
			
			foreach(var methodClass in iMethodsClassComps)
				methodClass.OnPlaybackStopped();
			
			ActivationControl(false);
		}
		
		private void OnMidiPlaybackFinished(object sender, EventArgs e)
		{
			MidiInfo.CurPlayback.MoveToStart();
			MidiInfo.CurPlayback.Stop();
			UnityMainThread.Wkr.AddJob(() => {
				foreach(var methodClass in iMethodsClassComps)
					 methodClass.OnPlaybackFinished(sender, e);
					 
				ActivationControl(false);
			});
			
			pbMethods.OnPlaybackFinished();
		}

		public void OnPlaybackMoveButtonClicked(string buttonArg)
		{
			if(MidiInfo.CurMidiFile == null && MidiInfo.CurPlayback == null)
				return;
			
			switch(buttonArg) {
				case "LeftMore":
					MidiInfo.CurPlayback.MoveBack(new MetricTimeSpan(TimeSpan.FromSeconds(Settings.PlaybackMoveMoreTime)));
					break;
					
				case "Left":
					MidiInfo.CurPlayback.MoveBack(new MetricTimeSpan(TimeSpan.FromSeconds(Settings.PlaybackMoveTime)));
					break;
					
				case "Right":
					MidiInfo.CurPlayback.MoveForward(new MetricTimeSpan(TimeSpan.FromSeconds(Settings.PlaybackMoveTime)));
					break;
					
				case "RightMore":
					MidiInfo.CurPlayback.MoveForward(new MetricTimeSpan(TimeSpan.FromSeconds(Settings.PlaybackMoveMoreTime)));
					break;
				
				default:
					throw new ArgumentException($"Invalid button argument: {buttonArg}");
			}
			
			MidiInfo.CurOutputDevice.TurnAllNotesOff();

			foreach(var methodClass in iMethodsClassComps)
				methodClass.OnPlaybackSeek();
		}

		private void SetOutputDeviceToCurSelection()
			=> MidiInfo.CurOutputDevice = OutputDevice.GetByIndex(outputDeviceSelector.value);


		private void ActivationControl(bool atvStatus)
		{
			UnityMainThread.Wkr.AddJob(() => {
				foreach(var methodComp in methodsCompArr)
					methodComp.gameObject.SetActive(atvStatus);
					
				foreach(var iMethodsComp in iMethodsClassComps) 
					iMethodsComp.IsUpdateEnabled = atvStatus;
			});
		}
	
		/*
		public void SeekPlayback()
		{
			var timeToSeek = new MetricTimeSpan(TimeSpan.FromSeconds(SeekbarSlider.Value));
			MidiPlaybackInfo.CurPlayback.MoveToTime(timeToSeek);
		}
	*/
	}
}