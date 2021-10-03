using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIIcon : MonoBehaviour
{
    public Quest Quest;
    public Image QuestImage;
    public Image NotificationImage;
    public Sprite ActiveNotificationSprite;
    public Sprite CompletedNotificationSprite;

    public void Initialize(Quest quest)
    {
        Quest = quest;
        QuestImage.sprite = quest.QuestSO.QuestImageSprite;
        if (quest.Status == "Active")
        {
            NotificationImage.sprite = ActiveNotificationSprite;
        }
        else if (quest.Status == "Completed")
        {
            NotificationImage.sprite = CompletedNotificationSprite;
        }
        else if (quest.Status == "Collected")
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("ERROR - Bu ikonun listede olmaması gerekliydi?");
        }
    }

    public void OnClick()
    {
        QuestManager.Instance.AQuestSelected(Quest);
    }
}
