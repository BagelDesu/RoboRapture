

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.TileAuxillary;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
[CreateAssetMenu(menuName="Decorations/Glacier")]
public class GlacierDecoration : ATileDecoration
{
    [SerializeField]
    private Material           OriginalMaterial;
    [SerializeField]
    private Material           SinkBurnMaterial;
    [SerializeField]
    private Material           DecalMaterial;

    [SerializeField]
    private Texture2D          NormalDecal;

    [SerializeField]
    private Texture2D          BaseHighlight;
    [SerializeField]
    private Texture2D          ActiveHighlight;
    [SerializeField]
    private Texture2D          SelectionHighlight;

    [SerializeField]
    private Texture2D          BaseAttack;
    [SerializeField]    
    private Texture2D          ActiveAttack;
    [SerializeField]
    private Texture2D          SelectionAttack;

    [SerializeField]
    private Texture2D          RaptureDecalHighlight;

    public override Texture GetStateTexture(TileStates state)
    {
        switch (state)
        {
            case TileStates.NORMAL:
                return NormalDecal;
            case TileStates.ATTACK:
                return BaseAttack;
            case TileStates.HIGHLIGHT:
                return BaseHighlight;
            case TileStates.TARGET:
                return SelectionAttack;
            case TileStates.BaseHighlight:
                return BaseHighlight;
            case TileStates.ActiveHighlight:
                return ActiveHighlight;
            case TileStates.SelectionHighlight:
                return SelectionHighlight;
            case TileStates.BaseAttack:
                return BaseAttack;
            case TileStates.ActiveAttack:
                return ActiveAttack;
            case TileStates.SelectionAttack:
                return SelectionAttack;
            default:
                return NormalDecal;
        }
    }
    
    public override void ApplyMaterial(TileMaterials state, List<Material> appliedMaterials)
    {
        switch (state)
        {
            case TileMaterials.REVERT:
                appliedMaterials.Add(OriginalMaterial);
                appliedMaterials.Add(DecalMaterial);
                break;
            case TileMaterials.SINK:
                appliedMaterials.Add(SinkBurnMaterial);
                appliedMaterials.Add(DecalMaterial);
                break;
            default:
                return;
        }
    }
}
