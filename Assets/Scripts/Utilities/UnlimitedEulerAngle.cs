using System;
using UnityEngine;
namespace Utilities
{
	public class UnlimitedEulerAngle : MonoBehaviour
	{
		private Transform thisTransform;
		[field: SerializeField] public Vector3 UnlimEulerAngle { get; set; }

		private void Awake()
		{
			thisTransform = transform;
			UnlimEulerAngle = thisTransform.eulerAngles;
		}
		private void Update()
		{
			thisTransform.localRotation = Quaternion.Euler(UnlimEulerAngle);
		}
	}
}