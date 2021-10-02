using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string TypeID;
    public string ID;
    public BuildingArea ConnectedArea = null;

    public int Level;

    public virtual void Initialize(string id, BuildingArea connectedArea = null, bool newCreated = false)
    {
        ID = id;
        ConnectedArea = connectedArea;

        if(!newCreated)
            PlacementBySavedPosition();

        Level = DataManager.Instance.GetBuildingLevel(ID);
    }

    public virtual void UpdateBuildingAreas()
    {
    }

    public virtual void UpdateView()
    {
    }

    public void PlacementBySavedPosition()
    {
        Vector2? positionInfo = DataManager.Instance.GetBuildingPosition(ID);
        if (positionInfo != null)
        {
            transform.position = new Vector3(((Vector2)positionInfo).x, GameManager.Instance.Player.transform.position.y, ((Vector2)positionInfo).y);
        }
    }
}
