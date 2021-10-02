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

    public bool CheckGridSize(Grid mainGrid, int rowSize = 1, int coloumnSize = 1)
    {
        if (mainGrid.RowIndex + rowSize - 1 > CurrentGridMap.RowCount - 1 || mainGrid.ColoumnIndex + coloumnSize - 1 > CurrentGridMap.ColumnCount - 1)
            return false;

        for (int r = 0; r < rowSize; r++)
        {
            for (int c = 0; c < coloumnSize; c++)
            {
                Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex + c);
                if(grid != null && grid.CurrentBuilding != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void FillOtherGridsBySize(Building building)
    {
        for (int r = 0; r < building.GridSize.RowSize; r++)
        {
            for (int c = 0; c < building.GridSize.ColoumnSize; c++)
            {
                Grid grid = GetGrid(building.ConnectedGrid.RowIndex + r, building.ConnectedGrid.ColoumnIndex + c);
                grid.CurrentBuilding = building;
            }
        }
    }
}
