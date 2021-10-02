using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
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
    public Image AreaViewSprite;
    public Image AreaCompleteSprite;

    public void Initialize(Grid connectedGrid = null)
    {
        gameObject.SetActive(true);
        ConnectedGrid = connectedGrid;
    }

    public void BuildToConnectedGrid(Building buildingPrefab)
    {
        if (!ConnectedGrid || ConnectedGrid.CurrentBuilding != null || !GridManager.Instance.CheckGridSize(ConnectedGrid, buildingPrefab.GridSize.RowSize, buildingPrefab.GridSize.ColoumnSize, Side))
            return;

        Debug.Log("aaa");
        ConnectedGrid.BuildTheTargetBuilding(buildingPrefab, this);
    }

    public void ChangeSelectedStatus(bool status)
    {
        if (status)
        {
            AreaCompleteSprite.gameObject.SetActive(true);
            AreaCompleteSprite.fillAmount = 0f;
            AreaViewSprite.color = new Color32(255, 244, 0, 255);
        }
        else
        {
            AreaCompleteSprite.gameObject.SetActive(false);
            AreaCompleteSprite.fillAmount = 0f;
            AreaViewSprite.color = new Color32(255, 255, 255, 255);
        }
    }
}
