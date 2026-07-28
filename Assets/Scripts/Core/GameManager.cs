using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private int startingLives = 20;
    [SerializeField] private int startingGold = 100;

    [Header("Tower Limits")]
    [SerializeField] private int maxTowers = 3; // Limite máximo
    private int currentTowers = 0; // Contador atual

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    private int currentLives;
    private int currentGold;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = startingLives;
        currentGold = startingGold;
        currentTowers = 0; 

        if (showDebugMessages)
            Debug.Log($"GameManager iniciado. Vidas: {currentLives}, Ouro: {currentGold}");
    }

    // --- MÉTODOS DE OURO (Já estavam prontos) ---
    public void AddGold(int amount) { currentGold += amount; /* Atualizar UI aqui */ }
    
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            return true;
        }
        return false;
    }
    public int GetGold() => currentGold;

    // --- MÉTODOS DE VIDA (Já estavam prontos) ---
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives <= 0) { currentLives = 0; GameOver(); }
    }
    public int GetLives() => currentLives;

    // --- NOVOS MÉTODOS PARA AS TORRES ---
    public bool CanPlaceTower()
    {
        return currentTowers < maxTowers;
    }

    public void RegisterTower()
    {
        currentTowers++;
        if (showDebugMessages) Debug.Log($"Torre construída. ({currentTowers}/{maxTowers})");
    }

    public void UnregisterTower() // Use isso se a torre for destruída
    {
        currentTowers--;
    }

    // --- GAME OVER E VITÓRIA ---
    void GameOver()
    {
        Debug.Log("GAME OVER!");
        Time.timeScale = 0;
    }
    public void Victory()
    {
        Debug.Log("VITÓRIA!");
        Time.timeScale = 0;
    }
}

