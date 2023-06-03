using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
namespace ComponentScripts
{
	public class CircularPbHandlePoint : MonoBehaviour
	{
		private bool isParentNull;
		private Image parentImgComp;
		[field: SerializeField] public Vector3 centerPos;
		
		[field: SerializeField] public float Radius { get; set; }
		
		[field: Range(0f, 1f)]
		[field: SerializeField] public float Percent { get; set; }

		private void Awake()
		{
			isParentNull = transform.parent == null;
			
			if(!isParentNull)
				parentImgComp = transform.parent.GetComponent<Image>();
				// parentImgComp = transform.parent.GetComponent<Image>();
		}
		
		private void Start()
		{
			centerPos = isParentNull ? transform.position : transform.parent.position;
		}
		
		public void Update()
		{
			if(!isParentNull)
				Percent = parentImgComp.fillAmount;
			
			var isClockwise = parentImgComp.fillClockwise;
			
			transform.position = VectorUtil.GetPosInCircumference(centerPos, GetAngleByPercent(Percent, isClockwise, 90f), Radius);
			transform.rotation = Quaternion.Euler(0f, 0f, GetAngleByPercent(Percent, isClockwise));
		}

		private static float GetAngleByPercent(float percent, bool isClockwise = false, float additionalRotation = 0f)
		{
			var angle = isClockwise ? 360 * (1 - percent) : 360 * percent;
			return angle + additionalRotation;
		}
	}
}