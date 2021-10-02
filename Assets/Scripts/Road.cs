using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Building
{
    public BuildingArea TopBuildingArea;
    public BuildingArea LeftBuildingArea;
    public BuildingArea RightBuildingArea;
    public BuildingArea BotBuildingArea;

    public GameObject[] RoadVariations;

    public override void Initialize(string id, BuildingArea connectedArea = null, bool newCreated = false)
    {
        if (newCreated)
            DataManager.Instance.SaveBuildingPosition(ID, new Vector2(transform.position.x, transform.position.z));

        base.Initialize(id, connectedArea, newCreated);

        UpdateBuildingAreas();
    }

    public override void UpdateBuildingAreas()
    {
        if (TopBuildingArea)
            TopBuildingArea.Initialize(ID + "_T", this);
        if (LeftBuildingArea)
            LeftBuildingArea.Initialize(ID + "_L", this);
        if (RightBuildingArea)
            RightBuildingArea.Initialize(ID + "_R", this);
        if (BotBuildingArea)
            BotBuildingArea.Initialize(ID + "_B", this);

        if (ConnectedArea && BotBuildingArea)
        {
            if (ConnectedArea.Side == BuildingArea.Sides.T)
            {
                BotBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                BotBuildingArea.AreaViewGroup.SetActive(false);
            }
            if (ConnectedArea.Side == BuildingArea.Sides.L)
            {
                //RightBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                //RightBuildingArea.AreaViewGroup.SetActive(false);
                BotBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                BotBuildingArea.AreaViewGroup.SetActive(false);
            }
            if (ConnectedArea.Side == BuildingArea.Sides.R)
            {
                //LeftBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                //LeftBuildingArea.AreaViewGroup.SetActive(false);
                BotBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                BotBuildingArea.AreaViewGroup.SetActive(false);
            }
            if (ConnectedArea.Side == BuildingArea.Sides.B)
            {
                //TopBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                //TopBuildingArea.AreaViewGroup.SetActive(false);
                BotBuildingArea.CurrentBuilding = ConnectedArea.ConnectedBuilding;
                BotBuildingArea.AreaViewGroup.SetActive(false);
            }
        }

        UpdateView();
    }

    [SerializeField]
    bool closedTopSide = false, closedBotSide = false, closedLeftSide = false, closedRightSide = false;
    public override void UpdateView()
    {
        if (TopBuildingArea && TopBuildingArea.CurrentBuilding && !TopBuildingArea.CurrentBuilding.GetComponent<Road>())
            closedTopSide = true;
        else
            closedTopSide = false;
        if (BotBuildingArea && BotBuildingArea.CurrentBuilding && !BotBuildingArea.CurrentBuilding.GetComponent<Road>())
            closedBotSide = true;
        else
            closedBotSide = false;
        if (LeftBuildingArea && LeftBuildingArea.CurrentBuilding && LeftBuildingArea.CurrentBuilding.GetComponent<Road>())
            closedLeftSide = false;
        else
            closedLeftSide = true;
        if (RightBuildingArea && RightBuildingArea.CurrentBuilding && RightBuildingArea.CurrentBuilding.GetComponent<Road>())
            closedRightSide = false;
        else
            closedRightSide = true;

        int roadVariationIndex = 0;
        if (!closedTopSide && !closedBotSide && closedLeftSide && closedRightSide)
        {
            roadVariationIndex = 0;
        }
        else if (!closedTopSide && !closedBotSide && !closedLeftSide && !closedRightSide)
        {
            roadVariationIndex = 1;
        }
        else if (!closedTopSide && !closedBotSide && closedLeftSide && !closedRightSide)
        {
            roadVariationIndex = 2;
        }
        else if (!closedTopSide && !closedBotSide && !closedLeftSide && closedRightSide)
        {
            roadVariationIndex = 3;
        }
        else if (closedTopSide && !closedBotSide && closedLeftSide && !closedRightSide)
        {
            roadVariationIndex = 4;
        }
        else if (closedTopSide && !closedBotSide && !closedLeftSide && closedRightSide)
        {
            roadVariationIndex = 5;
        }

        for (int i = 0; i < RoadVariations.Length; i++)
        {
            if (i == roadVariationIndex)
                RoadVariations[i].SetActive(true);
            else
                RoadVariations[i].SetActive(false);
        }
    }
}