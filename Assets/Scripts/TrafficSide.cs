using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class TrafficSide : MonoBehaviour
{
    public NavMeshSurface NavMeshSurface;

    public void Initialize()
    {
        NavMeshSurface.BuildNavMesh();
    }
}
