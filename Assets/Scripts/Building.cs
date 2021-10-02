using System.Collections.Generic;
using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string TypeID;
    public string ID;
    public Grid ConnectedGrid = null;

    [Serializable]
    public struct GridSizeProperty
    {
        public int RowSize;
        public int ColoumnSize;
    }
    public GridSizeProperty GridSize;
    public int Level;

    [Serializable]
    public struct LevelProperty
    {
        public int Price;
        public GameObject LevelGroup;
        public GridSizeProperty GridSize;
    }
    public LevelProperty[] LevelProperties;
    public Sides CreatedFromThisSide;

    public virtual void Initialize(string id, Grid connectedGrid = null, bool newCreated = false)
    {
        ID = id;
        ConnectedGrid = connectedGrid;

        //if(!newCreated)
        //    PlacementBySavedPosition();

        Level = DataManager.Instance.GetBuildingLevel(ID);
        GridSize = LevelProperties[Level].GridSize;
        GridManager.Instance.FillOtherGridsBySize(this);

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

        string creationSide = DataManager.Instance.GetBuildingCreationSideInfo(ID);
        if (creationSide != "")
        {
            if(creationSide == "T")
                CreatedFromThisSide = Sides.T;
            else if (creationSide == "B")
                CreatedFromThisSide = Sides.B;
            else if (creationSide == "L")
                CreatedFromThisSide = Sides.L;
            else if (creationSide == "R")
                CreatedFromThisSide = Sides.R;
        }
    }

    public void PlacementBySavedPosition()
    {
        Vector2? positionInfo = DataManager.Instance.GetBuildingPosition(ID);
        if (positionInfo != null)
        {
            transform.position = new Vector3(((Vector2)positionInfo).x, GameManager.Instance.Player.transform.position.y, ((Vector2)positionInfo).y);
        }
    }

    public void LevelUp()
    {
        Level = DataManager.Instance.GetBuildingLevel(ID);
        if (LevelProperties.Length <= Level + 1)
            return;

        LevelProperty levelProperty = LevelProperties[Level + 1];

        if (GridManager.Instance.CheckGridSize(ConnectedGrid, levelProperty.GridSize.RowSize, levelProperty.GridSize.ColoumnSize) == false)
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
