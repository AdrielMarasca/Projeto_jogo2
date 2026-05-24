using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float destroyAfterSeconds = 5f; // Segurança: autodestrói se errar

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private GameObject target;
    private float damage;
    private bool isInitialized = false;

    /// <summary>
    /// Chamado pelo TowerShooting para configurar o projétil.
    /// </summary>
    public void Initialize(GameObject enemyTarget, float projectileDamage)
    {
        target = enemyTarget;
        damage = projectileDamage;
        isInitialized = true;

        // Autodestruição de segurança
        Destroy(gameObject, destroyAfterSeconds);
    }

    void Update()
    {
        if (!isInitialized) return;

        // Se o alvo foi destruído, destrói o projétil também
        if (target == null)
        {
            if (showDebug)
                Debug.Log($"{gameObject.name}: Alvo desapareceu. Projétil destruído.");

            Destroy(gameObject);
            return;
        }

        // Move em direção ao alvo
        Vector2 direction = (target.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.transform.position,
            moveSpeed * Time.deltaTime
        );

        // Rotaciona o projétil para apontar na direção do alvo (opcional, visual)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Atualiza sorting order isométrica
        UpdateSortingOrder();
    }

    void UpdateSortingOrder()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com o alvo pretendido
        if (target != null && other.gameObject == target)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (showDebug)
            Debug.Log($"{gameObject.name}: Acertou {target.name}! Dano: {damage}");

        // FUTURO: Causar dano real no inimigo
        // EnemyHealth health = target.GetComponent<EnemyHealth>();
        // if (health != null) health.TakeDamage(damage);

        // Por enquanto, só destrói o alvo (placeholder)
        Destroy(target);
        
        // Destrói o projétil
        Destroy(gameObject);
    }
}