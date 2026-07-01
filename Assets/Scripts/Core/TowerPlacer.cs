using UnityEngine;
using UnityEngine.InputSystem; // Necessário para o novo Input System

public class TowerPlacer : MonoBehaviour
{
    [Header("Tower Prefab")]
    [SerializeField] private GameObject archerTowerPrefab;

    [Header("Placement Settings")]
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true;

    private GridManager gridManager;
    private Camera mainCamera;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        mainCamera = Camera.main;

        if (gridManager == null)
            Debug.LogError("TowerPlacer: GridManager não encontrado na cena!");

        if (archerTowerPrefab == null)
            Debug.LogError("TowerPlacer: ATENÇÃO! Arraste o Prefab da Torre no slot 'Archer Tower Prefab'!");
    }

    void Update()
    {
        // Clique esquerdo do mouse usando o novo Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPlaceTower();
        }
    }

    void TryPlaceTower()
    {
        Debug.Log("Mouse clicado!");

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(
            new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 
                       -mainCamera.transform.position.z));
        mouseWorldPos.z = 0;

        Debug.Log("Mouse World Pos: " + mouseWorldPos);

        Vector2Int gridPos = gridManager.WorldToGrid(mouseWorldPos);
        Debug.Log("Grid Pos: " + gridPos);

        if (!gridManager.IsValidGridPosition(gridPos))
        {
            if (showDebugMessages)
                Debug.Log("[TowerPlacer] Posição inválida (fora do grid).");
            return;
        }

        if (!gridManager.IsTileFree(gridPos))
        {
            if (showDebugMessages)
                Debug.Log("[TowerPlacer] Tile já ocupado!");
            return;
        }

        PlaceTower(gridPos);
    }

    void PlaceTower(Vector2Int gridPos)
    {
        Vector3 worldPos = gridManager.GridToWorld(gridPos.x, gridPos.y);
        GameObject newTower = Instantiate(archerTowerPrefab, worldPos, Quaternion.identity);

        GameObject towersContainer = GameObject.Find("Towers_Container");
        if (towersContainer == null)
        {
            towersContainer = new GameObject("Towers_Container");
        }
        newTower.transform.SetParent(towersContainer.transform);

        gridManager.OccupyTile(gridPos);
        newTower.name = $"ArcherTower_{gridPos.x}_{gridPos.y}";

        if (showDebugMessages)
            Debug.Log($"[TowerPlacer] Torre construída em Grid ({gridPos.x}, {gridPos.y})!");
    }

    // Script do construtor de torres (onde o jogador clica)
        void TentarConstruirTorre()
        {
            // Verifica o limite ANTES de gastar dinheiro ou instanciar a torre
            if (GerenciadorConstrucao.Instance.torresAtuais >= GerenciadorConstrucao.Instance.maximoTorres)
            {
                Debug.Log("Você já atingiu o limite de 3 torres!");
                // Toca um som de erro aqui, ou mostra uma mensagem na tela
                return; 
            }

            // Se passou do limite:
            // 1. Gasta ouro
            // 2. Instancia a torre
            // 3. Aumenta a variável: GerenciadorConstrucao.Instance.torresAtuais++;
        }
}
        