using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPan : MonoBehaviour
{
    [Header("Movement Settings")]
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit = new Vector2(10f, 10f); // Aumente os limites!

    [Header("Zoom Settings")]
    public float zoomSpeed = 50f;
    public float minZoom = 3f;   // Zoom máximo (valor menor = mais perto)
    public float maxZoom = 10f;  // Zoom mínimo (valor maior = mais longe)

    void Update()
    {
        Vector3 pos = transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Keyboard keyboard = Keyboard.current;

        // Log para debug
        Debug.Log($"Mouse: {mousePos} | Screen: {Screen.width}x{Screen.height} | Pos: {pos}");

        bool moved = false;

        // Movimento com teclado
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            {
                pos.y += panSpeed * Time.deltaTime;
                moved = true;
                Debug.Log("Movendo para CIMA");
            }
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            {
                pos.y -= panSpeed * Time.deltaTime;
                moved = true;
                Debug.Log("Movendo para BAIXO");
            }
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                pos.x += panSpeed * Time.deltaTime;
                moved = true;
                Debug.Log("Movendo para DIREITA");
            }
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                pos.x -= panSpeed * Time.deltaTime;
                moved = true;
                Debug.Log("Movendo para ESQUERDA");
            }
        }

        // Movimento com mouse nas bordas
        if (mousePos.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
            moved = true;
            Debug.Log("Mouse na borda SUPERIOR");
        }
        if (mousePos.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
            moved = true;
            Debug.Log("Mouse na borda INFERIOR");
        }
        if (mousePos.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
            moved = true;
            Debug.Log("Mouse na borda DIREITA");
        }
        if (mousePos.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
            moved = true;
            Debug.Log("Mouse na borda ESQUERDA");
        }

        // Aplica limites (mas só se os limites forem maiores que zero!)
        if (panLimit.x > 0 && panLimit.y > 0)
        {
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);
        }

        if (moved)
            Debug.Log($"Nova posição: {pos}");

        transform.position = pos;

        // Zoom com scroll do mouse
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scrollValue) > 0.01f)
        {
        Camera.main.orthographicSize -= scrollValue * zoomSpeed * Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        Debug.Log($"Zoom: {Camera.main.orthographicSize}");
        }
    }

    
}