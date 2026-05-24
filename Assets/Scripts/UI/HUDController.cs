using TMPro; // Ou using UnityEngine.UI; se usar Text normal
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText; // Ou Text
    [SerializeField] private TextMeshProUGUI livesText; // Ou Text

    void Update()
    {
        if (GameManager.Instance != null)
        {
            goldText.text = "Ouro: " + GameManager.Instance.GetGold();
            livesText.text = "Vidas: " + GameManager.Instance.GetLives();
        }
    }
}