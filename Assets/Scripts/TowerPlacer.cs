using UnityEngine;

/// <summary>
/// Classe responsável por permitir que o jogador coloque torres no grid clicando.
/// Utiliza o <see cref="GridManager"/> para conversão de coordenadas e validações.
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    [Header("Tower Prefab")]
    [SerializeField] private GameObject archerTowerPrefab; // Prefab da torre a ser instanciada

    [Header("Placement Settings")]
    [SerializeField] private LayerMask groundLayerMask; // Opcional: filtrar clique só no chão (não usado neste script)

    [Header("Debug")]
    [SerializeField] private bool showDebugMessages = true; // Ativa mensagens de debug no console

    // Referências internas
    private GridManager gridManager; // Gerenciador do grid na cena
    private Camera mainCamera; // Câmera principal usada para conversão de coordenadas

    void Start()
    {
        // Busca referências necessárias na cena
        gridManager = FindObjectOfType<GridManager>();
        mainCamera = Camera.main;

        // Validações para auxiliar durante desenvolvimento
        if (gridManager == null)
            Debug.LogError("TowerPlacer: GridManager não encontrado na cena!");

        if (archerTowerPrefab == null)
            Debug.LogError("TowerPlacer: ATENÇÃO! Arraste o Prefab da Torre no slot 'Archer Tower Prefab'!");
    }

    void Update()
    {
        // Ao detectar clique esquerdo do mouse, tenta posicionar uma torre
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceTower();
        }
    }

    /// <summary>
    /// Fluxo principal para tentativa de colocação de torre a partir da posição do mouse.
    /// Faz conversão de mundo -> grid, valida limites e ocupação do tile.
    /// </summary>
    void TryPlaceTower()
    {
        // 1. Converte posição do mouse (tela) para mundo
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Zera o eixo Z (jogo 2D em planos XY)

        // 2. Converte posição do mundo para coordenada de grid
        Vector2Int gridPos = gridManager.WorldToGrid(mouseWorldPos);

        if (showDebugMessages)
            Debug.Log($"[TowerPlacer] Clique em Grid ({gridPos.x}, {gridPos.y})");

        // 3. Verifica se a posição está dentro dos limites do grid
        if (!gridManager.IsValidGridPosition(gridPos))
        {
            if (showDebugMessages)
                Debug.Log("[TowerPlacer] Posição inválida (fora do grid).");
            return;
        }

        // 4. Verifica se o tile está livre para construção
        if (!gridManager.IsTileFree(gridPos))
        {
            if (showDebugMessages)
                Debug.Log("[TowerPlacer] Tile já ocupado!");
            return;
        }

        // 5. Realiza a construção da torre
        PlaceTower(gridPos);
    }

    /// <summary>
    /// Instancia o prefab da torre na posição correspondente ao tile, organiza a hierarquia
    /// e marca o tile como ocupado no GridManager.
    /// </summary>
    /// <param name="gridPos">Coordenada do tile onde a torre será colocada.</param>
    void PlaceTower(Vector2Int gridPos)
    {
        // Converte coordenada de grid para posição no mundo
        Vector3 worldPos = gridManager.GridToWorld(gridPos.x, gridPos.y);

        // Instancia o prefab da torre
        GameObject newTower = Instantiate(archerTowerPrefab, worldPos, Quaternion.identity);

        // Agrupa todas as torres em um container chamado "Towers_Container" para organização
        GameObject towersContainer = GameObject.Find("Towers_Container");
        if (towersContainer == null)
        {
            towersContainer = new GameObject("Towers_Container");
        }
        newTower.transform.SetParent(towersContainer.transform);

        // Marca o tile como ocupado no GridManager para evitar construções múltiplas
        gridManager.OccupyTile(gridPos);

        // Ajusta nome do objeto para facilitar identificação durante debug
        newTower.name = $"ArcherTower_{gridPos.x}_{gridPos.y}";

        if (showDebugMessages)
            Debug.Log($"[TowerPlacer] Torre construída em Grid ({gridPos.x}, {gridPos.y})!");
    }
}