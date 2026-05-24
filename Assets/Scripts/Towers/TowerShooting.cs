using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackSpeed = 1f; // Tiros por segundo
    [SerializeField] private float attackDamage = 10f;

    [Header("Debug")]
    [SerializeField] private bool showDebug = true;

    private TowerTargeting targeting;
    private float timeSinceLastAttack = 0f;
    private float attackCooldown;

    void Start()
    {
        targeting = GetComponent<TowerTargeting>();
        if (targeting == null)
            Debug.LogError($"{gameObject.name}: TowerShooting precisa de um TowerTargeting!");

        // Calcula o cooldown baseado na velocidade de ataque
        attackCooldown = 1f / attackSpeed;
        timeSinceLastAttack = attackCooldown; // Permite atirar imediatamente
    }

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (CanShoot())
        {
            Shoot();
        }
    }

    bool CanShoot()
    {
        // Precisa ter um alvo, cooldown completo e prefab configurado
        return targeting.HasTarget() && 
               timeSinceLastAttack >= attackCooldown && 
               projectilePrefab != null;
    }

    void Shoot()
    {
        GameObject target = targeting.GetCurrentTarget();
        if (target == null) return;

        // Reseta o cooldown
        timeSinceLastAttack = 0f;

        // Instancia o projétil na posição da torre
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Configura o projétil
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(target, attackDamage);
        }

        if (showDebug)
            Debug.Log($"{gameObject.name}: PEW! Projétil disparado em direção a {target.name}");
    }
}