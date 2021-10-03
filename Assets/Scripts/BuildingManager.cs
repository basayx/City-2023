using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public Building[] AllBuildingPrefabs;

    public Grid SelectedGrid;
    public BuildingArea SelectedBuildingArea;
    public Building SelectedBuilding;
    public BuildingUIButton SelectedBuildingUIButton;
    public GameObject BuildingsPanel;

    public float BuildDelay = 1f;
    float buildDelayLeft = 1f;

    private void Update()
    {
        if(SelectedBuilding && SelectedBuildingArea )
        {
            if (!PlayerController.Instance.Animator.GetBool("Walking"))
            {
                if (buildDelayLeft > 0f)
                {
                    buildDelayLeft -= 1f * Time.deltaTime;
                    SelectedBuildingArea.AreaCompleteSprite.fillAmount = 1f - buildDelayLeft;
                }
                else
                {
                    BuildTheSelectedBuildingToSelectedArea();
                }
            }
            else
            {
                buildDelayLeft = BuildDelay;
                SelectedBuildingArea.AreaCompleteSprite.fillAmount = 0f;
            }
        }
    }

    public Building GetBuildPrefabByTypeID(string typeID)
    {
        foreach(Building buildingPrefab in AllBuildingPrefabs)
        {
            if(buildingPrefab.TypeID == typeID)
            {
                return buildingPrefab;
            }
        }

        return null;
    }

    public void ChangeSelectedGrid(Grid grid = null)
    {
        if (SelectedGrid != null)
            SelectedGrid.ChangeVisibilityOfBuildingAreas();
        SelectedGrid = grid;
    }

    public void ChangeSelectedBuildingArea(BuildingArea buildingArea = null)
    {
        buildDelayLeft = BuildDelay;
        if (buildingArea == null)
            PlayerController.Instance.Animator.SetBool("Working", false);
        if(SelectedBuildingArea != null)
            SelectedBuildingArea.ChangeSelectedStatus(false);

        SelectedBuildingArea = buildingArea;
        if (SelectedBuildingArea)
            SelectedBuildingArea.ChangeSelectedStatus(true);
    }

    public void SelectBuildingUIButton(BuildingUIButton buildingUIButton)
    {
        if (SelectedBuildingUIButton)
        {
            SelectedBuildingUIButton.ChangeHighlighStatus(false);

            if (SelectedBuildingUIButton.TargetBuildingTypeID == buildingUIButton.TargetBuildingTypeID)
                return;
        }

        SelectedBuildingUIButton = buildingUIButton;
        SelectedBuildingUIButton.ChangeHighlighStatus(true);

        ChangeSelectedBuilding(buildingUIButton.TargetBuildingTypeID);
    }

    private void ChangeSelectedBuilding(string typeID)
    {
        SelectedBuilding = GetBuildPrefabByTypeID(typeID);
        if(SelectedBuildingArea)
            PlayerController.Instance.Animator.SetBool("Working", true);
    }

    public void BuildTheSelectedBuildingToSelectedArea()
    {
        if(SelectedBuildingArea && SelectedBuilding)
            SelectedBuildingArea.BuildToConnectedGrid(SelectedBuilding);

        if (SelectedBuilding.GetType() != typeof(Road))
        {
            SelectedBuildingArea.ChangeSelectedStatus(false);
            ChangeSelectedBuilding(null);
        }

        ChangeSelectedBuildingArea(null);
    }

    public void LevelUpTheBuildingFromSelectedBuildingArea()
    {
        if (SelectedBuildingArea && SelectedBuildingArea.ConnectedGrid && SelectedBuildingArea.ConnectedGrid.CurrentBuilding)
            SelectedBuildingArea.ConnectedGrid.CurrentBuilding.LevelUp();
    }

    public void BuildingsPanelOpenOrClose()
    {
        BuildingsPanel.SetActive(!BuildingsPanel.activeSelf);
    }

    public void BuildingsPanelOpenOrClose(bool status)
    {
        BuildingsPanel.SetActive(status);
    }

    public static IEnumerator DoScale(Transform _transform)
    {
        Debug.Log(_transform.gameObject.name);
        Vector3 targetScale = _transform.localScale;
        _transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence().Append(_transform.DOScale(targetScale * 1.1f, .25f)).Append(_transform.DOScale(targetScale, .1f));

        yield return seq.WaitForCompletion();
    }
}
