using UnityEngine;
using System.Collections.Generic;

public class GerenciadorConstrucao : MonoBehaviour
{
    public static GerenciadorConstrucao Instance; // Torna ele acessível de qualquer lugar

    [Header("Limites")]
    public int maximoTorres = 3;

    [HideInInspector]
    public int torresAtuais = 0;

    void Awake()
    {
        // Padrão Singleton para garantir que só existe um gerenciador
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}