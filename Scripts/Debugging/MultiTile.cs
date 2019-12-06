/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: grizendi@gmail.com | gd54gustavo@vfs.com
*      @Author: Gus Grizendi
*/

using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class MultiTile : MonoBehaviour
{
    [SerializeField]
    private TileType currentTile;
    [SerializeField]
    private PropType currentProp;

    [Header("")]

    //Tiles Setting
    [SerializeField]
    private GameObject[] multiTiles = new GameObject[19];
    private enum TileType
    {
        Orchard, OrchardAlt, OrchardMountainBase,
        GlacierSnow, GlacierIce, GlacierMountainBase,
        QuagmireNormal, QuagmireFlooded, QuagmireMountainBase,
        Rapture, RaptureAlt, RaptureMountainBase,
        Empty
    }

    //Props Setting
    [SerializeField]
    private GameObject[] multiProps = new GameObject[18];
    private enum PropType
    {
        Empty,
        City,
        GiantSkeleton01, GiantSkeleton02,
        Hellbloom,
        Obelisk,
        Pentagram,
        PenTestament,
        IceCube01, IceCube02,
        MountainGlacier01, MountainGlacier02,
        MountainQuagmire01, MountainQuagmire02,
        PlateauOrchard01, PlateauOrchard02,
        MountainRapture01, MountainRapture02
    }

    private void SetActiveTiles(int body, int top)
    {
        foreach (var item in multiTiles) { item.SetActive(false); }
        multiTiles[body].SetActive(true);
        multiTiles[top].SetActive(true);
    }

    private void SetActiveProp(int prop)
    {
        foreach (var item in multiProps) { item.SetActive(false); }
        multiProps[prop].SetActive(true);
    }

    private void Update()
    {
        switch (currentTile)
        {
            case TileType.Orchard:
                SetActiveTiles(0, 1);
                break;

            case TileType.OrchardAlt:
                SetActiveTiles(0, 2);
                break;

            case TileType.OrchardMountainBase:
                SetActiveTiles(0, 3);
                break;

            case TileType.GlacierSnow:
                SetActiveTiles(4, 5);
                break;

            case TileType.GlacierIce:
                SetActiveTiles(6, 7);
                break;

            case TileType.GlacierMountainBase:
                SetActiveTiles(4, 8);
                break;

            case TileType.QuagmireNormal:
                SetActiveTiles(9, 10);
                break;

            case TileType.QuagmireFlooded:
                SetActiveTiles(11, 12);
                break;

            case TileType.QuagmireMountainBase:
                SetActiveTiles(9, 13);
                break;

            case TileType.Rapture:
                SetActiveTiles(14, 15);
                break;

            case TileType.RaptureAlt:
                SetActiveTiles(14, 16);
                break;

            case TileType.RaptureMountainBase:
                SetActiveTiles(14, 17);
                break;

            case TileType.Empty:
                SetActiveTiles(18,18);
                break;

            default:
                break;
        }

        switch (currentProp)
        {
            case PropType.Empty:
                SetActiveProp(0);
                break;

            case PropType.City:
                SetActiveProp(1);
                break;

            case PropType.GiantSkeleton01:
                SetActiveProp(2);
                break;
            
            case PropType.GiantSkeleton02:
                SetActiveProp(3);
                break;
            
            case PropType.Hellbloom:
                SetActiveProp(4);
                break;
            
            case PropType.Obelisk:
                SetActiveProp(5);
                break;

            case PropType.Pentagram:
                SetActiveProp(6);
                break;

            case PropType.PenTestament:
                SetActiveProp(7);
                break;
            
            case PropType.IceCube01:
                SetActiveProp(8);
                break;
            
            case PropType.IceCube02:
                SetActiveProp(9);
                break;
            
            case PropType.MountainGlacier01:
                SetActiveProp(10);
                currentTile = TileType.GlacierMountainBase;
                break;
            
            case PropType.MountainGlacier02:
                SetActiveProp(11);
                currentTile = TileType.GlacierMountainBase;
                break;
            
            case PropType.MountainQuagmire01:
                SetActiveProp(12);
                currentTile = TileType.QuagmireMountainBase;
                break;
            
            case PropType.MountainQuagmire02:
                SetActiveProp(13);
                currentTile = TileType.QuagmireMountainBase;
                break;
            
            case PropType.PlateauOrchard01:
                SetActiveProp(14);
                currentTile = TileType.OrchardMountainBase;
                break;
            
            case PropType.PlateauOrchard02:
                SetActiveProp(15);
                currentTile = TileType.OrchardMountainBase;
                break;
            
            case PropType.MountainRapture01:
                SetActiveProp(16);
                currentTile = TileType.RaptureMountainBase;
                break;
            
            case PropType.MountainRapture02:
                SetActiveProp(17);
                currentTile = TileType.RaptureMountainBase;
                break;

            default:
                break;
        }
    }

}
