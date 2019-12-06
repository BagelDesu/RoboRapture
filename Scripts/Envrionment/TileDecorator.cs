

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using Edu.Vfs.RoboRapture.TileAuxillary;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class TileDecorator : MonoBehaviour
{
    [SerializeField]
    private ATileDecoration                         Decoration;

    [HideInInspector]
    public List<Material>                           AppliedMaterials = new List<Material>();
    public TileStates                               AppliedState {get; private set;} 
    public RefInt                                   TileFinishedRapture;

    [SerializeField]
    public Renderer[]                               TileComponents;
    [HideInInspector]
    public Point                                    TilePos;
    
    // Shader Values
    public float[]                                  RaptureValues;
    
    public int                                      RaptureValueId          {get; private set;}
    public int                                      BurnValueId             {get; private set;}
    public int                                      FresnelEffect           {get; private set;}
    public int                                      PatternSeed             {get; private set;}
    public int                                      DecalTexture            {get; private set;}
    
    public MaterialPropertyBlock                    MaterialProperties      {get; private set;}

    private Material[]                              OriginalMaterials;

    // Shader Properties
    //TODO: Carve off into decoration
    [SerializeField][Tooltip("How long does it take to rapturize this unit?")] 
    private float Seconds = 5f;

    private void Awake()
    {
        if(TileComponents.Length <= 0)
        {
            TileComponent[] tileComponents = GetComponentsInChildren<TileComponent>();
            TileComponents = new MeshRenderer[tileComponents.Length];
            OriginalMaterials = new Material[tileComponents.Length];

            for (int i = 0; i < tileComponents.Length; i++)
            {
                TileComponents[i] = tileComponents[i].gameObject.GetComponent<MeshRenderer>();
                OriginalMaterials[i] = TileComponents[i].material;
            }
        }

        ApplyMaterialState(TileMaterials.REVERT);

        // START SHADER SETUP
        
        MaterialProperties      = new MaterialPropertyBlock();
        RaptureValues           = new float[TileComponents.Length];
        RaptureValueId          = Shader.PropertyToID("_AscensionAmount");
        BurnValueId             = Shader.PropertyToID("_Threshold");
        FresnelEffect           = Shader.PropertyToID("_FresnelState");
        PatternSeed             = Shader.PropertyToID("_PatternSeed");
        DecalTexture            = Shader.PropertyToID("_DecalTexture");

        for (int i = 0; i < TileComponents.Length; i++)
        {
            MaterialProperties.SetFloat(RaptureValueId, 0);
            TileComponents[i].SetPropertyBlock(MaterialProperties);
        }

        // END SHADER SETUP
    }

    public void SetDecorator(ATileDecoration decor)
    {
        Decoration = decor;
    }

    public void ApplyMaterialState(TileMaterials state)
    {
        if(AppliedMaterials == null 
        || OriginalMaterials == null 
        || TileComponents == null
        || TileComponents.Length == 0)
        {
            return;
        }

        if(state == TileMaterials.REVERT)
        {
            for (int i = 0; i < TileComponents.Length; i++)
            {
                AppliedMaterials.Clear();
                Decoration?.ApplyMaterial(TileMaterials.REVERT, AppliedMaterials);
                foreach (MeshRenderer rend in TileComponents)
                {
                    if(!rend.name.Contains("Body"))
                    {
                        rend.materials = AppliedMaterials.ToArray();
                        continue;
                    }

                    rend.material = OriginalMaterials[0];
                }
            }
            return;
        }

        AppliedMaterials.Clear();
        Decoration.ApplyMaterial(state, AppliedMaterials);

        // if(state == TileMaterials.RAISE)
        // {
        //     if(TileComponents.Length >= 2)
        //     {
        //         foreach (MeshRenderer rend in TileComponents)
        //         {
        //             if(!rend.name.Contains("Body"))
        //             {
        //                 rend.materials = AppliedMaterials.ToArray();
        //             }
        //         }
        //         return;
        //     }
        // }
        
        foreach (MeshRenderer rend in TileComponents)
        {
            rend.materials = AppliedMaterials.ToArray();
        }
    }

    public void ApplyTileState(TileStates state)
    {
        AppliedState = state;
        foreach (Renderer item in TileComponents)
        {
            if(Decoration == null)
            {
                Debug.Log($"[TILE DECORATOR] {state}, {name}", this);
            }
            MaterialProperties?.SetTexture(DecalTexture, Decoration?.GetStateTexture(state));
            item.SetPropertyBlock(MaterialProperties);
        }
    }

    public void RaiseInstant()
    {
        for (int i = 0; i < TileComponents.Length; i++)
        {
            RaptureValues[i] = 1;
            MaterialProperties.SetFloat(RaptureValueId, RaptureValues[i]);
            MaterialProperties.SetFloat(BurnValueId, RaptureValues[i]);
            MaterialProperties.SetFloat(FresnelEffect, 1);
            TileComponents[i].SetPropertyBlock(MaterialProperties);
        }
    }

    //TODO: Remove this part from TileDecorator into it's own handler.
    // RAPTURE SHADER

    /// <summary>
    /// Raises the component's shader's ascension value.
    /// </summary>
    public void RaiseComponents()
    {
        if(this.isActiveAndEnabled)
        {
            StartCoroutine(ComponentUpdate(TileAscensionDirection.UP));
        }
    }
    /// <summary>
    /// Lowers the component's shader's ascension value.
    /// </summary>
    public void SinkComponents()
    {
        if(this.isActiveAndEnabled)
        {
            StartCoroutine(ComponentUpdate(TileAscensionDirection.DOWN));
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
    private IEnumerator ComponentUpdate(TileAscensionDirection direction)
    {
        yield return new WaitForSeconds(.13f * Random.Range(0 ,TilePos.z));

        float t = 0;

        bool isFinished = true;

        MaterialProperties.SetFloat(PatternSeed, Random.Range(1f,2f));

        while(isFinished)
        {  
            t += Time.deltaTime/Seconds;

            for (int i = 0; i < TileComponents.Length; i++)
            {
                RaptureValues[i] = Mathf.Lerp(RaptureValues[i], (int)direction, Mathf.SmoothStep(0.0f, 1.0f,t));
                MaterialProperties.SetFloat(RaptureValueId, RaptureValues[i]);
                MaterialProperties.SetFloat(BurnValueId, RaptureValues[i]);
                MaterialProperties.SetFloat(FresnelEffect, (int)direction);
                TileComponents[i].SetPropertyBlock(MaterialProperties);
            }
            foreach (float value in RaptureValues)
            {                                           //true  | //false
                isFinished = ( (value == (int)direction) ? false : true);
            }
            yield return null;
        }

        if(direction == TileAscensionDirection.UP && TileFinishedRapture != null)
        {
            TileFinishedRapture.Value++;
        }

        // ApplyMaterialState(TileMaterials.REVERT);
    }
}