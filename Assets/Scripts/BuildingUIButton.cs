using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIButton : MonoBehaviour
{
    public Image BorderImage;
    public Color32 DefaultColor = Color.white;
    public Color32 HighlightColor = Color.yellow;
    public string TargetBuildingTypeID;

    public void ChangeHighlighStatus(bool status = false)
    {
        if (status)
            BorderImage.color = HighlightColor;
        else
            BorderImage.color = DefaultColor;
    }
}
