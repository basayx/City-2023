using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_RayliUlasim : Quest
{
    public Animator NPCAnimator;

    public override void CheckStatus()
    {
        if(Status == "Active" && ConnectedGrid.RightSideGrid && ConnectedGrid.RightSideGrid.CurrentBuilding != null && ConnectedGrid.RightSideGrid.CurrentBuilding.TypeID == "Railway6")
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
