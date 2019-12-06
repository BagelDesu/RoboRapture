

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>

[CreateAssetMenu(menuName = "Map/Data")]
public class MapData : ScriptableObject
{
    public int BiomeSize;

    [SerializeField]
    private BoardPoolType[] BiomePool;

    public List<Board> RapturePool = new List<Board>();

    public Board BossBiome;

    public Dictionary<int,List<Board>> ConvertToDictionary()
    {
        Dictionary<int,List<Board>> BoardCollection = new Dictionary<int, List<Board>>();

        foreach(var pool in BiomePool)
        {
            BoardCollection[pool.BoardRating] = pool.Boards;
        }

        return BoardCollection;
    }
}
