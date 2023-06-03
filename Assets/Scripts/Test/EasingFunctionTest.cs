using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Utilities;
namespace Test
{
	public class EasingFunctionTest : MonoBehaviour
	{
		[Range(0.0f, 1.0f)]
		[SerializeField] private float value;
		[SerializeField] private EasingFunction.Ease easeType = EasingFunction.Ease.Linear;
		private void Update()
		{
			var func = EasingFunction.GetEasingFunction(easeType);
			var res = func(100f, -100f, value);
			transform.position = new Vector3(res, res, 0);
		}
	}
}