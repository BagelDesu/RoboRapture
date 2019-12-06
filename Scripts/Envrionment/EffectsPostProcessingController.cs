

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Edu.Vfs.RoboRapture.EffectsSystem
{

    public enum EffectType
    {
        None,
        VignetteFocus,
        ExposureFlash,
        ChromaticAbberationFlash
    }
    ///<summary>
    ///-Applies quick post processing effects-
    ///</summary>
    public class EffectsPostProcessingController : MonoBehaviour
    {
        private static EffectsPostProcessingController instance;
        public static EffectsPostProcessingController Instance {get => instance; private set => instance = value;}

        private PostProcessVolume volume;

        private Vignette vignette;
        private ColorGrading grading;
        private ChromaticAberration chromatic;

        /// <summary>
        /// Set up the static class as a singleton pattern.
        /// </summary>
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                Debug.Log("[Effects Controller] Successfully created Post Processing Controller", this);
            }
            else
            {
                Debug.Log($"[Effects Controller] Duplicate detected... Deleting {this.gameObject.name}'s Effects controller", this);
                Destroy(this);
            }

            volume = GetComponent<PostProcessVolume>();

            volume.profile.TryGetSettings<ChromaticAberration>(out chromatic);
            volume.profile.TryGetSettings<Vignette>(out vignette);
            volume.profile.TryGetSettings<ColorGrading>(out grading);
        }

        public void ApplyEffect(EffectType type, float effectDuration, float effectSpeed, float effectStrength)
        {
            switch (type)
            {
                case EffectType.VignetteFocus:
                    StartCoroutine(PushVignetteEffect(effectSpeed, effectStrength));
                    break;
                case EffectType.ChromaticAbberationFlash:
                    StartCoroutine(PushAbberationEffect(effectSpeed, effectStrength));
                    break;
                case EffectType.ExposureFlash:
                    StartCoroutine(ExposureFlashEffect(effectSpeed, effectStrength));
                    break;
                default:
                    break;
            }

            StartCoroutine(PushEffects(effectDuration));
        }

        private IEnumerator PushVignetteEffect(float effectSpeed, float effectStrength)
        {
            float t = 0;

            while(vignette.intensity.value < effectStrength)
            {
                t += Time.deltaTime/effectSpeed;
                vignette.intensity.value = AbsoluteLerp(vignette.intensity.value, effectStrength, t);

                yield return null;
            }
        }

        private IEnumerator ExposureFlashEffect(float effectSpeed, float effectStrength)
        {
            float t = 0;

            while(grading.postExposure.value < effectStrength)
            {
                t += Time.deltaTime/effectSpeed;
                grading.postExposure.value = AbsoluteLerp(grading.postExposure.value, effectStrength, t);

                yield return null;
            }
        }

        private IEnumerator PushAbberationEffect(float effectSpeed, float effectStrength)
        {
            float t = 0;

            while(chromatic.intensity.value < effectStrength)
            {
                t += Time.deltaTime/effectSpeed;
                chromatic.intensity.value = AbsoluteLerp(chromatic.intensity.value, effectStrength, t);

                yield return null;
            }
        }

        private IEnumerator PushEffects(float effectDuration)
        {
            StartCoroutine(StepVolumeWeightTo(1));
            yield return new WaitForSeconds(effectDuration);
            StopCoroutine(StepVolumeWeightTo(1));
            StartCoroutine(StepVolumeWeightTo(0));

            // pop the effects.

            vignette.intensity.value = 0;
            chromatic.intensity.value = 0;
            grading.postExposure.value = 0;
        }

        private IEnumerator StepVolumeWeightTo(float destination)
        {
            float t = 0;

            while (volume.weight != destination)
            {
                t += Time.deltaTime / 0.2f;
                volume.weight = AbsoluteLerp(volume.weight, destination, t);
                yield return null;
            }
        }

        private float AbsoluteLerp(float initialTarget, float target , float time)
        {
            return Mathf.Lerp(initialTarget, target, Mathf.SmoothStep(0.0f, 1.0f, time));
        }
    }
}
