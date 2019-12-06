

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.Environment;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Edu.Vfs.RoboRapture.WeatherSystem
{
    [System.Serializable]
    public struct BiomeData
    {
        public BiomeType Biome;
        public PostProcessVolume Volume;
    }

    [System.Serializable]
    public struct BiomeEffectData
    {
        public BiomeType Biome;
        public GameObject Effect;
    }
    
    ///<summary>
    ///-Handles the switching of weather-
    ///</summary>
    public class WeatherManager : MonoBehaviour
    {
        [SerializeField]
        private float TransitionSpeed;

        [SerializeField]
        private BiomeType StartingBiome;

        [SerializeField]
        private float ParticleTransitionTime;

        [SerializeField]
        private BiomeData[] Biomes;

        [SerializeField]
        private BiomeEffectData[] Effects;

        private bool HasFinishedTransition = true;

        private Dictionary<BiomeType, PostProcessVolume> BiomeAmbiences = new Dictionary<BiomeType, PostProcessVolume>();
        public Dictionary<BiomeType, GameObject> BiomeEffects {get; private set;} = new Dictionary<BiomeType, GameObject>(); 

        private PostProcessVolume ActiveBiome;
        private BiomeType Type;

        public static event Action<PostProcessVolume> OnBiomeChanged;

        private void OnEnable()
        {
            BoardController.OnBoardSwitch += BoardSwapped;
        }

        private void OnDisable()
        {
            BoardController.OnBoardSwitch -= BoardSwapped;            
        }

        private void Start()
        {
            ConvertBiomeDataToDictionary();
            ConvertEffectsToDictionary();
            SetEmissionStates(); 
            Type = BiomeType.NONE;
        }

        private void ConvertEffectsToDictionary()
        {
            foreach (BiomeEffectData item in Effects)
            {
                BiomeEffects.Add(item.Biome, item.Effect);
            }
        }

        private void ConvertBiomeDataToDictionary()
        {
            foreach (BiomeData data in Biomes)
            {
                BiomeAmbiences.Add(data.Biome, data.Volume);
            }
        }

        private void SetEmissionStates()
        {
            foreach (var item in BiomeEffects)
            {
                ToggleWeatherParticle(item.Key, false);
            }
        }
        /// <summary>
        /// Fades the indicated biome in or out.
        /// 
        /// </summary>
        /// <param name="biome">the biome to enact on</param>
        /// <param name="direction"> the direction to fade to (1 - fade in , 0 - fade out)</param>
        /// <returns></returns>
        private IEnumerator FadeBiome(PostProcessVolume biome)
        {
            float t = 0;

            while(biome.weight != 1)
            {
                t += Time.deltaTime / TransitionSpeed;

                if(ActiveBiome != null && ActiveBiome.weight > 0)
                {
                    ActiveBiome.weight = AbsoluteLerp(ActiveBiome.weight, 0, t);
                }
                
                biome.weight = AbsoluteLerp(biome.weight, 1, t);
                yield return null;
            }

            if(ActiveBiome != null )
            {
                ActiveBiome.weight = 0.0f;
            }
            ActiveBiome = biome;
            HasFinishedTransition = true;
        }
        
        private void ToggleWeatherParticle(BiomeType biomeType, bool state)
        {
            if(biomeType == BiomeType.NONE)
            {
                return;
            }
            
            WeatherComponent ActiveComponents = BiomeEffects[biomeType].GetComponent<WeatherComponent>();

            ActiveComponents?.WindZone.gameObject.SetActive(state);

            foreach (var item in ActiveComponents.WeatherParticles)
            {
                item.enableEmission = state;
            }

            foreach (ParticleSystem item in ActiveComponents.AmbienceParticle)
            {
                item.enableEmission = state;
            }
        }
        /// <summary>
        /// Switches the weather particles with a different one.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CrossFadeParticles(BiomeType newbiome)
        {
            ToggleWeatherParticle(Type, false);
            
            yield return new WaitForSeconds(ParticleTransitionTime);

            ToggleWeatherParticle(newbiome, true);


        }
        /// <summary>
        /// Switches to a different biome and sets the destination biome as the active one.
        /// </summary>
        /// <param name="destinationBiome"> the biome to transition to</param>
        public void SwitchToBiome( PostProcessVolume destinationBiome )
        {
            if(HasFinishedTransition)
            {
                StartCoroutine(FadeBiome(destinationBiome));
                OnBiomeChanged?.Invoke(destinationBiome);
                HasFinishedTransition = false;
            }
        }

        public void BoardSwapped(Board board)
        {
            if(board.Biome != Type && board.Biome != BiomeType.RAPTURE)
            {
                SwitchToBiome(BiomeAmbiences[board.Biome]);
                StartCoroutine(CrossFadeParticles(board.Biome));
                Type = board.Biome;
            }
        }

        public void ManualBoardSwap(Board board)
        {
            if(board.Biome != Type)
            {
                SwitchToBiome(BiomeAmbiences[board.Biome]);

                WeatherComponent ActiveComponents = BiomeEffects[Type].GetComponent<WeatherComponent>();

                ActiveComponents?.WindZone.gameObject.SetActive(false);

                foreach (ParticleSystem item in ActiveComponents.WeatherParticles)
                {
                    item.gameObject.SetActive(false); 
                }

                foreach (ParticleSystem item in ActiveComponents.AmbienceParticle)
                {
                    item.gameObject.SetActive(false); 
                }

                ToggleWeatherParticle(board.Biome, true);
                Type = board.Biome;
            }
        }

        public void ManualBoardSwap(BiomeType boardType)
        {
            if(boardType != Type)
            {
                SwitchToBiome(BiomeAmbiences[boardType]);

                WeatherComponent ActiveComponents = BiomeEffects[Type].GetComponent<WeatherComponent>();

                ActiveComponents?.WindZone.gameObject.SetActive(false);

                foreach (ParticleSystem item in ActiveComponents.WeatherParticles)
                {
                    item.gameObject.SetActive(false); 
                }

                foreach (ParticleSystem item in ActiveComponents.AmbienceParticle)
                {
                    item.gameObject.SetActive(false); 
                }

                ToggleWeatherParticle(boardType, true);
                Type = boardType;
            }
        }

        public void PerformInstantSwap(Board board)
        {
            if(board.Biome != Type)
            {
                if(ActiveBiome != null)
                {
                    ActiveBiome.weight = 0;
                }

                BiomeAmbiences[board.Biome].weight = 1f;
                ActiveBiome = BiomeAmbiences[board.Biome];
                ToggleWeatherParticle(Type, false);
                ToggleWeatherParticle(board.Biome, true);
                Type = board.Biome;

                OnBiomeChanged?.Invoke(BiomeAmbiences[board.Biome]);
            }
        }

        private float AbsoluteLerp(float initialTarget, float target , float time)
        {
            return Mathf.Lerp(initialTarget, target, Mathf.SmoothStep(0.0f, 1.0f, time));
        }

#if UNITY_EDITOR
        public void DebugBiomeToQuagmire()
        {
            SwitchToBiome(BiomeAmbiences[BiomeType.QUAGMIRE]);
        }

        public void DebugBiomeToGlacier()
        {
            SwitchToBiome(BiomeAmbiences[BiomeType.GLACIER]);
        }

        public void DebugBiomeToBoneyard()
        {
            SwitchToBiome(BiomeAmbiences[BiomeType.BONEYARD]);
        }
#endif
    }
}
