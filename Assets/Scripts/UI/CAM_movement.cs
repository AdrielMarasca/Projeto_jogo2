using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPan : MonoBehaviour
{
    [Header("Movement Settings")]
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    // Substituí o Vector2 por variáveis individuais para ficar mais claro
    public float limitX = 15f; // Ajuste esses valores arrastando a câmera no Editor
    public float limitY = 10f; 

    [Header("Zoom Settings")]
    public float zoomSpeed = 50f;
    public float minZoom = 3f;   
    public float maxZoom = 10f;  

    void Update()
    {
        Vector3 pos = transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Keyboard keyboard = Keyboard.current;

        // --- MOVIMENTO ---

        // Teclado (WASD / Setas)
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                pos.y += panSpeed * Time.deltaTime;
            
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                pos.y -= panSpeed * Time.deltaTime;
            
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                pos.x += panSpeed * Time.deltaTime;
            
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                pos.x -= panSpeed * Time.deltaTime;
        }

        // Mouse nas bordas
        if (mousePos.y >= Screen.height - panBorderThickness)
            pos.y += panSpeed * Time.deltaTime;
        
        if (mousePos.y <= panBorderThickness)
            pos.y -= panSpeed * Time.deltaTime;
        
        if (mousePos.x >= Screen.width - panBorderThickness)
            pos.x += panSpeed * Time.deltaTime;
        
        if (mousePos.x <= panBorderThickness)
            pos.x -= panSpeed * Time.deltaTime;

        // --- LIMITAÇÃO DO MAPA (Aqui é a parte que estava faltando) ---
        // Isso trava a câmera para não sair do chão marrom
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        pos.y = Mathf.Clamp(pos.y, -limitY, limitY);

        // Aplica a posição final da câmera
        transform.position = pos;

        // --- ZOOM ---
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scrollValue) > 0.01f)
        {
            Camera.main.orthographicSize -= scrollValue * zoomSpeed * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }
}