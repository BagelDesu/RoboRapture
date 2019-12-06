

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.TurnSystem;
using UnityEngine;

namespace Edu.Vfs.RoboRapture.GrandFinale
{    
    ///<summary>
    ///-summary of script here-
    ///</summary>
    public class SoulSphereController : MonoBehaviour
    {
        public MeshRenderer Sphere;
        public EntityTurnManager etm;

        private MaterialPropertyBlock block;
        [SerializeField]
        private float bgalpha = 0;

        private int bgalphaPropID;

        [SerializeField]
        private float speed;

        private void Start()
        {
            etm.OnTurnEnd += BossSequence;

            block = new MaterialPropertyBlock();
            bgalphaPropID = Shader.PropertyToID("_TransitionAlpha");

            Sphere.SetPropertyBlock(block);

            block.SetFloat(bgalphaPropID, 0);
        }


        private void BossSequence(TurnEntities entities)
        {
            StopAllCoroutines();

            if(entities == TurnEntities.ENEMY)
            {
                StartCoroutine(StartTransition(1));
            }

            if(entities == TurnEntities.PLAYER)
            {
                StartCoroutine(StartTransition(0));
            }
        }

        private IEnumerator StartTransition(int dest)
        {
            float t = 0f;
            
            while (bgalpha != dest)
            {
                t += Time.deltaTime / speed;
                bgalpha = AbsoluteLerp(bgalpha, (float)dest, t);
                block.SetFloat(bgalphaPropID, (float)bgalpha);
                Sphere.SetPropertyBlock(block);
                yield return null;
            }
        }

        private float AbsoluteLerp(float initialTarget, float target , float time)
        {
            return Mathf.Lerp(initialTarget, target, Mathf.SmoothStep(0.0f, 1.0f, time));
        }
    }
}
