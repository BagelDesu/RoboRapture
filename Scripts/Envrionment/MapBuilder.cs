

/*
*      @Copyright: (c) 2019 All Rights Reserved
*      @Company: VFS ROBORAPTURE
*      @Contact: maquinodesign@gmail.com | pg15miguel@vfs.com
*      @Author: Carlos Miguel Aquino
*/

using System.Collections;
using System.Collections.Generic;
using Edu.Vfs.RoboRapture.ScriptableLibrary;
using UnityEngine;

///<summary>
///-summary of script here-
///</summary>
public class MapBuilder : MonoBehaviour
{

    [SerializeField]
    private MapData[] MapPool;
    private MapData Data;

    [SerializeField]
    private RefMapData preLoadedData;

    private int BiomeSize = 1;
    public int BoardIndex;
    private Dictionary<int, List<Board>> BiomeCollection = new Dictionary<int, List<Board>>();

    public List<Board> GeneratedBoard {get; private set;} = new List<Board>();

    [SerializeField]
    private RefInt targetTileLoads;
    
    private void Awake()
    {
        SetUpData();
    }

    private void SetUpData()
    {
        if(MapPool.Length <= 0 && preLoadedData.mapData == null)
        {
            Debug.LogError("No data loaded inside of the MapBuilder. No map will be generated.");
            return;
        }

        if(preLoadedData.mapData == null)
        {
            if(MapPool.Length < 2)
            {
                Data = MapPool[0];
            }

            int rand = Random.Range(0, MapPool.Length);
            // Debug.Log(rand);
            Data = MapPool[rand];
        }
        else
        {
            Data = preLoadedData.mapData;
        }
        

        BiomeSize = Data.BiomeSize;
        BiomeCollection = Data.ConvertToDictionary();

        BuildMap();
    }

    public void BuildMap()
    {
        Board generated = null;

        for (int i = 0; i < BiomeSize; i++)
        {
            generated = CreateRandomBoardFromRating(i, BiomeCollection);
            if(generated != null)
            {
                GeneratedBoard.Add(generated);
            }
        }

        generated = CreateRandomBoardFromPool(Data.RapturePool);

        if(generated != null)
        {
            GeneratedBoard.Add(generated);
        }

        if(Data.BossBiome != null)
        {
            generated = CreateBoardFromPrefab(Data.BossBiome);
        }
        
        if(generated != null)
        {
            GeneratedBoard.Add(generated);
        }

        
        targetTileLoads.Value = GeneratedBoard.Count * 8 * 8;
    }

    private Board CreateRandomBoardFromRating(int rating, Dictionary<int, List<Board>> pool)
    {
        if(BiomeCollection[rating].Count < 1)
        {
            Debug.LogError("Board Rating has no boards in it.");
            return null;
        }

        int randomIndex = Random.Range(0, pool[rating].Count);
        GameObject createdBoard = Instantiate(pool[rating][randomIndex].gameObject);
        Board createdBoardClass = createdBoard.GetComponent<Board>();
        AdjustBoardPosition(createdBoardClass);
        return createdBoardClass;
    }

    private Board CreateRandomBoardFromPool(List<Board> pool)
    {
        if(pool.Count < 1)
        {
            return null;
        }

        int randomIndex = Random.Range(0, pool.Count);
        GameObject createdBoard = Instantiate(pool[randomIndex].gameObject);
        Board createdBoardClass = createdBoard.GetComponent<Board>();
        AdjustBoardPosition(createdBoardClass);
        return createdBoardClass;

    }

    private Board CreateBoardFromPrefab(Board prefab)
    {
        GameObject createdBoard = Instantiate(prefab.gameObject);
        Board createdBoardClass = createdBoard.GetComponent<Board>();
        AdjustBoardPosition(createdBoardClass);
        return createdBoardClass;
    }

    private void AdjustBoardPosition(Board board)
    {

        if(GeneratedBoard.Count <= 0)
        {
            board.gameObject.transform.position = new Vector3(0 ,0, 0);
            return;
        }

        BoardIndex++;
        // Debug.Log("[MAP BUILDER] " + BoardIndex);
        board.BoardOffset = BoardIndex;
        Vector3 previousBoard = GeneratedBoard[GeneratedBoard.Count - 1].gameObject.transform.position;
        board.transform.position = new Vector3(previousBoard.x + board.BoardDimensions.x, 0, 0);
    }
}
