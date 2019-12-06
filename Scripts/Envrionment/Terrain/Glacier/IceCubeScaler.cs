

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class IceCubeScaler : MonoBehaviour
{
    [SerializeField] [Tooltip("How big is the change in scale?")]
    private float ReductionFactor;
    [SerializeField] [Tooltip("How fast is it when the scale goes from one factor to another?")]
    private float ReductionSpeed;
    [SerializeField] [Tooltip("Similar to health, The amount of times the ice berg can melt before breaking.")] [Range(0, 5)]
    private int Iterations;

    [SerializeField]
    private DestroyableBlockers DestroyableBlocker;

    private int CurrentIterations;

    private bool HasStarted;

    private void Start()
    {
        CurrentIterations = 0;
        HasStarted = true;
    }

    public void ReduceSize()
    {
        if(!HasStarted)
        {
            return;
        }

        StartCoroutine(ScaleCube());
        CurrentIterations++;
        if(CurrentIterations >= Iterations)
        {
            StopAllCoroutines();
            DestroyableBlocker.DestroyTerrain();
        }
    }

    private IEnumerator ScaleCube()
    {
        Vector3 localScale = this.transform.localScale;
        Vector3 targetScale = new Vector3(localScale.x - ReductionFactor, localScale.y - ReductionFactor, localScale.z - ReductionFactor);
        float t = 0;

        while(localScale != targetScale)
        {
            t += Time.deltaTime/ReductionSpeed;

            this.transform.localScale = new Vector3( AbsoluteLerp(localScale.x,targetScale.x ,t),
                                                     AbsoluteLerp(localScale.y,targetScale.y, t),
                                                     AbsoluteLerp(localScale.z,targetScale.z, t));
            yield return null;
        }
    }

    private float AbsoluteLerp(float initialTarget, float target , float time)
    {
        return Mathf.Lerp(initialTarget, target, Mathf.SmoothStep(0.0f, 1.0f, time));
    }
}
