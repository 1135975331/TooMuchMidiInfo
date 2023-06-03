using System;
using Melanchall.DryWetMidi.Interaction;
using Methods;
using Unity.VisualScripting;

public static class MidiUtil
{
	public static string GetFileExtensionFromPath(string filePath)
		=> filePath.Split('.')[^1];

	public static int GetOctaveDifference(int curOctave, int curChannelOctave)
		=> curChannelOctave - curOctave;

	public static double MidiClockToBeats(int midiClock)  // todo 아래의 /를 *로 바꾸면 4분의 n박 기준으로 박자를 세도록 할 수 있다 -> 서로 변경할 수 있도록 설정을 만든다.
		=>  midiClock / (MidiInfo.MidiTimePerQuarterNote / ((double)MidiInfo.CurTimeSignature.Denominator / 4)) + 1;

	// public static int MidiTimeToBeatsInt(int midiClock)
	// 	=> (int) Math.Ceiling(midiClock / (double)MidiPlaybackInfo.MidiTimePerQuarterNote);
	
	public static int MidiTimeToBeatsInt(int midiClock)
		=> (int)MidiClockToBeats(midiClock);

	public static string FormatMetricTime(MetricTimeSpan metricTime)
		=> $"{metricTime.Hours:D}:{metricTime.Minutes:D2}:{metricTime.Seconds:D2}.{metricTime.Milliseconds / 100}";

	public static float BarBeatToFloatSeconds(int bars, float beats)
	{
		var barBeatTimeSpan = new BarBeatFractionTimeSpan(bars, beats);
		return (float)barBeatTimeSpan.ConvertTo<MetricTimeSpan>().TotalSeconds;
	}
}