using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance; // Singleton para fácil acesso

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.red;

    private List<Transform> waypointList = new List<Transform>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Coleta automaticamente todos os filhos do objeto "Path"
        foreach (Transform child in transform)
        {
            waypointList.Add(child);
        }
        
        Debug.Log($"[Waypoints] Rota carregada com {waypointList.Count} pontos.");
    }

    /// <summary>
    /// Retorna a posição no mundo de um waypoint específico pelo índice.
    /// </summary>
    public Vector3 GetWaypointPosition(int index)
    {
        if (index >= 0 && index < waypointList.Count)
            return waypointList[index].position;
        else
            return Vector3.zero;
    }

    /// <summary>
    /// Retorna o número total de waypoints.
    /// </summary>
    public int GetTotalWaypoints()
    {
        return waypointList.Count;
    }

    /// <summary>
    /// Retorna o último waypoint (a BASE).
    /// </summary>
    public Vector3 GetEndPosition()
    {
        if (waypointList.Count > 0)
            return waypointList[waypointList.Count - 1].position;
        return Vector3.zero;
    }

    /// <summary>
    /// Retorna o primeiro waypoint (SPAWN).
    /// </summary>
    public Vector3 GetStartPosition()
    {
        if (waypointList.Count > 0)
            return waypointList[0].position;
        return Vector3.zero;
    }

    // Visualização no Editor
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;

        // Desenha linhas conectando os waypoints filhos
        Transform[] children = GetComponentsInChildren<Transform>();
        
        // Ignora o próprio objeto pai (índice 0)
        for (int i = 1; i < children.Length - 1; i++)
        {
            if (children[i] != null && children[i + 1] != null)
            {
                Gizmos.DrawLine(children[i].position, children[i + 1].position);
            }
        }
    }
}