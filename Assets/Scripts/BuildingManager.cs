﻿using System.Collections;
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
    public BuildingUIButton SelectedBuildingUIButton;

    public float BuildDelay = 1f;
    float buildDelayLeft = 1f;

    private void Update()
    {
        if(SelectedBuilding && SelectedBuildingArea )
        {
            if (!GameManager.Instance.Player.Animator.GetBool("Walking"))
            {
                if (buildDelayLeft > 0f)
                {
                    buildDelayLeft -= 1f * Time.deltaTime;
                    SelectedBuildingArea.AreaCompleteSprite.fillAmount = 1f - buildDelayLeft;
                }
                else
                {
                    BuildTheSelectedBuildingToSelectedArea();
                }
            }
            else
            {
                buildDelayLeft = BuildDelay;
                SelectedBuildingArea.AreaCompleteSprite.fillAmount = 0f;
            }
        }
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

    public void ChangeSelectedGrid(Grid grid = null)
    {
        if (SelectedGrid != null)
            SelectedGrid.ChangeVisibilityOfBuildingAreas();
        SelectedGrid = grid;
    }

    public void ChangeSelectedBuildingArea(BuildingArea buildingArea = null)
    {
        buildDelayLeft = BuildDelay;
        if(SelectedBuildingArea != null)
            SelectedBuildingArea.ChangeSelectedStatus(false);

        SelectedBuildingArea = buildingArea;
        if (SelectedBuildingArea)
            SelectedBuildingArea.ChangeSelectedStatus(true);
    }

    public void SelectBuildingUIButton(BuildingUIButton buildingUIButton)
    {
        if (SelectedBuildingUIButton)
            SelectedBuildingUIButton.ChangeHighlighStatus(false);
        SelectedBuildingUIButton = buildingUIButton;
        SelectedBuildingUIButton.ChangeHighlighStatus(true);

        ChangeSelectedBuilding(buildingUIButton.TargetBuildingTypeID);
    }

    private void ChangeSelectedBuilding(string typeID)
    {
        SelectedBuilding = GetBuildPrefabByTypeID(typeID);
    }

    public void BuildTheSelectedBuildingToSelectedArea()
    {
        if(SelectedBuildingArea && SelectedBuilding)
            SelectedBuildingArea.BuildToConnectedGrid(SelectedBuilding);

        ChangeSelectedBuildingArea(null);
    }

    public void LevelUpTheBuildingFromSelectedBuildingArea()
    {
        if (SelectedBuildingArea && SelectedBuildingArea.ConnectedGrid && SelectedBuildingArea.ConnectedGrid.CurrentBuilding)
            SelectedBuildingArea.ConnectedGrid.CurrentBuilding.LevelUp();
    }

    public GameObject BuildingsPanel;
    public void BuildingsPanelOpenOrClose()
    {
        BuildingsPanel.SetActive(!BuildingsPanel.activeSelf);
    }
}
