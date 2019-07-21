using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controladores;
using UnityEngine;

/// <summary>
/// Classe responsável por controlar nosso personagem.
/// </summary>
public class ControladorPersonagem : MonoBehaviour
{
    /// <summary>
    /// Objeto controlador da nossa cena.
    /// </summary>
    private GameObject controlador;
    /// <summary>
    /// Vector2 com a força em X e Y que vamos aplicar no personagem. 
    /// </summary>
    [SerializeField] private Vector2 voo = new Vector2(0, 3500);
    /// <summary>
    /// Booleano se o pássaro pode ou não jogar.
    /// </summary>
    private bool podeVoar = true;
    /// <summary>
    /// Booleano se estamos ainda em jogo ou já perdemos. Ex: Pause ou Game Over.
    /// </summary>
    private bool jogando = true;

    // Use this for initialization
    void Start()
    {
        // Procuramos na tela por um objeto com nome 'Controlador'.
        controlador = GameObject.Find("Controlador");
    }

    // Função que acontece a cada frame.
    void Update()
    {
        // Verificaremos se a tela foi tocada, ou o mouse foi precionado. Também veremos se podemos voar e se estamos jogando.
        if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && jogando && podeVoar)
        {
            // Se o mouse foi pressionado (é necessário para não dar erro de não existir a posição 0 nos toques, caso for testado pelo computador) ou se estamos na fase de inicio do toque na tela.
            if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Caso o jogo ainda não tiver sido iniciado, vamos iniciar.
                if (!controlador.GetComponent<ControladorJogo>().jogoIniciado)
                {
                    controlador.GetComponent<ControladorJogo>().IniciarJogo();
                }
                else // Caso já estiver sido iniciado.
                {
                    // Se ainda estivermos no estado cinemático, trocaremos para dinâmico e em seguida iremos aplicar uma ação de Voo.
                    if (this.GetComponent<Rigidbody2D>().isKinematic)
                    {
                        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        Voar();
                    }
                    else // Caso já não tiver cinemático, apenas voamos.flappy
                    {
                        Voar();
                    }
                }

            }
        }

        // Pegaremos o angulo em Euler ( para ser igual ao que usamos diariamente ) e se for maior de 30 graus e for positivo, mudamos para apenas 30 graus.
        if (this.transform.rotation.eulerAngles.z > 30.0f && this.transform.rotation.eulerAngles.z <= 180 && jogando && podeVoar)
        {
            transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 30.0f);

        }
        // Se não, se for maior que 30 graus negativos, voltamos para 30 graus negativo.
        else if (this.transform.rotation.eulerAngles.z > 180.0f && this.transform.rotation.eulerAngles.z < 330 && jogando && podeVoar)
        {
            transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, -30.0f);
        }

        // Se estiver caindo (velocidade negativa), vamos rotacionando o pássaro para baixo.
        if (this.GetComponent<Rigidbody2D>().velocity.y < 0 && jogando && podeVoar)
        {
            this.GetComponent<Rigidbody2D>().rotation -= 1f;
        }

        // Se não poder mais voar (colidiu com um cano) e estiver caindo com angulo maior que 90 graus negativos, vamos rotacionar para 90 graus negativos.
        if (!podeVoar && Math.Abs(transform.rotation.eulerAngles.z - 270f) > 0.1f)
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 270.0f);
        }
    }

    /// <summary>
    /// Função
    /// </summary>
    private void Voar()
    {
        // Caso ainda poder mesmo voar.
        if (podeVoar)
        {
            // Vamos pegar o componente Rigidbody2D do personagem.
            Rigidbody2D passaro = this.GetComponent<Rigidbody2D>();
            // Vamos zerar a velocidade.
            passaro.velocity = Vector2.zero;
            // Caso a rotação for negativa, vamos transformar em positiva.
            if (passaro.rotation <= 0)
                passaro.rotation = 7.5f;
            // Em seguida vamos aumentar em mais 7.5 graus.
            passaro.rotation += 7.5f;
            // Adicionaremos a força do voo.
            passaro.AddForce(voo);
            // Tocará o som de voo.
            GetComponent<AudioSource>().Play();
        }
    }

    // Função própria da Unity para quando ocorre uma colisão.
    void OnCollisionEnter2D(Collision2D other)
    {
        // Se ainda puder jogar.
        if (jogando)
        {
            // Se for o objeto com nome FundoChao, vamos finalizar a jogada.
            if (other.gameObject.name == "FundoChao")
            {
                // Checaremos se ainda pode voar, para quando o jogador caiu no chão antes de colidir com um cano
                if (podeVoar)
                {
                    // Iremos executar a função de parar a jogada.
                    controlador.GetComponent<ControladorJogo>().PararJogo();
                    // Vamos rotacionar o nosso pássaro para -90 graus.
                    this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 270.0f);
                    // Iremos parar com a animação de bater asas.
                    this.GetComponent<Animator>().enabled = false;
                }
                // Primeiro o pássaro fica com modo estático.
                this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                // Depois vamos definir o booleano Jogando como falso.
                jogando = false;
                // Em seguida tocaremos o som de fim de jogo.
                other.transform.GetComponent<AudioSource>().Play();
                // Por fim preencheremos a tela de Game Over.
                controlador.GetComponent<ControladorPontos>().PreencherGO();
            }
            // Se tiver relado em um dos canos.
            else if (other.gameObject.tag == "Canos" | other.gameObject.tag == "Limite")
            {
                // Definimos que não se pode mais voar
                podeVoar = false;
                // Iremos executar a função de parar a jogada.
                controlador.GetComponent<ControladorJogo>().PararJogo();
                // Para cada filho do objeto colidido, vamos desabilitar os colisores.
                foreach (Transform child in other.transform.parent)
                {
                    child.transform.GetComponent<BoxCollider2D>().enabled = false;

                }
                // Vamos rotacionar o nosso pássaro para -90 graus.
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, 270.0f);
                // Iremos parar com a animação de bater asas.
                this.GetComponent<Animator>().enabled = false;
                // Por fim, iremos tocar o som de colisão com um cano.
                other.transform.GetComponent<AudioSource>().Play();
            }
        }


    }
}
