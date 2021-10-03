using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public static bool Vibration
    {
        get => PlayerPrefs.GetInt("Vibration", 1) == 1;
        set => PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
    }

    public static int Money
    {
        get => PlayerPrefs.GetInt("Money", 0);
        set => PlayerPrefs.SetInt("Money", value);
    }

    public int MoneyIncrease(int incereaseValue)
    {
        Money += incereaseValue;
        return Money;
    }

    public int MoneyDecrease(int decreaseValue)
    {
        Money += decreaseValue;
        return Money;
    }

    public static int RoadCounter
    {
        get => PlayerPrefs.GetInt("RoadCounter", 0);
        set => PlayerPrefs.SetInt("RoadCounter", value);
    }

    public int AddOneToRoadCounter()
    {
        RoadCounter++;
        return RoadCounter;
    }

    public string GetSavedBuildingID(string gridID)
    {
        return PlayerPrefs.GetString(gridID + "_CurrentBuilding", "");
    }

    public void SaveBuildingID(string gridID, string buildingTypeID)
    {
        PlayerPrefs.SetString(gridID + "_CurrentBuilding", gridID + "_" + buildingTypeID);
    }

    public int GetBuildingLevel(string buildingID)
    {
        return PlayerPrefs.GetInt(buildingID + "_Level", 0);
    }

    public void SaveBuildingLevel(string buildingID, int level)
    {
        PlayerPrefs.SetInt(buildingID + "_Level", level);
    }

    public Vector2? GetBuildingPosition(string buildingID)
    {
        Vector2 positionInfo = new Vector2(0, 0);

        if (!PlayerPrefs.HasKey(buildingID + "_PosX"))
            return null;

        positionInfo.x = PlayerPrefs.GetFloat(buildingID + "_PosX");
        positionInfo.y = PlayerPrefs.GetFloat(buildingID + "_PosZ");

        return positionInfo;
    }

    public void SaveBuildingPosition(string buildingID, Vector2 positionInfo)
    {
        PlayerPrefs.SetFloat(buildingID + "_PosX", positionInfo.x);
        PlayerPrefs.SetFloat(buildingID + "_PosZ", positionInfo.y);
    }

    public float GetBuildingRotation(string buildingID)
    {
        return PlayerPrefs.GetFloat(buildingID + "_RotY", 0f);
    }

    public void SaveBuildingRotation(string buildingID, float value)
    {
        PlayerPrefs.SetFloat(buildingID + "_RotY", value);
    }

    public string GetBuildingCreationSideInfo(string buildingID)
    {
        return PlayerPrefs.GetString(buildingID + "_CreationSide", "");
    }

    public void SaveBuildingCreationSideInfo(string buildingID, string value)
    {
        PlayerPrefs.SetString(buildingID + "_CreationSide", value);
    }

    public string GetQuestAttachmentFromGrid(string gridID)
    {
        return PlayerPrefs.GetString(gridID + "_Quest", "");
    }

    public void SaveQuestAttachmentToGrid(string gridID, string questID)
    {
        PlayerPrefs.SetString(gridID + "_Quest", questID);
    }

    public string GetQuestStatus(string gridID, string questID)
    {        
        return PlayerPrefs.GetString(gridID + "_" + questID + "_Status", "Inactive");
    }

    public void SaveQuestStatus(string gridID, string questID, string status)
    {
        PlayerPrefs.SetString(gridID + "_" + questID + "_Status", status);
    }
}
