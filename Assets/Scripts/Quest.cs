using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestSO QuestSO;
    public Grid ConnectedGrid = null;
    public float StartDelay = 1f;
    public string Status = "Inactive";
    public Transform CamPos;

    public void Initialize(QuestSO questSO, Grid grid)
    {
        QuestSO = questSO;
        ConnectedGrid = grid;
        Status = DataManager.Instance.GetQuestStatus(grid.ID, QuestSO.ID);
        CheckStatus();

        if(Status == "Inactive")
        {
            StartCoroutine(Delay());
            IEnumerator Delay()
            {
                yield return new WaitForSeconds(StartDelay);
                SetStatusAsActive();
            }
        }
        else if(Status == "Active")
        {
            SetStatusAsActive();
        }
        else if (Status == "Completed")
        {
            QuestManager.Instance.AQuestActivated(this);
            QuestCompleted();
        }
        else if (Status == "Collected")
        {
            QuestManager.Instance.AQuestCollected(this);
        }
    }

    public void SetStatusAsActive()
    {
        Status = "Active";
        DataManager.Instance.SaveQuestStatus(ConnectedGrid.ID, QuestSO.ID, "Active");
        QuestManager.Instance.AQuestActivated(this);
    }

    public virtual void CheckStatus()
    {

    }

    public virtual void QuestCompleted()
    {
        Status = "Completed";
        QuestManager.Instance.AQuestCompleted(this);
    }

    public virtual void QuestCollected()
    {
        Status = "Collected";
    }
}
