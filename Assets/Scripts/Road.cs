using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    public GameObject[] RoadVariations;
    public GameObject[] RoadEdgeColliders;

    public override void Initialize(string id, Grid connectedGrid = null, bool newCreated = false)
    {
        base.Initialize(id, connectedGrid, newCreated);
    }

    public override void UpdateLevelView()
    {
        base.UpdateLevelView();

        UpdateView();
    }

    [SerializeField]
    bool closedTopSide = false, closedBotSide = false, closedLeftSide = false, closedRightSide = false;
    public void UpdateView()
    {
        if (ConnectedGrid.TopSideGrid && ConnectedGrid.TopSideGrid.CurrentBuilding && (ConnectedGrid.TopSideGrid.CurrentBuilding.GetType() == typeof(Road) || ConnectedGrid.TopSideGrid.IsABuildingEnterance))
            closedTopSide = false;
        else
            closedTopSide = true;

        if (ConnectedGrid.BotSideGrid && ConnectedGrid.BotSideGrid.CurrentBuilding && (ConnectedGrid.BotSideGrid.CurrentBuilding.GetType() == typeof(Road) || ConnectedGrid.BotSideGrid.IsABuildingEnterance))
            closedBotSide = false;
        else
            closedBotSide = true;

        if (ConnectedGrid.LeftSideGrid && ConnectedGrid.LeftSideGrid.CurrentBuilding && (ConnectedGrid.LeftSideGrid.CurrentBuilding.GetType() == typeof(Road) || ConnectedGrid.LeftSideGrid.IsABuildingEnterance))
            closedLeftSide = false;
        else
            closedLeftSide = true;

        if (ConnectedGrid.RightSideGrid && ConnectedGrid.RightSideGrid.CurrentBuilding && (ConnectedGrid.RightSideGrid.CurrentBuilding.GetType() == typeof(Road) || ConnectedGrid.RightSideGrid.IsABuildingEnterance))
            closedRightSide = false;
        else
            closedRightSide = true;


        if (closedTopSide)
            RoadEdgeColliders[0].SetActive(true);
        else
            RoadEdgeColliders[0].SetActive(false);

        if (closedBotSide)
            RoadEdgeColliders[1].SetActive(true);
        else
            RoadEdgeColliders[1].SetActive(false);

        if (closedLeftSide)
            RoadEdgeColliders[2].SetActive(true);
        else
            RoadEdgeColliders[2].SetActive(false);

        if (closedRightSide)
            RoadEdgeColliders[3].SetActive(true);
        else
            RoadEdgeColliders[3].SetActive(false);


        int roadVariationIndex = 0;

        if (closedTopSide && closedBotSide && closedLeftSide && closedRightSide)
            roadVariationIndex = 0;
        else
        if (closedTopSide && !closedBotSide && closedLeftSide && closedRightSide)
            roadVariationIndex = 0;
        else
        if (!closedTopSide && closedBotSide && closedLeftSide && closedRightSide)
            roadVariationIndex = 0;
        else
        if (!closedTopSide && !closedBotSide && closedLeftSide && closedRightSide)
            roadVariationIndex = 0;

        else
        if (closedTopSide && closedBotSide && !closedLeftSide && closedRightSide)
            roadVariationIndex = 6;
        else
        if (closedTopSide && !closedBotSide && !closedLeftSide && closedRightSide)
            roadVariationIndex = 1;
        else
        if (!closedTopSide && closedBotSide && !closedLeftSide && closedRightSide)
            roadVariationIndex = 10;
        else
        if (!closedTopSide && !closedBotSide && !closedLeftSide && closedRightSide)
            roadVariationIndex = 3;

        else
        if (closedTopSide && closedBotSide && closedLeftSide && !closedRightSide)
            roadVariationIndex = 6;
        else
        if (closedTopSide && !closedBotSide && closedLeftSide && !closedRightSide)
            roadVariationIndex = 2;
        else
        if (!closedTopSide && closedBotSide && closedLeftSide && !closedRightSide)
            roadVariationIndex = 9;
        else
        if (!closedTopSide && !closedBotSide && closedLeftSide && !closedRightSide)
            roadVariationIndex = 4;

        else
        if (!closedTopSide && !closedBotSide && !closedLeftSide && !closedRightSide)
            roadVariationIndex = 5;

        else
        if (closedTopSide && closedBotSide && !closedLeftSide && !closedRightSide)
            roadVariationIndex = 6;

        else
        if (!closedTopSide && closedBotSide && !closedLeftSide && !closedRightSide)
            roadVariationIndex = 7;

        else
        if (closedTopSide && !closedBotSide && !closedLeftSide && !closedRightSide)
            roadVariationIndex = 8;


        for (int i = 0; i < RoadVariations.Length; i++)
        {
            if (i == roadVariationIndex)
                RoadVariations[i].SetActive(true);
            else
                RoadVariations[i].SetActive(false);
        }
    }
}