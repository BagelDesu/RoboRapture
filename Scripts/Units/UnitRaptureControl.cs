

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Edu.Vfs.RoboRapture.Units
{
    ///<summary>
    ///-Controlls how the unit is spawned with the shader effect-
    ///</summary>
    public class UnitRaptureControl : MonoBehaviour
    {
        [SerializeField]
        public Renderer                                 SpriteComponent;
        [SerializeField]
        private Material                                NormalMaterial;
        
        private float                                   RaptureValue;
        public int                                      RaptureValueId          {get; private set;}
        public MaterialPropertyBlock                    MaterialProperties      {get; private set;}

        private StateRenderer SRenderer;

        [SerializeField][Tooltip("How long does it take to rapturize this unit?")] 
        private float Seconds = 5f;

        private void Awake()
        {
            SRenderer = GetComponent<StateRenderer>();

            // START SHADER SETUP
            
            MaterialProperties      = new MaterialPropertyBlock();
            RaptureValueId          = Shader.PropertyToID("_AscensionAmount");

            MaterialProperties.SetFloat(RaptureValueId, 0);
            SpriteComponent.SetPropertyBlock(MaterialProperties);

            // END SHADER SETUP
        }

        private void OnEnable()
        {
            RaiseComponents();
        }

        private void OnDisable()
        {
            SinkComponents();
        }

        /// <summary>
        /// Raises the component's shader's ascension value.
        /// </summary>
        public void RaiseComponents()
        {
            if(this.isActiveAndEnabled)
            {
                StartCoroutine(ComponentUpdate(1));
            }
        }
        /// <summary>
        /// Lowers the component's shader's ascension value.
        /// </summary>
        public void SinkComponents()
        {
            if(this.isActiveAndEnabled)
            {
                StartCoroutine(ComponentUpdate(0));
            }
        }
        /// <summary>
        /// 
        /// Handle's the smooth ascension/descent of the shader.
        /// 
        /// TODO abstract into a different class.
        /// 
        /// </summary>
        /// <param name="direction">UP OR DOWN</param>
        /// <returns></returns>
        private IEnumerator ComponentUpdate(int direction)
        {
            float t = 0;

            bool isFinished = true;

            while(isFinished)
            {  
                t += Time.deltaTime/Seconds;
                
                RaptureValue = Mathf.Lerp(RaptureValue, direction, Mathf.SmoothStep(0.0f, 1.0f,t));
                MaterialProperties.SetFloat(RaptureValueId, RaptureValue);
                SpriteComponent.SetPropertyBlock(MaterialProperties);
                
                isFinished = ((RaptureValue == (int)direction) ? false : true);
                yield return null;
            }

            SRenderer?.ChangeNormalAndDisabledMaterials(NormalMaterial);
        }
    }
}