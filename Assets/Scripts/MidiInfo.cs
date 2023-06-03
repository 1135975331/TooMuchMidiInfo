using System;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
namespace Methods
{
	public static class MidiInfo
	{
		public static MidiFile CurMidiFile { get; set; }
		public static Playback CurPlayback { get; set; }
		public static OutputDevice CurOutputDevice { get; set; }
		
		
		public static MetricTimeSpan MetricRealTime { get; set; }
		public static MetricTimeSpan MetricTimeLeft { get; set; }
		public static float MetricTimePercentage { get; set; }
		
		public static MidiTimeSpan MidiTime { get; set; }
		public static MusicalTimeSpan MusicalTime { get; set; }
		public static BarBeatFractionTimeSpan BarBeatFracTime { get; set; }
		public static BarBeatTicksTimeSpan BarBeatTicksTime { get; set; }

		public static MetricTimeSpan MetricDur { get; set; }
		public static MidiTimeSpan MidiDur { get; set; }
		public static MusicalTimeSpan MusicalDur { get; set; }
		public static BarBeatFractionTimeSpan BarBeatFracDur { get; set; }
		public static BarBeatTicksTimeSpan BarBeatTicksDur { get; set; }
		
	
		public static TempoMap CurTempoMap { get; set; } = null!;
		public static TimeSignature CurTimeSignature { get; set; }
		public static ValueChange<TimeSignature> NextTimeSig { get; set; } = null!;
		public static ValueChange<TimeSignature>[] TimeSigChanges { get; set; }
		public static double CurBpm { get; set; }
		public static ValueChange<Tempo> NextTempo { get; set; } = null!;
		public static ValueChange<Tempo>[] TempoChanges { get; set; }
	
		public static int MidiTimePerQuarterNote { get; set; }
		
		
		public static long PlayedNoteCount { get; set; }
		public static long TotalNotesCount { get; set; }
		public static long NotesCurPlaying { get; set; }
		public static long NotesPlayedPerSecond { get; set; }
		public static long NotesPlayedPerBar { get; set; }
		public static long NotesPlayedPerBeat { get; set; }
		public static long CurNotesPlayedPerSecond { get; set; }
		public static long CurNotesPlayedPerBar { get; set; }
		public static long CurNotesPlayedPerBeat { get; set; }

	}
}