using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentScaleWithParent : MonoBehaviour
{ 
    public bool executeConstantly;
    [LabelOverride("Wanted Child Scale")] public Vector3 wantedCScale;
    
    private Transform thisTransform;
    private Transform parentTransform;
    
    public bool applyIndependentX = true;
    public bool applyIndependentY = true;
    public bool applyIndependentZ = true;
    
    //pLocal * cLocal = custom(1) -> cLocal = custom(1) / pLocal  <- 공식

    private void Start()
    {
        thisTransform = transform;
        parentTransform = thisTransform.parent.transform;
        
        MakeScaleIndependent();
    }

    private void Update()
    {
        if(!executeConstantly)  return;
        
        MakeScaleIndependent();
    }



    private void MakeScaleIndependent()
    {
        var pLocalScale = parentTransform.localScale;

        var cScaleX = applyIndependentX ? wantedCScale.x / pLocalScale.x : thisTransform.localScale.x;
        var cScaleY = applyIndependentY ? wantedCScale.y / pLocalScale.y : thisTransform.localScale.y;
        var cScaleZ = applyIndependentZ ? wantedCScale.z / pLocalScale.z : thisTransform.localScale.z;
        
        thisTransform.localScale = new Vector3(cScaleX, cScaleY, cScaleZ);
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
