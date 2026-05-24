using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private int startingLives = 20;
    [SerializeField] private int startingGold = 100;

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    private int currentLives;
    private int currentGold;

    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentLives = startingLives;
        currentGold = startingGold;

        if (showDebugMessages)
            Debug.Log($"GameManager iniciado. Vidas: {currentLives}, Ouro: {currentGold}");
    }

    // --- Métodos de Ouro ---
    public void AddGold(int amount)
    {
        currentGold += amount;
        if (showDebugMessages)
            Debug.Log($"Ouro adicionado: +{amount}. Total: {currentGold}");
        // FUTURO: Atualizar HUD
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            if (showDebugMessages)
                Debug.Log($"Ouro gasto: -{amount}. Restante: {currentGold}");
            return true;
        }
        else
        {
            if (showDebugMessages)
                Debug.Log("Ouro insuficiente!");
            return false;
        }
    }

    public int GetGold() => currentGold;

    // --- Métodos de Vida ---
    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        if (showDebugMessages)
            Debug.Log($"Dano recebido: -{damage} vida(s). Vidas restantes: {currentLives}");

        if (currentLives <= 0)
        {
            currentLives = 0;
            GameOver();
        }
        // FUTURO: Atualizar HUD
    }

    public int GetLives() => currentLives;

    void GameOver()
    {
        if (showDebugMessages)
            Debug.Log("GAME OVER! Você perdeu todas as vidas.");
        // FUTURO: Tela de Game Over, reiniciar cena, etc.
        Time.timeScale = 0; // Pausa o jogo (simples placeholder)
    }

    public void Victory()
{
    if (showDebugMessages)
        Debug.Log("VITÓRIA! Você defendeu o castelo com sucesso!");
    Time.timeScale = 0;
}
}