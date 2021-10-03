using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public string ID;
    public int RowIndex;
    public int ColoumnIndex;

    public Building CurrentBuilding;

    public Grid TopSideGrid;
    public Grid BotSideGrid;
    public Grid LeftSideGrid;
    public Grid RightSideGrid;

    public GameObject BuildingAreasGroup;
    public BuildingArea TopBuildingArea;
    public BuildingArea BotBuildingArea;
    public BuildingArea LeftBuildingArea;
    public BuildingArea RightBuildingArea;

    public bool IsABuildingEnterance = false;

    public QuestSO QuestAttachment = null;

    public void Initialize(string id)
    {
        ID = id;
        gameObject.name = RowIndex + "_" + ColoumnIndex;

        string questID = DataManager.Instance.GetQuestAttachmentFromGrid(ID);
        if (questID != "")
        {
            QuestAttachment = QuestManager.Instance.GetQuestSO(questID);
            if(QuestAttachment != null)
            {
                QuestManager.Instance.InitializeTargetQuest(QuestAttachment, this);
            }
        }
    }

    public Building Build(Building buildingPrefab, float rotY)
    {
        Building building = Instantiate(buildingPrefab);
        building.transform.parent = transform;
        building.transform.localPosition = Vector3.zero;
        building.transform.localRotation = Quaternion.Euler(Vector3.zero);//new Vector3(0, rotY, 0));
        //building.transform.parent = null;
        return building;
    }

    public void CheckNeighborGrids(bool saveCheck = false)
    {
        TopSideGrid = GridManager.Instance.GetGrid(RowIndex + 1, ColoumnIndex);
        BotSideGrid = GridManager.Instance.GetGrid(RowIndex - 1, ColoumnIndex);
        LeftSideGrid = GridManager.Instance.GetGrid(RowIndex, ColoumnIndex - 1);
        RightSideGrid = GridManager.Instance.GetGrid(RowIndex, ColoumnIndex + 1);

        if (TopSideGrid)
        {
            if (TopSideGrid.CurrentBuilding != null)
            {
                TopBuildingArea.gameObject.SetActive(false);
            }
            else
                TopBuildingArea.Initialize(TopSideGrid);
        }
        else
            TopBuildingArea.gameObject.SetActive(false);

        if (BotSideGrid)
        {
            if (BotSideGrid.CurrentBuilding != null)
            {
                BotBuildingArea.gameObject.SetActive(false);
            }
            else
                BotBuildingArea.Initialize(BotSideGrid);
        }
        else
            BotBuildingArea.gameObject.SetActive(false);

        if (LeftSideGrid)
        {
            if (LeftSideGrid.CurrentBuilding != null)
            {
                LeftBuildingArea.gameObject.SetActive(false);
            }
            else
                LeftBuildingArea.Initialize(LeftSideGrid);
        }
        else
            LeftBuildingArea.gameObject.SetActive(false);

        if (RightSideGrid)
        {
            if (RightSideGrid.CurrentBuilding != null)
            {
                RightBuildingArea.gameObject.SetActive(false);
            }
            else
                RightBuildingArea.Initialize(RightSideGrid);
        }
        else
            RightBuildingArea.gameObject.SetActive(false);

        if(saveCheck)
            CheckSavedBuilding();
    }

    public void CheckSavedBuilding()
    {
        string currentBuildingID = DataManager.Instance.GetSavedBuildingID(ID);
        if (currentBuildingID != "")
        {
            string currentBuildingTypeID = currentBuildingID.Split('_')[currentBuildingID.Split('_').Length - 1];

            Building buildingPrefab = BuildingManager.Instance.GetBuildPrefabByTypeID(currentBuildingTypeID);
            CurrentBuilding = Build(buildingPrefab, DataManager.Instance.GetBuildingRotation(currentBuildingID));
            CurrentBuilding.Initialize(currentBuildingID, this);
        }
    }

    public void BuildTheTargetBuilding(Building buildingPrefab, BuildingArea buildingArea)
    {
        if (CurrentBuilding != null)
        {
            Debug.LogError(ID + " - Buraya inşa yapılamaz çünkü zaten burada bir bina yer alıyor!");
            return;
        }

        Building building = Build(buildingPrefab, buildingArea.transform.localEulerAngles.y);
        CurrentBuilding = building;

        DataManager.Instance.SaveBuildingID(ID, buildingPrefab.TypeID);

        if (buildingArea.Side == Sides.T)
            DataManager.Instance.SaveBuildingCreationSideInfo(DataManager.Instance.GetSavedBuildingID(ID), "T");
        else if (buildingArea.Side == Sides.B)
            DataManager.Instance.SaveBuildingCreationSideInfo(DataManager.Instance.GetSavedBuildingID(ID), "B");
        else if (buildingArea.Side == Sides.L)
            DataManager.Instance.SaveBuildingCreationSideInfo(DataManager.Instance.GetSavedBuildingID(ID), "L");
        else if (buildingArea.Side == Sides.R)
            DataManager.Instance.SaveBuildingCreationSideInfo(DataManager.Instance.GetSavedBuildingID(ID), "R");

        CurrentBuilding.Initialize(DataManager.Instance.GetSavedBuildingID(ID), this, true);
    }

    public void ChangeVisibilityOfBuildingAreas(bool status = false)
    {
        CheckNeighborGrids();
        BuildingAreasGroup.SetActive(status);
    }
}
