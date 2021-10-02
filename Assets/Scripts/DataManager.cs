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

    public string GetSavedBuildingID(string buildingAreaID)
    {
        return PlayerPrefs.GetString(buildingAreaID + "_CurrentBuilding", "");
    }

    public void SaveBuildingID(string buildingAreaID, string buildingTypeID)
    {
        PlayerPrefs.SetString(buildingAreaID + "_CurrentBuilding", buildingAreaID + "_" + buildingTypeID);
    }

    public void SaveBuildingID(string buildingAreaID, string buildingTypeID, string no)
    {
        PlayerPrefs.SetString(buildingAreaID + "_CurrentBuilding", no + "_" + buildingTypeID);
    }

    public int GetBuildingLevel(string buildingID)
    {
        return PlayerPrefs.GetInt(buildingID + "_Level", 0);
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
}
