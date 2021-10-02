using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    public Building ConnectedBuilding;
    [SerializeField]
    public enum Sides
    {
        T,
        L,
        R,
        B
    }
    public Sides Side;
    public string ID;
    public Building CurrentBuilding = null;

    public GameObject AreaViewGroup;

    public void Initialize(string id, Building connectedBuilding = null)
    {
        Debug.Log(connectedBuilding);
        ConnectedBuilding = connectedBuilding;
        ID = id;

        //0_Top => 0_Top_Road3
        string currentBuildID = DataManager.Instance.GetSavedBuildingID(ID);
        if (currentBuildID != "")
        {
            string currentBuildingTypeID = currentBuildID.Split('_')[currentBuildID.Split('_').Length - 1];

            Building buildingPrefab = BuildingManager.Instance.GetBuildPrefabByTypeID(currentBuildingTypeID);

            Building building = Instantiate(buildingPrefab);
            building.transform.parent = transform;
            building.transform.localPosition = Vector3.zero;
            building.transform.localRotation = Quaternion.Euler(Vector3.zero);
            building.transform.parent = null;
            CurrentBuilding = building;

            CurrentBuilding.Initialize(currentBuildID, this);

            if (ConnectedBuilding)
                ConnectedBuilding.UpdateView();

            AreaViewGroup.SetActive(false);
        }
    }

    public void BuildTheTargetBuilding(Building buildingPrefab)
    {
        if (CurrentBuilding != null)
        {
            Debug.LogError(ID + " - Buraya inşa yapılamaz çünkü zaten burada bir bina yer alıyor!");
            return;
        }

        Building building = Instantiate(buildingPrefab);
        building.transform.parent = transform;
        building.transform.localPosition = Vector3.zero;
        building.transform.localRotation = Quaternion.Euler(Vector3.zero);
        building.transform.parent = null;
        CurrentBuilding = building;

        if (building.GetComponent<Road>())
        {
            DataManager.Instance.SaveBuildingID(ID, buildingPrefab.TypeID, DataManager.Instance.AddOneToRoadCounter().ToString());
            CurrentBuilding.Initialize(DataManager.Instance.GetSavedBuildingID(ID), this, true);
        }
        else
        {
            DataManager.Instance.SaveBuildingID(ID, buildingPrefab.TypeID);
            CurrentBuilding.Initialize(DataManager.Instance.GetSavedBuildingID(ID), this, true);
        }

        if (ConnectedBuilding)
            ConnectedBuilding.UpdateView();

        AreaViewGroup.SetActive(false);
    }
}
