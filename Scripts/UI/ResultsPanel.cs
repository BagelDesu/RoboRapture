//-----------------------------------------------------------------------
// <copyright file="ResultsPanel.cs" company="VFS">
// Copyright (c) VFS. All rights reserved.
// </copyright>
// <author>Angelica Mendez</author>
//-----------------------------------------------------------------------
namespace Edu.Vfs.RoboRapture.UI
{
    using System.Collections;
    using Edu.Vfs.RoboRapture.ResultsSystem;
    using UnityEngine;
    using UnityEngine.UI;
   
    public enum ResultType
    {
        WIN,
        LOSE,
        CURTAIN
    }

    public class ResultsPanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup LoseScreen;
        [SerializeField]
        private CanvasGroup WinScreen;

        private float crossFadePrecentage;

        [SerializeField]
        private float crossFadeDuration;
        [SerializeField]
        private float crossFadeSpeed;

        [SerializeField]
        private ResultScreenManager resultScreenManager;


        public void EnterResultsScreen(ResultType type)
        {
            StartCoroutine(StartCrossFade(type));
        }

        public void RaiseLose()
        {
            Debug.Log("Starting CrossFade For Lose");
            StartCoroutine(StartCrossFade(ResultType.LOSE));
        }

        public void RaiseWin()
        {
            Debug.Log("Starting CrossFade For Win");
            StartCoroutine(StartCrossFade(ResultType.WIN));
        }

        private IEnumerator StartCrossFade(ResultType type)
        {
            switch (type)
            {
                case ResultType.WIN:
                    WinScreen.interactable = true;
                    WinScreen.blocksRaycasts = true;
                    break;
                case ResultType.LOSE:
                    LoseScreen.interactable = true;
                    LoseScreen.blocksRaycasts = true;
                    break;
                default:
                    Debug.LogError("Impossible Evaluation");
                    throw new System.Exception();
            }
            
            float t = 0;
            while(crossFadePrecentage < 1)
            {
                t += Time.deltaTime / crossFadeDuration;

                crossFadePrecentage = AbsoluteLerp(crossFadePrecentage, 1, t);

                switch (type)
                {
                    case ResultType.WIN:
                        WinScreen.alpha = crossFadePrecentage;
                        break;
                    case ResultType.LOSE:
                        LoseScreen.alpha = crossFadePrecentage;
                        break;
                    default:
                        Debug.LogError("Impossible Evaluation");
                        throw new System.Exception();
                }

                yield return null;
            }

            resultScreenManager.UpdateTexts();
        }

        private float AbsoluteLerp(float initial, float target , float time)
        {
            return Mathf.Lerp(initial, target, Mathf.SmoothStep(0.0f, 1.0f, time));
        }
    }
}