using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Dimensions")]
    public int gridWidth = 10;   // Número de colunas do grid
    public int gridHeight = 10;  // Número de linhas do grid
    public float tileWidth = 1f;  // Largura de cada tile em unidades de mundo
    public float tileHeight = 0.5f; // Altura de cada tile em unidades de mundo

    [Header("Tilemap Offset")]
    public Vector3 originPosition; // Ponto de origem do grid no mundo (base para as conversões)

    void Start()
    {
        // Usa a posição do GameObject como origem do grid ao iniciar
        originPosition = transform.position;
    }

    /// <summary>
    /// Converte coordenadas de grid (coluna, linha) para posição no mundo.
    /// Funciona para um grid isométrico com tiles em forma de losango.
    /// </summary>
    /// <param name="col">Índice da coluna no grid.</param>
    /// <param name="row">Índice da linha no grid.</param>
    /// <returns>Posição em coordenadas do mundo correspondente ao tile.</returns>
    public Vector3 GridToWorld(int col, int row)
    {
        // A coordenada X no mundo depende da diferença entre coluna e linha
        float worldX = (col - row) * (tileWidth / 2f);

        // A coordenada Y no mundo depende da soma de coluna e linha
        float worldY = (col + row) * (tileHeight / 2f);

        // Ajusta pela posição de origem do grid
        return originPosition + new Vector3(worldX, worldY, 0);
    }

    /// <summary>
    /// Converte uma posição no mundo para coordenadas de grid.
    /// Isso é útil para detectar qual tile foi clicado ou qual tile um objeto ocupa.
    /// </summary>
    /// <param name="worldPosition">Posição em coordenadas do mundo.</param>
    /// <returns>Coordenadas de grid arredondadas (coluna, linha).</returns>
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        // Converte a posição global para uma posição relativa à origem do grid
        Vector3 relativePos = worldPosition - originPosition;
        
        // Fórmula inversa da projeção isométrica usada em GridToWorld
        float colFloat = (relativePos.x / (tileWidth / 2f) + relativePos.y / (tileHeight / 2f)) / 2f;
        float rowFloat = (relativePos.y / (tileHeight / 2f) - relativePos.x / (tileWidth / 2f)) / 2f;
        
        // Arredonda para o tile mais próximo
        int col = Mathf.RoundToInt(colFloat);
        int row = Mathf.RoundToInt(rowFloat);
        
        return new Vector2Int(col, row);
    }

    /// <summary>
    /// Verifica se uma posição de grid está dentro dos limites definidos.
    /// </summary>
    /// <param name="gridPos">Posição de grid a ser verificada.</param>
    /// <returns>True se estiver dentro do grid; caso contrário, false.</returns>
    public bool IsValidGridPosition(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < gridWidth && 
               gridPos.y >= 0 && gridPos.y < gridHeight;
    }

    // Desenha o grid no Editor com gizmos para visualizar a posição de cada tile.
    void OnDrawGizmos()
    {
        // Atualiza a origem quando não estiver em execução,
        // permitindo ajustar o GameObject no Editor e ver o grid correto.
        if (!Application.isPlaying) originPosition = transform.position;
        
        Gizmos.color = Color.yellow;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPos = GridToWorld(x, y);
                // Desenha um wireframe de cubo achatado para visualizar cada tile
                Gizmos.DrawWireCube(worldPos, new Vector3(tileWidth, tileHeight, 0.1f));
            }
        }
    }
}