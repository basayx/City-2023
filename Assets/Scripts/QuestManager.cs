using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public List<QuestSO> QuestSOs = new List<QuestSO>();
    public List<Quest> CurrentActiveQuests = new List<Quest>();

    public GameObject GeneralPanelsGroup;
    public GameObject QuestsIconPanel;
    public Transform QuestsIconListParent;
    public QuestUIIcon QuestUIIconPrefab;

    public Quest SelectedQuest = null;
    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;
    public GameObject DescriptionPanel;
    public TextMeshProUGUI QuestDescriptionText;
    public GameObject PrizesPanel;
    public TextMeshProUGUI MoneyPrizeText;
    public GameObject DiaglogueOkeyButton;
    public GameObject DiaglogueCollectButton;

    public QuestSO GetQuestSO(string id)
    {
        foreach(QuestSO questSO in QuestSOs)
        {
            if (questSO.ID == id)
                return questSO;
        }

        return null;
    }

    public void InitializeTargetQuest(QuestSO questSO, Grid grid)
    {
        if (DataManager.Instance.GetQuestStatus(grid.ID, questSO.ID) == "Collected")
            return;

        Quest quest = Instantiate(questSO.QuestPrefab, null);
        quest.transform.parent = grid.transform;
        quest.transform.localPosition = Vector3.zero;
        quest.Initialize(questSO, grid);
    }

    public void AQuestActivated(Quest quest)
    {
        foreach(QuestUIIcon uiIcon in QuestsIconListParent.GetComponentsInChildren<QuestUIIcon>())
        {
            if (uiIcon.Quest == quest)
                Destroy(uiIcon.gameObject);
        }
        QuestUIIcon questUIIcon = Instantiate(QuestUIIconPrefab, QuestsIconListParent);
        questUIIcon.Initialize(quest);

        if (!CurrentActiveQuests.Find((x) => x == quest))
            CurrentActiveQuests.Add(quest);
    }

    public void UpdateQuestsList()
    {
        foreach(QuestUIIcon questUIIcon in QuestsIconListParent.GetComponentsInChildren<QuestUIIcon>())
        {
            questUIIcon.Initialize(questUIIcon.Quest);
        }
    }

    public void CheckActiveQuests()
    {
        foreach(Quest quest in CurrentActiveQuests)
        {
            quest.CheckStatus();
        }
    }

    public void AQuestSelected(Quest quest)
    {
        GeneralPanelsGroup.SetActive(false);
        SelectedQuest = quest;
        CameraController.Instance.MoveToTargetPos(quest.CamPos);
        StartCoroutine(CamWaiting());
        IEnumerator CamWaiting()
        {
            while (!CameraController.Instance.IsReady)
            {
                yield return new WaitForEndOfFrame();
            }
            OpenDialoguePanel(quest);
        }
    }

    public void UnSelectQuest(float delay = 0f)
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            UpdateQuestsList();
            GeneralPanelsGroup.SetActive(true);
            CameraController.Instance.BackToMainPos();
            if (SelectedQuest.Status == "Collected")
            {
                while (!CameraController.Instance.IsReady)
                {
                    yield return new WaitForEndOfFrame();
                }
                AQuestCollected(SelectedQuest);
            }
            SelectedQuest = null;
        }
    }

    public void OpenDialoguePanel(Quest quest)
    {
        QuestDescriptionText.text = quest.QuestSO.QuestDescription;
        if (DataManager.Instance.GetQuestStatus(quest.ConnectedGrid.ID, quest.QuestSO.ID) == "Active")
        {
            DescriptionPanel.SetActive(true);
            PrizesPanel.SetActive(false);
            DialogueText.text = quest.QuestSO.RequestTalk;
            DiaglogueOkeyButton.SetActive(true);
            DiaglogueCollectButton.SetActive(false);
        }
        else if(DataManager.Instance.GetQuestStatus(quest.ConnectedGrid.ID, quest.QuestSO.ID) == "Completed")
        {
            PrizesPanel.SetActive(true);
            DescriptionPanel.SetActive(false);
            DialogueText.text = quest.QuestSO.ResultTalk;
            DiaglogueCollectButton.SetActive(true);
            DiaglogueOkeyButton.SetActive(false);

            if (quest.QuestSO.MoneyPrizeAmount > 0)
            {
                PrizesPanel.SetActive(true);
                MoneyPrizeText.text = "+" + quest.QuestSO.MoneyPrizeAmount.ToString();
            }
            else
                PrizesPanel.SetActive(false);
        }

        DialoguePanel.SetActive(true);
    }

    public void CloseDialoguePanel()
    {
        DialoguePanel.SetActive(false);
    }

    public void OnClickedToDialogueOkeyButton()
    {
        UnSelectQuest();
        CloseDialoguePanel();
    }

    public void OnClickedToCollectButton()
    {
        SelectedQuest.QuestCollected();
        DataManager.Instance.MoneyIncrease(SelectedQuest.QuestSO.MoneyPrizeAmount);
        DataManager.Instance.SaveQuestStatus(SelectedQuest.ConnectedGrid.ID, SelectedQuest.QuestSO.ID, "Collected");
        CloseDialoguePanel();
        UnSelectQuest(0.5f);
    }

    public void AQuestCompleted(Quest quest)
    {
        DataManager.Instance.SaveQuestStatus(quest.ConnectedGrid.ID, quest.QuestSO.ID, "Completed");
        UpdateQuestsList();
    }

    public void AQuestCollected(Quest quest)
    {
        CurrentActiveQuests.Remove(quest);
        Destroy(quest.gameObject);
    }
}
