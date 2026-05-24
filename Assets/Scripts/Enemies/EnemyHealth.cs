using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 30f;

    [Header("Reward")]
    [SerializeField] private int goldReward = 10;

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Aplica dano ao inimigo. Retorna true se o inimigo morreu.
    /// </summary>
    public bool TakeDamage(float damageAmount)
    {
        if (isDead) return false;

        currentHealth -= damageAmount;

        if (showDebugMessages)
            Debug.Log($"{gameObject.name} recebeu {damageAmount} de dano. Vida restante: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (showDebugMessages)
            Debug.Log($"{gameObject.name} morreu! Dropa {goldReward} de ouro.");

        // FUTURO: Adicionar ouro ao GameManager
        // GameManager.Instance.AddGold(goldReward);

        // FUTURO: Tocar animação de morte, partículas, som...

        Destroy(gameObject);
    }

    /// <summary>
    /// Retorna a vida atual (útil para UI de barra de vida depois).
    /// </summary>
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Retorna a vida máxima.
    /// </summary>
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Verifica se o inimigo já está morto.
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }
}