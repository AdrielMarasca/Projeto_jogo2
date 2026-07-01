using UnityEngine;

public class CasteloDisparo : MonoBehaviour
{
    public GameObject projetilPrefab; // Arraste a bala/bolinha aqui
    public Transform pontoDeDisparo;  // Um objeto vazio filho do castelo, na ponta dele
    public float tempoEntreTiros = 1.5f;
    public float alcance = 10f; // Distância máxima que ele atira

    private float timer;
    private Transform alvoAtual;

    void Update()
    {
        // Procura um inimigo (você precisa de uma tag "Inimigo" nos seus monstros)
        ProcurarAlvo();

        if (alvoAtual != null)
        {
            timer += Time.deltaTime;
            
            // Olha para o alvo (para 2D Top-Down, rotaciona no eixo Z)
            Vector3 direcao = alvoAtual.position - transform.position;
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo - 90); // Ajuste o -90 conforme a arte do seu castelo

            if (timer >= tempoEntreTiros)
            {
                Atirar();
                timer = 0f;
            }
        }
        else
        {
            timer = 0f; // Reseta o timer se não tiver alvo
        }
    }

    void ProcurarAlvo()
    {
        // Lógica simples: Pega o primeiro inimigo por perto (se usar OverlapCircle)
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(transform.position, alcance);
        // ... (Aqui você deve filtrar os inimigos, pegando o mais próximo ou o que chegou primeiro)
    }

    void Atirar()
    {
        Instantiate(projetilPrefab, pontoDeDisparo.position, pontoDeDisparo.rotation);
    }
}