using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sides
{
    T,
    L,
    R,
    B
}
public class BuildingArea : MonoBehaviour
{
    public Grid ConnectedGrid;
    public Sides Side;

    public GameObject AreaViewGroup;
    public SpriteRenderer AreaViewSprite;

    public void Initialize(Grid connectedGrid = null)
    {
        gameObject.SetActive(true);
        ConnectedGrid = connectedGrid;
    }

    public void BuildToConnectedGrid(Building buildingPrefab)
    {
        if (!ConnectedGrid || ConnectedGrid.CurrentBuilding != null || !GridManager.Instance.CheckGridSize(ConnectedGrid, buildingPrefab.GridSize.RowSize, buildingPrefab.GridSize.ColoumnSize))
            return;

        ConnectedGrid.BuildTheTargetBuilding(buildingPrefab, this);
    }
}
