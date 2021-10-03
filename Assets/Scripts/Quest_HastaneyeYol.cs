using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_HastaneyeYol : Quest
{
    public Animator NPCAnimator;

    public override void CheckStatus()
    {
        if(Status == "Active" && ConnectedGrid.BotSideGrid && ConnectedGrid.BotSideGrid.CurrentBuilding != null && ConnectedGrid.BotSideGrid.CurrentBuilding.GetType() == typeof(Road))
        {
            QuestCompleted();
        }
    }

    public override void QuestCompleted()
    {
        NPCAnimator.SetTrigger("Happy");
        base.QuestCompleted();
    }
}
