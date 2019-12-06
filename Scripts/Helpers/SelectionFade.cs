

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///-summary of script here-
///</summary>
//TODO: HACK UNIFY THIS CLASS WITH THE RESULTS PANEL AND MOVE ABSOLUTE LERP TO A STATIC LIBRARY
public class SelectionFade : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup curtain;
    private float crossFadePrecentage;

    [SerializeField]
    private float crossFadeDuration;
    [SerializeField]
    private float crossFadeSpeed;

    public UnityEvent OnButtonPressed;

    public void RaiseCurtain()
    {
        StartCoroutine(StartCrossFade());
    }

    private IEnumerator StartCrossFade()
    {
        float t = 0;
        while(crossFadePrecentage < 1)
        {
            t += Time.deltaTime / crossFadeDuration;

            crossFadePrecentage = AbsoluteLerp(crossFadePrecentage, 1, t);
            curtain.alpha = crossFadePrecentage;
 
            yield return new WaitForSeconds(crossFadeSpeed);
        }

        OnButtonPressed.Invoke();
        
    }

    private float AbsoluteLerp(float initial, float target , float time)
    {
        return Mathf.Lerp(initial, target, Mathf.SmoothStep(0.0f, 1.0f, time));
    }
}
