﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GridMap", menuName = "GridMap")]
public class GridMapSO : ScriptableObject
{
    public int RowCount;
    public int ColumnCount;
    public Grid GridPrefab;
    [Serializable]
    public struct ReadyGridInfo
    {
        public Vector2Int ColoumnAndRow;
        public Building BuildingPrefab;
        public int BuildingLevel;
    }
    public List<ReadyGridInfo> ReadyGridInfos = new List<ReadyGridInfo>();
}