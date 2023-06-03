using Melanchall.DryWetMidi.Interaction;
namespace Methods
{
	public struct Settings
	{
		public static float PlaybackMoveTime { get; set; }
		public static float PlaybackMoveMoreTime { get; set; }
		public static int UiOffsetMs { get; set; }
		public static MetricTimeSpan UiOffsetTimeSpan;
	}
}