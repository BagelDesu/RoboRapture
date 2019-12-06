

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

namespace Edu.Vfs.RoboRapture.TileAuxillary
{
    // TODO: Create a seperate enum for Highlights vs. Material set.
    public enum TileStates
    {
        HIGHLIGHT,
        NORMAL,
        ATTACK,
        TARGET,
        BaseHighlight,
        ActiveHighlight,
        SelectionHighlight,
        BaseAttack,
        ActiveAttack,
        SelectionAttack
    }

    public enum TileMaterials
    {
        SINK,
        REVERT,
        // RAISE
    }

    public enum TileAscensionDirection
    {
        DOWN = 0,
        UP = 1,
    }

    public enum DescriptionTypes
    {
        RAPTURE,
        LEG,
        LIGHTNING,
        NORMAL,
        PENTAGRAM,
        SIGIL
    }
}

namespace Edu.Vfs.RoboRapture.TerrainSystem
{
    /// <summary>
    /// Represents the different types of navigation.
    /// <para>| WALK |</para>
    /// <para>FLY  |</para>
    /// <para>BOTH |</para>
    /// <para>NONE |</para>
    /// 
    /// </summary>
    public enum TerrainNavigationType
    {
        BOTH,
        NONE
    }

    /// <summary>
    ///   Represents the different types of status effects.
    /// <para>| ICELAKE |</para>
    /// <para>  OPEN  |</para>
    /// </summary>
    public enum TileType
    {
        OPEN,
        ICELAKE,
        FLOODED
    }
}