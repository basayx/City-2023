using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject GameCanvas;
    public Transform MenuStartPos;
    public Transform GameStartCamPos;

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
}
