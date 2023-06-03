using UnityEngine;
namespace Utilities
{
	/// <summary>
	/// Scale값 보정 클래스
	/// </summary>
	
	public class SubFieldBorderScaleCorrection : MonoBehaviour
	{
		private Transform parentTransform;
		private Vector3 prevScale;
		
		private void Start()
		{
			parentTransform = this.transform.parent;
		}

		private void Update()
		{
			var parentLocalScale = parentTransform.localScale;
			if(parentLocalScale == prevScale)
				return;
			
			var transform1       = transform;
			// transform1.localScale.OffsetY(parentLocalScale.x / parentLocalScale.y)
		}
	}
}