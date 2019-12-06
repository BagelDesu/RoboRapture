using System;
using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.GrandFinale;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField]
    private Vector3 initialPosition = new Vector3(-3f, 12f, -5f);
    [SerializeField]
    private Vector3 cameraStep = new Vector3(1f, 0f, 0f);
    [SerializeField]
    private Vector3 ResetPosition;

    [SerializeField]
    private float PanSpeed;

    [SerializeField]
    private float StopDuration;

    [SerializeField]
    private float PanDistance;

    [SerializeField]
    private bool AllowReset;

    [SerializeField]
    private CreditsSceneManager PManager;

    [SerializeField]
    private GameObject EndTurn;

    public bool ShouldRemoveEndTurn;

    [SerializeField]
    private int PanAmount;

    private bool CanResetCamera = false;

    public static event Action OnPanFinished;
    
    private void Awake()
    {
        transform.position = initialPosition;
    }

    public void MoveCameraForward()
    {
        transform.position += cameraStep;
    }

    public void MoveCameraBackward()
    {
        transform.position -= cameraStep;
    }

    private void Update()
    {
        if(CanResetCamera && AllowReset)
        {
            if(Input.anyKeyDown)
            {
                ResetCameraToStart();
                PManager?.ResetLastPanel();
                CanResetCamera = false;
            }
        }
    }

    public void PerformEndroll()
    {
        if(ShouldRemoveEndTurn)
        {
            EndTurn.SetActive(false);
        }
        
        StartCoroutine(Endroll());
    }

    private IEnumerator Endroll()
    {
        int TransitionAmounts = 0;

        while(TransitionAmounts < PanAmount)
        {
            float t = 0;

            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + PanDistance, transform.position.z);

            while(transform.position.y < targetPos.y)
            {
                t += Time.deltaTime / PanSpeed;
                Vector3 pos = transform.position;
                pos.y = AbsoluteLerp(transform.position.y, targetPos.y, t);
                transform.position = pos;
                yield return null;
            }

            TransitionAmounts++;
            OnPanFinished?.Invoke();
            
            yield return new WaitForSeconds(StopDuration);
        }

        CanResetCamera = true;
    }

    public void ResetCameraToStart()
    {
        transform.position = ResetPosition;
    }

    private float AbsoluteLerp(float initialTarget, float target , float time)
    {
        return Mathf.Lerp(initialTarget, target, Mathf.SmoothStep(0.0f, 1.0f, time));
    }
}
