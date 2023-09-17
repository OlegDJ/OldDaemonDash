using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshManager : MonoBehaviour
{
    #region Singleton
    public static NavMeshManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField] private NavMeshSurface[] surfaces;

    private void Start() { GenerateNavMeshSurface(); }

    public void GenerateNavMeshSurface()
    {
        foreach (NavMeshSurface surface in surfaces) surface.BuildNavMesh();
    }
}
