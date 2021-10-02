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

    public bool CheckGridSize(Grid mainGrid, int rowSize = 1, int coloumnSize = 1, Sides side = Sides.T)
    {
        Debug.Log("bbb");
        Debug.Log(side);
        if (side == Sides.T)
        {
            if (mainGrid.RowIndex + (rowSize - 1) > CurrentGridMap.RowCount - 1 ||
                mainGrid.ColoumnIndex + ((coloumnSize - 1) / 2) > CurrentGridMap.ColumnCount - 1 || mainGrid.ColoumnIndex - ((coloumnSize - 1) / 2) < 0)
                return false;

            int possitiveC = coloumnSize / 2;
            int neggativeC = coloumnSize / 2;
            if (possitiveC + neggativeC < coloumnSize)
                neggativeC += 1;

            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 1; c <= possitiveC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex + c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }

                for (int c = 0; c < neggativeC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex - c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }
            }
        }
        else if (side == Sides.B)
        {
            if (mainGrid.RowIndex - (rowSize - 1) < 0 ||
                mainGrid.ColoumnIndex + ((coloumnSize - 1) / 2) > CurrentGridMap.ColumnCount - 1 || mainGrid.ColoumnIndex - ((coloumnSize - 1) / 2) < 0)
                return false;

            int possitiveC = coloumnSize / 2;
            int neggativeC = coloumnSize / 2;
            if (possitiveC + neggativeC < coloumnSize)
                neggativeC += 1;

            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 1; c <= possitiveC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex + c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }

                for (int c = 0; c < neggativeC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex - c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }
            }
        }
        else if (side == Sides.L)
        {
            if (mainGrid.ColoumnIndex - (coloumnSize - 1) < 0 ||
                mainGrid.RowIndex + ((rowSize - 1) / 2) > CurrentGridMap.RowCount - 1 || mainGrid.RowIndex - ((rowSize - 1) / 2) < 0)
                return false;

            int possitiveR = rowSize / 2;
            int neggativeR = rowSize / 2;
            if (possitiveR + neggativeR < rowSize)
                neggativeR += 1;

            for (int c = 0; c < coloumnSize; c++)
            {
                for (int r = 1; r <= possitiveR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex - c);
                    Debug.Log(grid.ID + " | " + grid.CurrentBuilding);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }

                for (int r = 0; r < neggativeR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex - c);
                    Debug.Log(grid.ID + " | " + grid.CurrentBuilding);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }
            }
        }
        else if (side == Sides.R)
        {
            if (mainGrid.ColoumnIndex + (coloumnSize - 1) > CurrentGridMap.ColumnCount - 1 ||
                mainGrid.RowIndex + ((rowSize - 1) / 2) > CurrentGridMap.RowCount - 1 || mainGrid.RowIndex - ((rowSize - 1) / 2) < 0)
                return false;

            int possitiveR = rowSize / 2;
            int neggativeR = rowSize / 2;
            if (possitiveR + neggativeR < rowSize)
                neggativeR += 1;

            for (int c = 0; c < coloumnSize; c++)
            {
                for (int r = 1; r <= possitiveR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex + c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }

                for (int r = 0; r < neggativeR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex + c);
                    if (grid != null && grid.CurrentBuilding != null)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public void FillOtherGridsBySize(Building building, Sides side = Sides.T)
    {
        Grid mainGrid = building.ConnectedGrid;
        int rowSize = building.GridSize.RowSize;
        int coloumnSize = building.GridSize.ColoumnSize;

        if (side == Sides.T)
        {
            int possitiveC = coloumnSize / 2;
            int neggativeC = coloumnSize / 2;
            if (possitiveC + neggativeC < coloumnSize)
                neggativeC += 1;

            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 1; c <= possitiveC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex + c);
                    grid.CurrentBuilding = building;
                }

                for (int c = 0; c < neggativeC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex - c);
                    grid.CurrentBuilding = building;
                }
            }
        }
        else if (side == Sides.B)
        {
            int possitiveC = coloumnSize / 2;
            int neggativeC = coloumnSize / 2;
            if (possitiveC + neggativeC < coloumnSize)
                neggativeC += 1;

            for (int r = 0; r < rowSize; r++)
            {
                for (int c = 1; c <= possitiveC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex + c);
                    grid.CurrentBuilding = building;
                }

                for (int c = 0; c < neggativeC; c++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex - c);
                    grid.CurrentBuilding = building;
                }
            }
        }
        else if (side == Sides.L)
        {
            int possitiveR = rowSize / 2;
            int neggativeR = rowSize / 2;
            if (possitiveR + neggativeR < rowSize)
                neggativeR += 1;

            for (int c = 0; c < coloumnSize; c++)
            {
                for (int r = 1; r <= possitiveR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex - c);
                    grid.CurrentBuilding = building;
                }

                for (int r = 0; r < neggativeR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex - c);
                    grid.CurrentBuilding = building;
                }
            }
        }
        else if (side == Sides.R)
        {
            int possitiveR = rowSize / 2;
            int neggativeR = rowSize / 2;
            if (possitiveR + neggativeR < rowSize)
                neggativeR += 1;

            for (int c = 0; c < coloumnSize; c++)
            {
                for (int r = 1; r <= possitiveR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex + r, mainGrid.ColoumnIndex + c);
                    grid.CurrentBuilding = building;
                }

                for (int r = 0; r < neggativeR; r++)
                {
                    Grid grid = GetGrid(mainGrid.RowIndex - r, mainGrid.ColoumnIndex + c);
                    grid.CurrentBuilding = building;
                }
            }
        }
    }
}
