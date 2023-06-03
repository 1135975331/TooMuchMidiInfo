using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Utilities
{
	public static class FadeIEnumerators
	{
		public static IEnumerator ObjFadeIn(SpriteRenderer sprRenderer, float untilAlpha, float? intervalSec, byte fadingVariance)
		{
			var waitForAMoment = intervalSec != null ? new WaitForSeconds((float) intervalSec) : null;
			var finalColor     = sprRenderer.color;  finalColor.a = untilAlpha;
     			
			while(true) {
				sprRenderer.color += new Color32(0, 0, 0, fadingVariance);
     
				if(sprRenderer.color.a >= untilAlpha) {
					sprRenderer.color = finalColor;
					// Debug.Log("FadeIn End");
					yield break;
				}
     				
				yield return waitForAMoment;
			}
		}
     
		public static IEnumerator ObjFadeOut(SpriteRenderer sprRenderer, float untilAlpha, float? intervalSec, byte fadingVariance)
		{
			var waitForAMoment = intervalSec != null ? new WaitForSeconds((float) intervalSec) : null;
			var finalColor     = sprRenderer.color;  finalColor.a = untilAlpha;
     			
			while(true) {
				sprRenderer.color -= new Color32(0, 0, 0, fadingVariance);
     
				if(sprRenderer.color.a <= untilAlpha) {
					sprRenderer.color = finalColor;
					yield break;
				}
     
				yield return waitForAMoment;
			}
		}
		
		public static IEnumerator UIFadeIn(Image imageComp, float untilAlpha, float? intervalSec, byte fadingVariance)
		{
			var waitForAMoment = intervalSec != null ? new WaitForSeconds((float) intervalSec) : null;
        
			while(imageComp.color.a <= untilAlpha) {
				imageComp.color += new Color32(0, 0, 0, fadingVariance);
				yield return waitForAMoment;
			}
		}

		public static IEnumerator UIFadeOut(Image imageComp, float untilAlpha, float? intervalSec, byte fadingVariance)
		{
			var waitForAMoment = intervalSec != null ? new WaitForSeconds((float) intervalSec) : null;

			while(imageComp.color.a >= untilAlpha) {
				imageComp.color -= new Color32(0, 0, 0, fadingVariance);
				yield return waitForAMoment;
			}
		}

		public static IEnumerator TMPTextFadeEffect(this TMP_Text tmpText, float fromAlpha, float toAlpha, float duration)
		{
			const float A_MOMENT      = 0.08f;
			var         waitAMoment  = new WaitForSeconds(A_MOMENT);
			var         deltaAlpha   = Mathf.Abs(fromAlpha - toAlpha);
			var         alphaPerLoop = deltaAlpha * A_MOMENT / duration;

			tmpText.alpha = fromAlpha;

			if(fromAlpha < toAlpha)  // FadeIn
				while(fromAlpha <= toAlpha) {
					fromAlpha         += alphaPerLoop;
					tmpText.alpha += alphaPerLoop;
					yield return waitAMoment;
				}
			else if(fromAlpha > toAlpha)  // FadeOut
				while(fromAlpha >= toAlpha) {
					fromAlpha         -= alphaPerLoop;
					tmpText.alpha -= alphaPerLoop;
					yield return waitAMoment;
				}
		}
	}
}