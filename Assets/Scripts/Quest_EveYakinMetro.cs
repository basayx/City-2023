using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_EveYakinMetro : Quest
{
    public SubwayKid SubwayKid;

    public override void CheckStatus()
    {
        if (Status == "Active" && ConnectedGrid && ConnectedGrid.CurrentBuilding)
        {
            Vector2Int[] coloumnsAndRows = {
            new Vector2Int(1,0),
        };
            for(int i = 0; i < coloumnsAndRows.Length; i++)
            {
                Grid grid = GridManager.Instance.GetGridByReferanceGrid(ConnectedGrid, coloumnsAndRows[i].y, coloumnsAndRows[i].x, ConnectedGrid.CurrentBuilding.CreatedFromThisArea.Side);
                if(grid != null && grid.CurrentBuilding && grid.CurrentBuilding.TypeID == "SubwayStop0")
                {
                    QuestCompleted();
                    break;
                }
            }
        }
    }

    public override void QuestCompleted()
    {
        SubwayKid.GraphicAnimator.SetTrigger("Happy");
        base.QuestCompleted();
    }

    public override void QuestCollected()
    {
        SubwayKid.transform.parent.parent = ConnectedGrid.transform;
        SubwayKid.Initialize(17f);
        base.QuestCollected();
    }
}
