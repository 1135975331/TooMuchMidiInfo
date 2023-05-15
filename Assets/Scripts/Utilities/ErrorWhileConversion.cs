using System;
using System.Linq;
using static ErrorWhileConversion;

public enum ErrorWhileConversion
{
	/* 파일을 선택 중 발생한 에러 */
	NO_FILE_SELECTED,   // 경로를 지정하지 않은 경우
	FILE_NOT_FOUND,     // 지정된 경로에 파일이 없을 경우, 찾을 수 없는 경우
	FILE_NOT_VALID,     // 파일 형식이 지원되지 않는 경우, MIDI 파일이 아닌 경우
	FILE_PATH_TOO_LONG, // 파일 경로가 너무 긴 경우
	
	NO_CHANNEL_SELECTED,  // 변환할 채널을 선택하지 않은 경우
	
	/*MML 채널 세팅 중 발생한 에러*/
	INVALID_CHANNEL_NAME,
	INVALID_INSTRUMENT_NUM,
	INVALID_CHANNEL_VOLUME,
	INVALID_DEFAULT_OCTAVE,
	INVALID_DEFAULT_NOTE_LENGTH,
	
	/*미디 노트 분석 중 발생한 에러*/
	MORE_THAN_TWO_NOTES_SIMULTANEOUSLY,  // 변환 작업 중 노트가 2개 이상 동시에 연주되도록 배치되어있음을 발견한 경우 
	
	/*위에 해당하지 않는 에러*/
	UNKNOWN_ERROR
}

public static class ErrorWhileConversionMethods
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="error"></param>
	/// <param name="lineBreakAmount"></param>
	/// <param name="exception"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public static string GetErrorMessage(ErrorWhileConversion error, int lineBreakAmount = 0, Exception exception = null)
	{
		var errMsg = error switch {
			NO_FILE_SELECTED   => "Error: No MIDI file is selected.",
			FILE_NOT_FOUND     => "Error: File is not found in that path.",
			FILE_NOT_VALID     => "Error: File is not valid. Please select MIDI(*.mid) file.",
			FILE_PATH_TOO_LONG => "Error: File path or name is too long. (paths must be less than 248 characters, and file names must be less than 260 characters)",
			
			NO_CHANNEL_SELECTED => "Error: No channel selected.",
			
			INVALID_CHANNEL_NAME        => "MML Channel: Invalid value, please input a letter in A ~ I",
			INVALID_INSTRUMENT_NUM      => "Instrument Number(@): Invalid value, please input an integer between 0-255.",
			INVALID_CHANNEL_VOLUME      => "MML Channel Volume(v): Invalid value, please input an integer between 0-15.",
			INVALID_DEFAULT_OCTAVE      => "Default Octave(o): Invalid value, please input an integer between 1-8.",
			INVALID_DEFAULT_NOTE_LENGTH => "Default Note Length(l): Invalid value, please input an integer among 1, 2, 3, 4, 6, 8, 12, 16, 24, 32, 48, 64 or 96.",
			
			MORE_THAN_TWO_NOTES_SIMULTANEOUSLY => "There are notes playing simultaneously.",
			
			UNKNOWN_ERROR => "Error: Unknown error.",
			
			_ => throw new ArgumentOutOfRangeException($"Falied to get error message, Unknown error: {error}")
		};
		
		errMsg += new string('\n', lineBreakAmount);
		
		if(error == UNKNOWN_ERROR)
			errMsg += "\n" + exception!.Message;
		
		return errMsg;
	}
}