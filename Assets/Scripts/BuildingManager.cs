using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public Building[] AllBuildingPrefabs;

    public Grid SelectedGrid;
    public BuildingArea SelectedBuildingArea;
    public Building SelectedBuilding;

    public Building GetBuildPrefabByTypeID(string typeID)
    {
        foreach(Building buildingPrefab in AllBuildingPrefabs)
        {
            if(buildingPrefab.TypeID == typeID)
            {
                return buildingPrefab;
            }
        }

        return null;
    }

    public void ChangeSelectedGrid(Grid grid = null)
    {
        if (SelectedGrid != null)
            SelectedGrid.ChangeVisibilityOfBuildingAreas();
        SelectedGrid = grid;
    }

    public void ChangeSelectedBuildingArea(BuildingArea buildingArea = null)
    {
        if(SelectedBuildingArea != null)
            SelectedBuildingArea.AreaViewSprite.color = new Color32(255, 255, 255, 255);
        SelectedBuildingArea = buildingArea;
        SelectedBuildingArea.AreaViewSprite.color = new Color32(255, 244, 0, 255);
    }

    public void ChangeSelectedBuilding(string typeID)
    {
        SelectedBuilding = GetBuildPrefabByTypeID(typeID);
    }

    public void BuildTheSelectedBuildingToSelectedArea()
    {
        if(SelectedBuildingArea && SelectedBuilding)
            SelectedBuildingArea.BuildToConnectedGrid(SelectedBuilding);
    }

    public void LevelUpTheBuildingFromSelectedBuildingArea()
    {
        if (SelectedBuildingArea && SelectedBuildingArea.ConnectedGrid && SelectedBuildingArea.ConnectedGrid.CurrentBuilding)
            SelectedBuildingArea.ConnectedGrid.CurrentBuilding.LevelUp();
    }
}
