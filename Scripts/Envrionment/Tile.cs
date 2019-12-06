

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edu.Vfs.RoboRapture.GridSystem;
using Edu.Vfs.RoboRapture.TerrainSystem;
using Edu.Vfs.RoboRapture.Base;
using Edu.Vfs.RoboRapture.DataTypes;
using Edu.Vfs.RoboRapture.TileAuxillary;
using Edu.Vfs.RoboRapture.ScriptableLibrary;

///<summary>
/// Handles the description and interfacing of Tile components.
/// <remarks> Contains AddDecoration(), RemoveDecoration() and a reference to the TerrainType the tile is. </remarks>
///</summary>
[SelectionBase]
[RequireComponent(typeof(RoboRaptureTerrain))]
public class Tile : GridLockedEntity, ISelectable
{
    [HideInInspector]
    public Board                                    ParentBoard;
    public TileDecorator                            Decorator   {get; private set;}
    public RoboRaptureTerrain                       TerrainType {get; private set;}
    private Collider                                BCollider;

    [SerializeField]
    private RefInt                                  TileLoader;

    [SerializeField]
    private Sprite image;

    [SerializeField]
    private string tileName;

    [SerializeField]
    private string description;

    [SerializeField]
    private TileDescriptions TDesc;

    public Sprite Image { get => this.image; set => this.image = value; }

    public string TileName { get => this.tileName; set => this.tileName = value; }

    public string Description { get => this.description; set => this.description = value; }

    /// <summary>
    ///  Sets up the Tile.
    /// </summary>
    private void Awake()
    {
        // set up collider
        BCollider = GetComponent<Collider>();
        BCollider.enabled = false;
        //Set up decorator
        Decorator = GetComponent<TileDecorator>();
        Decorator.TilePos = EntityPosition;
        // set up terrain
        TerrainType = GetComponent<RoboRaptureTerrain>();

        // if(TileLoader != null)
        // {
        //     TileLoader.Value++;
        // }
    }
    /// <summary>
    /// Returns the position of the tile in World Space.
    /// </summary>
    /// <returns>Point</returns>
    public Point GetPosition()
    {
        return new Point(EntityPosition.x + (ParentBoard.BoardDimensions.x * ParentBoard.BoardOffset), EntityPosition.y, EntityPosition.z);
    }

    public void SwitchDescription(DescriptionTypes types)
    {
        switch (types)
        {
            case DescriptionTypes.RAPTURE:

                image = TDesc.RaptureImage;
                description = TDesc.RaptureDescription;

            break;
            case DescriptionTypes.LEG:

                image = TDesc.LegStompImage;
                description = TDesc.LegStompDescription;

            break;
            case DescriptionTypes.LIGHTNING:

                image = TDesc.LightningImage;
                description = TDesc.LightningDescription;

            break;
            case DescriptionTypes.SIGIL:

                image = TDesc.SigilImage;
                description = TDesc.SigilDescription;

            break;
            case DescriptionTypes.PENTAGRAM:

                image = TDesc.PentagramImage;
                description = TDesc.PentagramDescription;

            break;
            default:

                image = TDesc.NormalImage;
                description = TDesc.NormalDescription;

            break;
        }

    }

    public void RaiseComponents()
    {
        BCollider.enabled = true;
        Decorator.RaiseComponents();
    }

    public void SinkComponents()
    {
        BCollider.enabled = false;
        Decorator.SinkComponents();
    }
}