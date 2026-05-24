using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waypointThreshold = 0.1f; // Distância para considerar "chegou ao waypoint"

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private int currentWaypointIndex = 0;
    private bool isMoving = true;
    private Waypoints waypoints;
    
    // Direção para flip do sprite (se tiver animações depois)
    private Vector2 lastDirection = Vector2.right;

    void Start()
    {
        waypoints = Waypoints.Instance;
        
        if (waypoints == null)
        {
            Debug.LogError("PathFollower: Waypoints Instance não encontrada na cena!");
            return;
        }

        // Posiciona o inimigo no primeiro waypoint (spawn)
        transform.position = waypoints.GetStartPosition();
    }

    void Update()
    {
        if (!isMoving || waypoints == null) return;

        MoveAlongPath();
        UpdateSortingOrder(); // Essencial para isométrico!
    }

    void MoveAlongPath()
    {
        // 1. Pega a posição do waypoint atual
        Vector3 targetPosition = waypoints.GetWaypointPosition(currentWaypointIndex);

        // 2. Calcula direção e move
        Vector2 direction = (targetPosition - transform.position).normalized;
        lastDirection = direction;
        
        transform.position = Vector2.MoveTowards(
            transform.position, 
            targetPosition, 
            moveSpeed * Time.deltaTime
        );

        // 3. Verifica se chegou ao waypoint atual
        float distance = Vector2.Distance(transform.position, targetPosition);
        
        if (distance < waypointThreshold)
        {
            currentWaypointIndex++; // Avança para o próximo waypoint

            // 4. Verifica se chegou ao fim (BASE)
            if (currentWaypointIndex >= waypoints.GetTotalWaypoints())
            {
                ReachedEnd();
            }
        }
    }

    void UpdateSortingOrder()
    {
        // Fórmula isométrica: quanto mais "baixo" na tela (Y menor), mais à frente
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

    void ReachedEnd()
    {
        isMoving = false;
        
        if (showDebug)
            Debug.Log($"{gameObject.name} chegou à base! Dano ao jogador!");
        
        if (GameManager.Instance != null)
        GameManager.Instance.TakeDamage(1);
        else
        Debug.LogError("PathFollower: GameManager.Instance não encontrado!");
        
        Destroy(gameObject); // Remove o inimigo
    }

    /// <summary>
    /// Retorna a distância restante até o fim do caminho (útil para torres priorizarem alvos).
    /// </summary>
    public float GetDistanceRemaining()
    {
        if (waypoints == null) return 0f;

        float distance = 0f;

        // Distância do waypoint atual até a posição do inimigo
        distance += Vector2.Distance(transform.position, 
                                     waypoints.GetWaypointPosition(currentWaypointIndex));

        // Distância entre os waypoints restantes
        for (int i = currentWaypointIndex; i < waypoints.GetTotalWaypoints() - 1; i++)
        {
            distance += Vector2.Distance(
                waypoints.GetWaypointPosition(i), 
                waypoints.GetWaypointPosition(i + 1)
            );
        }

        return distance;
    }

    // Visualização no Editor (só no Prefab Mode ou se estiver na cena)
    void OnDrawGizmos()
    {
        if (!showDebug || !Application.isPlaying) return;

        // Desenha linha até o próximo waypoint
        Gizmos.color = Color.green;
        if (waypoints != null && currentWaypointIndex < waypoints.GetTotalWaypoints())
        {
            Gizmos.DrawLine(transform.position, 
                           waypoints.GetWaypointPosition(currentWaypointIndex));
        }
    }
}