using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public GridMapSO CurrentGridMap;
    public List<Grid> Grids = new List<Grid>();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (CurrentGridMap != null)
            Initialize(CurrentGridMap);
    }

    public void Initialize(GridMapSO gridMapSO)
    {
        CurrentGridMap = gridMapSO;

        foreach (GridMapSO.ReadyGridInfo readyGridInfo in CurrentGridMap.ReadyGridInfos)
        {
            int gridIndex = GetGridIndex(readyGridInfo.ColoumnAndRow.y, readyGridInfo.ColoumnAndRow.x);
            DataManager.Instance.SaveBuildingID(gridIndex.ToString(), readyGridInfo.BuildingPrefab.TypeID);
            string side = "T";
            if (readyGridInfo.Side == Sides.T)
                side = "T";
            else if (readyGridInfo.Side == Sides.B)
                side = "B";
            else if (readyGridInfo.Side == Sides.L)
                side = "L";
            else if (readyGridInfo.Side == Sides.R)
                side = "R";
            DataManager.Instance.SaveBuildingCreationSideInfo(DataManager.Instance.GetSavedBuildingID(gridIndex.ToString()), side);
            DataManager.Instance.SaveBuildingLevel(gridIndex.ToString(), readyGridInfo.BuildingLevel);
        }

        for (int r = 0; r < CurrentGridMap.RowCount; r++)
        {
            for (int c = 0; c < CurrentGridMap.ColumnCount; c++)
            {
                Grid grid = Instantiate(CurrentGridMap.GridPrefab, transform);
                grid.transform.position = new Vector3(c * CurrentGridMap.GridPrefab.transform.localScale.x, 0, r * CurrentGridMap.GridPrefab.transform.localScale.z);
                grid.RowIndex = r;
                grid.ColoumnIndex = c;
                grid.Initialize(Grids.Count.ToString());
                Grids.Add(grid);
            }
        }

        transform.position = new Vector3(CurrentGridMap.GridPrefab.transform.localScale.x / 2f * CurrentGridMap.ColumnCount * -1, transform.position.y, CurrentGridMap.GridPrefab.transform.localScale.z / 2f * CurrentGridMap.RowCount * -1);

        foreach(Grid grid in Grids)
        {
            grid.CheckNeighborGrids(true);
        }

        foreach (Grid grid in Grids)
        {
            if(grid.CurrentBuilding)
                grid.CurrentBuilding.UpdateLevelView();
        }
    }

    public Grid GetGrid(int rowIndex, int coloumnIndex)
    {
        if (rowIndex > CurrentGridMap.RowCount - 1 || rowIndex < 0 || coloumnIndex > CurrentGridMap.ColumnCount - 1 || coloumnIndex < 0)
            return null;

        int index = (rowIndex * (CurrentGridMap.ColumnCount) + coloumnIndex);
        return Grids[index];
    }

    public int GetGridIndex(int rowIndex, int coloumnIndex)
    {
        if (rowIndex > CurrentGridMap.RowCount - 1 || rowIndex < 0 || coloumnIndex > CurrentGridMap.ColumnCount - 1 || coloumnIndex < 0)
            return -1;

        int index = (rowIndex * (CurrentGridMap.ColumnCount) + coloumnIndex);
        return index;
    }

    public bool CheckGridSize(Grid mainGrid, List<Vector2Int> coloumnsAndRows, Sides side = Sides.T)
    {
        if(side == Sides.T)
        {
            for(int i = 0; i < coloumnsAndRows.Count; i++)
            {
                int row = mainGrid.RowIndex + coloumnsAndRows[i].y;
                int coloumn = mainGrid.ColoumnIndex + coloumnsAndRows[i].x;
                Grid grid = GetGrid(row, coloumn);

                if (grid == null)
                    return false;
                if (grid.CurrentBuilding)
                    return false;
            }
        }
        else if (side == Sides.B)
        {
            for (int i = 0; i < coloumnsAndRows.Count; i++)
            {
                int row = mainGrid.RowIndex - coloumnsAndRows[i].y;
                int coloumn = mainGrid.ColoumnIndex - coloumnsAndRows[i].x;
                Grid grid = GetGrid(row, coloumn);

                if (grid == null)
                    return false;
                if (grid.CurrentBuilding)
                    return false;
            }
        }
        else if (side == Sides.L)
        {
            for (int i = 0; i < coloumnsAndRows.Count; i++)
            {
                int row = mainGrid.RowIndex + coloumnsAndRows[i].x;
                int coloumn = mainGrid.ColoumnIndex - coloumnsAndRows[i].y;
                Grid grid = GetGrid(row, coloumn);

                if (grid == null)
                    return false;
                if (grid.CurrentBuilding)
                    return false;
            }
        }
        else if (side == Sides.R)
        {
            for (int i = 0; i < coloumnsAndRows.Count; i++)
            {
                int row = mainGrid.RowIndex - coloumnsAndRows[i].x;
                int coloumn = mainGrid.ColoumnIndex + coloumnsAndRows[i].y;
                Grid grid = GetGrid(row, coloumn);

                if (grid == null)
                    return false;
                if (grid.CurrentBuilding)
                    return false;
            }
        }

        return true;
    }

    public void FillOtherGridsBySize(Building building, Sides side = Sides.T)
    {
        Grid mainGrid = building.ConnectedGrid;
        List<Vector2Int> gridSize = new List<Vector2Int>(building.ColoumnsAndRows);

        if (side == Sides.T)
        {
            for (int i = 0; i < gridSize.Count; i++)
            {
                int row = mainGrid.RowIndex + gridSize[i].y;
                int coloumn = mainGrid.ColoumnIndex + gridSize[i].x;

                Grid grid = GetGrid(row, coloumn);
                grid.CurrentBuilding = building;
            }
        }
        else if (side == Sides.B)
        {
            for (int i = 0; i < gridSize.Count; i++)
            {
                int row = mainGrid.RowIndex - gridSize[i].y;
                int coloumn = mainGrid.ColoumnIndex - gridSize[i].x;

                Grid grid = GetGrid(row, coloumn);
                grid.CurrentBuilding = building;
            }
        }
        else if (side == Sides.L)
        {
            for (int i = 0; i < gridSize.Count; i++)
            {
                int row = mainGrid.RowIndex + gridSize[i].x;
                int coloumn = mainGrid.ColoumnIndex - gridSize[i].y;

                Grid grid = GetGrid(row, coloumn);
                grid.CurrentBuilding = building;
            }
        }
        else if (side == Sides.R)
        {
            for (int i = 0; i < gridSize.Count; i++)
            {
                int row = mainGrid.RowIndex - gridSize[i].x;
                int coloumn = mainGrid.ColoumnIndex + gridSize[i].y;

                Grid grid = GetGrid(row, coloumn);
                grid.CurrentBuilding = building;
            }
        }
    }
}
