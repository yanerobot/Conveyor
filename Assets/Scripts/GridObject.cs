using UnityEngine;
using System.Collections.Generic;

public class GridObject : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] float cellSize;
    [SerializeField] bool drawGrid;


    [HideInInspector]
    public TextMesh[,] textObjects;
    public static CustomGrid grid;
    void Awake()
    {
        grid = new CustomGrid(width, height, cellSize, transform.position, this);
        if (drawGrid)
        {
            textObjects = new TextMesh[width, height];
            DrawGrid();
        }
    }

    void DrawGrid()
    {
        var color = Color.yellow;
        Debug.DrawLine(grid.GetCellWorldPos(0, height), grid.GetCellWorldPos(width, height), color, 400);
        Debug.DrawLine(grid.GetCellWorldPos(width, height), grid.GetCellWorldPos(width, 0), color, 400);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = grid.GetCellWorldPos(x, y);
                var text = Extensions.CreateWorldText(grid.GetValue(x, y).ToString(), pos.AddTo(x: cellSize / 2, z: cellSize / 2), true);
                textObjects[x, y] = text;
                Debug.DrawLine(pos, pos.AddTo(z: cellSize), color, 400);
                Debug.DrawLine(pos, pos.AddTo(x: cellSize), color, 400);
            }
        }
    }

}
