using System.Collections.Generic;
using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string TypeID;
    public string ID;
    public Grid ConnectedGrid = null;

    public List<Vector2Int> ColoumnsAndRows = new List<Vector2Int>();
    public int Level;

    [Serializable]
    public struct LevelProperty
    {
        public int Price;
        public GameObject LevelGroup;
        public List<Vector2Int> ColoumnsAndRows;
    }
    public LevelProperty[] LevelProperties;

    public BuildingArea CreatedFromThisArea;

    public bool IsHaveBuildingEnterance = false;

    public virtual void Initialize(string id, Grid connectedGrid = null, bool newCreated = false)
    {
        ID = id;
        ConnectedGrid = connectedGrid;
        ConnectedGrid.IsABuildingEnterance = IsHaveBuildingEnterance;

        QuestManager.Instance.CheckActiveQuests();

        //if(!newCreated)
        //    PlacementBySavedPosition();

        Level = DataManager.Instance.GetBuildingLevel(ID);
        ColoumnsAndRows = new List<Vector2Int>(LevelProperties[Level].ColoumnsAndRows);

        string creationSide = DataManager.Instance.GetBuildingCreationSideInfo(ID);        
        if (creationSide != "")
        {
            //HACK: İşin aslında burada doğrudan bağlı olunan griddeki areaya değil bağlı olunan gridin bağlı olduğu gridin areası alınmalı.
            //Ancak bu bilgi şu an sadece rotasyon için kullanıldığından böyle bırakıyorum.
            if (creationSide == "T" && ConnectedGrid.TopBuildingArea)
                CreatedFromThisArea = ConnectedGrid.TopBuildingArea;
            else if (creationSide == "B" && ConnectedGrid.BotBuildingArea)
                CreatedFromThisArea = ConnectedGrid.BotBuildingArea;
            else if (creationSide == "L" && ConnectedGrid.LeftBuildingArea)
                CreatedFromThisArea = ConnectedGrid.LeftBuildingArea;
            else if (creationSide == "R" && ConnectedGrid.RightBuildingArea)
                CreatedFromThisArea = ConnectedGrid.RightBuildingArea;
        }

        if (this.GetType() != typeof(Road))
            transform.localRotation = Quaternion.Euler(0, CreatedFromThisArea.transform.localEulerAngles.y, 0);

        GridManager.Instance.FillOtherGridsBySize(this, (CreatedFromThisArea ? CreatedFromThisArea.Side : Sides.T));

        if (newCreated)
        {
            if (connectedGrid.TopSideGrid && connectedGrid.TopSideGrid.CurrentBuilding)
                connectedGrid.TopSideGrid.CurrentBuilding.UpdateLevelView();
            if (connectedGrid.BotSideGrid && connectedGrid.BotSideGrid.CurrentBuilding)
                connectedGrid.BotSideGrid.CurrentBuilding.UpdateLevelView();
            if (connectedGrid.LeftSideGrid && connectedGrid.LeftSideGrid.CurrentBuilding)
                connectedGrid.LeftSideGrid.CurrentBuilding.UpdateLevelView();
            if (connectedGrid.RightSideGrid && connectedGrid.RightSideGrid.CurrentBuilding)
                connectedGrid.RightSideGrid.CurrentBuilding.UpdateLevelView();          

            UpdateLevelView();
        }
    }

    public void PlacementBySavedPosition()
    {
        Vector2? positionInfo = DataManager.Instance.GetBuildingPosition(ID);
        if (positionInfo != null)
        {
            transform.position = new Vector3(((Vector2)positionInfo).x, PlayerController.Instance.transform.position.y, ((Vector2)positionInfo).y);
        }
    }

    public void LevelUp()
    {
        Level = DataManager.Instance.GetBuildingLevel(ID);
        if (LevelProperties.Length <= Level + 1)
            return;

        LevelProperty levelProperty = LevelProperties[Level + 1];

        if (GridManager.Instance.CheckGridSize(ConnectedGrid, levelProperty.ColoumnsAndRows, (CreatedFromThisArea ? CreatedFromThisArea.Side : Sides.T)) == false)
            return;

        if(DataManager.Money > levelProperty.Price)
        {
            DataManager.Instance.MoneyDecrease(levelProperty.Price);
            Level++;
            DataManager.Instance.SaveBuildingLevel(ID, Level);
            UpdateLevelView();
        }
    }

    public virtual void UpdateLevelView()
    {
        for(int i = 0; i < LevelProperties.Length; i++)
        {
            if(i == Level)
                LevelProperties[i].LevelGroup.SetActive(true);
            else
                LevelProperties[i].LevelGroup.SetActive(false);
        }
    }
}
