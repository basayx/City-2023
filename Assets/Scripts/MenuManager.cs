using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public GameObject MenuCanvas;
    public GameObject GameCanvas;
    public Transform MenuStartPos;
    public Transform GameStartCamPos;

    public GameObject MoneyCanvas;
    public TextMeshProUGUI MoneyText;
    public GameObject MoneyBar;

    private void Start()
    {
        UpdateMoneyText();
    }

    public void PlayButton()
    {
        MenuCanvas.SetActive(false);

        CameraController.Instance.BackToMainPos();
        StartCoroutine(WaitingToCamera());
        IEnumerator WaitingToCamera()
        {
            while (!CameraController.Instance.IsReady)
            {
                yield return new WaitForEndOfFrame();
            }
            GameManager.Instance.GameState = true;
            PlayerController.Instance.CanMove = true;
            GameCanvas.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        MenuStartPos.transform.position = new Vector3(CameraController.Instance.transform.position.x, MenuStartPos.transform.position.y, CameraController.Instance.transform.position.z);

        BuildingManager.Instance.ChangeSelectedBuildingArea(null);
        BuildingManager.Instance.ChangeSelectedGrid(null);

        GameCanvas.SetActive(false);

        CameraController.Instance.MoveToTargetPos(MenuStartPos);
        StartCoroutine(WaitingToCamera());
        IEnumerator WaitingToCamera()
        {
            while (!CameraController.Instance.IsReady)
            {
                yield return new WaitForEndOfFrame();
            }
            GameManager.Instance.GameState = false;
            PlayerController.Instance.CanMove = false;
            MenuCanvas.SetActive(true);
        }
    }

    public void UpdateMoneyText()
    {
        MoneyText.text = DataManager.Money.ToString();
        MoneyBar.transform.DORewind();
        MoneyBar.transform.DOPunchScale(new Vector3(.25f, .25f, .25f), .25f);
    }
}
