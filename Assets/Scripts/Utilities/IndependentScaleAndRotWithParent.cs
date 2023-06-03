using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentScaleAndRotWithParent : MonoBehaviour //todo 클래스 이름을 IndependentScaleWithParent로 바꾼다
{ 
	[LabelOverride("Wanted Child Scale")] public Vector3 wantedCScale;
	[LabelOverride("Wanted Child Rotation")] public Vector3 wantedCRotation;
    
	//pLocal * cLocal = custom (1) -> cLocal = custom(1) / pLocal  <- 공식

	private void Start()
	{
		var transform1 = transform.parent.transform;
		var pLocalScale = transform1.localScale;

		var cScaleX = wantedCScale.x / pLocalScale.x;
		var cScaleY = wantedCScale.y / pLocalScale.y;
		var cScaleZ = wantedCScale.z / pLocalScale.z;
        
		var pLocalRotation = transform1.localRotation;

		var cRotX = wantedCRotation.x / pLocalRotation.x;
		var cRotY = wantedCRotation.y / pLocalRotation.y;
		var cRotZ = wantedCRotation.z / pLocalRotation.z;

		Debug.Log(new Vector3(cRotX, cRotY, cRotZ));

		transform.localScale = new Vector3(cScaleX, cScaleY, cScaleZ);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, cRotZ));
	}
    
	/*void Start()
	{
	    // name = $"{name}_{Util.GetNumberInt(transform.parent.gameObject)}";
	    //var parent = transform.parent;
	    transform.parent = null; 
	    transform.localScale = childScale;
	    transform.localEulerAngles = childRotation;
	    //transform.parent = parent;
	}*/
}