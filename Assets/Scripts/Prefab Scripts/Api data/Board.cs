using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    // Scene components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Other prefabs
    [SerializeField] Tile tilePrefab;

    // Prefab Settings
    public BoardData boardData;

    // Prefab components
    [SerializeField] GameObject boardBackground;
    
    // Public board
    public List<List<Tile>> tileMatrix;

    // Intern logic
    float size = 1300;
    int dim;

    private void Start()
    {
        boardBackground.SetActive(true);
    }

    public void CreateBoard()
    {
        
        tileMatrix = new List<List<Tile>>();
        dim = (int)Math.Sqrt(boardData.tiles.Count);
        for (int y = 0; y < dim; y++)
        {
            tileMatrix.Add(new List<Tile>());
            for (int x = 0; x < dim; x++)
            {
                GameObject tileGO = Instantiate(tilePrefab.gameObject, transform);
                Tile tile = tileGO.GetComponent<Tile>();
                tile.audioController = audioController;
                tile.tileData = boardData.tiles[x + y * dim];
                tile.ApplyChanges();
                float tilePosX = -size / 2 + size / (dim - 1) * x;
                float tilePosY = size / 2 - size / (dim - 1) * y;
                tile.transform.localPosition = new Vector2(tilePosX, tilePosY);
                tileMatrix[y].Add(tile);
            }
        }
    }

    public void UpdateBoardData(BoardData newBoardData)
    {
        boardData = newBoardData;
        for (int i = 0; i < boardData.tiles.Count; i++)
        {
            Tile tile = i == 0 ? tileMatrix[0][0] : tileMatrix[i / dim][i % dim];
            tile.tileData = newBoardData.tiles[i];
        }
    }

    public void UpdateTileMatrix(int tile1_x, int tile1_y, int tile2_x, int tile2_y)
    {
        Tile tile2Ref = tileMatrix[tile1_y][tile1_x];
        tileMatrix[tile1_y][tile1_x] = tileMatrix[tile2_y][tile2_x];
        tileMatrix[tile2_y][tile2_x] = tile2Ref;
        for (int y = 0; y < dim; y++)
        {
            for (int x = 0; x < dim; x++)
            {
                Tile tile = tileMatrix[y][x];
                float tilePosX = -size / 2 + size / (dim - 1) * x;
                float tilePosY = size / 2 - size / (dim - 1) * y;
                transformUtils.MoveOriginalToTarget(transform: tile.transform,
                    targetPosition: new Vector2(tilePosX, tilePosY),
                    seconds: 0.25f,
                    handleActive: true);
            }
        }
    }

    public void HighlightTiles()
    {
        for (int y = 0; y < dim; y++)
        {
            for (int x = 0; x < dim; x++)
            {
                tileMatrix[y][x].HandleHighLights();
            }
        }
    }

    public void SetTargetTiles(MovType movType, int x, int y)
    {
        List<Vector2> targetTilesPos = MovCard.GetMovTypeTarget(movType, x, y);
        foreach (Vector2 targetTilePos in targetTilesPos)
        {
            bool valid_X = 0 <= targetTilePos.x && targetTilePos.x < dim;
            bool valid_Y = 0 <= targetTilePos.y && targetTilePos.y < dim;
            bool isTargetOrigin = targetTilePos.x == x && targetTilePos.y == y;
            if (valid_X && valid_Y && !isTargetOrigin)
            {
                tileMatrix[(int)targetTilePos.y][(int)targetTilePos.x].SetTarget();
            }
        }
    }

}
