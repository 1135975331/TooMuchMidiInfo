using UnityEngine;
namespace Utilities
{
	/// <summary>
	/// 
	/// </summary>
	public class FixWorldTransform : MonoBehaviour
	{
		private Transform thisTransform;
		
		[SerializeField] private bool fixPosition;
		[SerializeField] private bool fixRotation;

		[SerializeField] private Vector3 worldPosition;
		[SerializeField] private Vector3 worldRotation;


		private void Start()
		{
			thisTransform = transform;
		}
		
		private void Update()
		{
			if(fixPosition)
				transform.position = worldPosition;
			
			if(fixRotation)
				transform.rotation = Quaternion.Euler(worldRotation);
		}

	}
}