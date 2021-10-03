using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestSO : ScriptableObject
{
    public string ID;
    public Quest QuestPrefab;
    public Sprite QuestImageSprite;
    [TextArea]
    public string RequestTalk;
    [TextArea]
    public string QuestDescription;
    [TextArea]
    public string ResultTalk;
    public int MoneyPrizeAmount = 0;
}
