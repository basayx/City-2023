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

    public Road FirstRoad;
    public Building[] AllBuildingPrefabs;

    public Building SelectedBuilding;
    public BuildingArea SelectedBuildingArea;

    private void Start()
    {
        FirstRoad.Initialize("0");
    }

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

    public void ChangeSelectedBuildingArea(BuildingArea buildingArea = null)
    {
        SelectedBuildingArea = buildingArea;
    }

    public void ChangeSelectedBuilding(string typeID)
    {
        SelectedBuilding = GetBuildPrefabByTypeID(typeID);
    }

    public void BuildTheSelectedBuildingToSelectedArea()
    {
        SelectedBuildingArea.BuildTheTargetBuilding(SelectedBuilding);
    }
}
