using UnityEngine;
using UnityEngine.Events; // Para eventos de Game Over

public class CasteloVida : MonoBehaviour
{
    [Header("Atributos")]
    public int vidaMaxima = 100;
    private int vidaAtual;

    [Header("Eventos")]
    public UnityEvent aoTomarDano; // Para tocar som, animação de tremer, etc.
    public UnityEvent aoDestruir;  // Para tela de Game Over

    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    public void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        aoTomarDano.Invoke(); // Dispara o evento

        // Atualizar barra de vida na UI aqui...

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            DestruirCastelo();
        }
    }

    private void DestruirCastelo()
    {
        aoDestruir.Invoke();
        // Desativar o objeto ou mostrar animação de explosão
        gameObject.SetActive(false); 
    }
}