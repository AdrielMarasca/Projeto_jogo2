using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    [Header("Targeting Settings")]
    [SerializeField] private TargetingMode targetingMode = TargetingMode.First;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject currentTarget;
    private CircleCollider2D rangeCollider;

    private enum TargetingMode
    {
        First,      // Inimigo mais próximo do FIM do caminho (prioridade padrão TD)
        Last,       // Inimigo mais próximo do SPAWN
        Strongest,  // Inimigo com mais vida (implementar depois)
        Weakest     // Inimigo com menos vida (implementar depois)
    }

    void Start()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        if (rangeCollider == null)
            Debug.LogError($"{gameObject.name}: TowerTargeting precisa de um CircleCollider2D!");
    }

    void Update()
    {
        CleanDeadEnemies();
        UpdateCurrentTarget();
    }

    // Remove inimigos mortos ou que saíram do alcance
    void CleanDeadEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);
    }

    // Escolhe o melhor alvo baseado no modo de targeting
    void UpdateCurrentTarget()
    {
        if (enemiesInRange.Count == 0)
        {
            currentTarget = null;
            return;
        }

        switch (targetingMode)
        {
            case TargetingMode.First:
                currentTarget = GetFirstEnemy();
                break;
            case TargetingMode.Last:
                currentTarget = GetLastEnemy();
                break;
            default:
                currentTarget = GetFirstEnemy();
                break;
        }
    }

    // Inimigo que percorreu MAIS do caminho (mais próximo da base)
    GameObject GetFirstEnemy()
    {
        GameObject best = enemiesInRange[0];
        float maxDistance = 0f;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            PathFollower follower = enemy.GetComponent<PathFollower>();
            if (follower != null)
            {
                float remaining = follower.GetDistanceRemaining();
                // Quanto MENOS distância restante, mais perto da base está
                if (remaining < maxDistance || best == enemiesInRange[0])
                {
                    maxDistance = remaining;
                    best = enemy;
                }
            }
        }
        return best;
    }

    // Inimigo mais próximo do spawn (menos percorreu)
    GameObject GetLastEnemy()
    {
        GameObject best = enemiesInRange[0];
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            PathFollower follower = enemy.GetComponent<PathFollower>();
            if (follower != null)
            {
                float remaining = follower.GetDistanceRemaining();
                // Quanto MAIS distância restante, mais perto do spawn está
                if (remaining > minDistance || best == enemiesInRange[0])
                {
                    minDistance = remaining;
                    best = enemy;
                }
            }
        }
        return best;
    }

    // Chamado automaticamente quando algo entra no trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
                
                if (showDebug)
                    Debug.Log($"{gameObject.name}: Inimigo detectado no alcance. Total: {enemiesInRange.Count}");
            }
        }
    }

    // Chamado automaticamente quando algo sai do trigger
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            
            if (showDebug)
                Debug.Log($"{gameObject.name}: Inimigo saiu do alcance. Total: {enemiesInRange.Count}");
        }
    }

    /// <summary>
    /// Retorna o alvo atual (chamado pelo TowerShooting).
    /// </summary>
    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    /// <summary>
    /// Retorna se há um alvo válido.
    /// </summary>
    public bool HasTarget()
    {
        return currentTarget != null;
    }

    // Visualização no Editor
    void OnDrawGizmosSelected()
    {
        if (currentTarget != null && showDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
        }
    }
}