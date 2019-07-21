using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controladores
{
    /// <summary>
    /// Classe responsavel por controlar os pontos do jogador.
    /// </summary>
    class ControladorPontos : MonoBehaviour
    {
        /// <summary>
        /// Pontuação do jogador (valor).
        /// </summary>
        public int Ponto { get; private set; }
        /// <summary>
        /// Texto (UI) da pontuação atual.
        /// </summary>
        public Text pontuacaoJogo;
        /// <summary>
        /// Texto (UI) da pontuação atual para tela de Game Over.
        /// </summary>
        public Text pontuacaoGOAtual;
        /// <summary>
        /// Texto (UI) da pontuação recorde para tela de Game Over.
        /// </summary>
        public Text pontuacaoGORecorde;
        /// <summary>
        /// Imagem (UI) da medalha para tela de Game Over.
        /// </summary>
        public Image pontuacaoMedalha;
        /// <summary>
        /// Objeto da tela do Game Over.
        /// </summary>
        public GameObject telaGameOver;

        /// <summary>
        /// Sprites (arquivo de imagem) das medalhas.
        /// </summary>
        public Sprite[] medalhas;

        // Função que ocorre uma vez, antes do Start, quando a cena é carregada.
        void Awake()
        {
            // Verificaremos se existe um PlayerPref com o recorde do jogador, se não existir, criamos.
            if (!PlayerPrefs.HasKey("Recorde"))
            {
                PlayerPrefs.SetInt("Recorde", 0);
            }
            // Pontos iniciando em zero.
            Ponto = 0;
        }

        /// <summary>
        /// Função responsável por pontuar.
        /// </summary>
        public void Pontuar()
        {
            // Aumenta o ponto em um.
            Ponto++;
            // Altera o texto da pontuação.
            pontuacaoJogo.text = Ponto.ToString();
            // Caso a pontuação atual seja maior que o recorde.
            if (Ponto > PlayerPrefs.GetInt("Recorde"))
            {
                // O recorde passa a ter o valor da pontuação atual.
                PlayerPrefs.SetInt("Recorde", Ponto);
            }
        }

        /// <summary>
        /// Preenche a tela do Game Over com os valores atuais.
        /// </summary>
        public void PreencherGO()
        {
            // Define a tela de game over como ativada.
            telaGameOver.SetActive(true);
            // Altera o texto da pontuação atual da tela game over.
            pontuacaoGOAtual.text = Ponto.ToString();
            // Altera o texto da pontuação recorde da tela game over.
            pontuacaoGORecorde.text = PlayerPrefs.GetInt("Recorde").ToString();

            // Se a pontuação for maior que 3, terá medalha.
            if (Ponto >= 3)
            {
                // Ativaremos o objeto da medalha.
                pontuacaoMedalha.enabled = true;
                // Dividiremos o texto por 3 para ver qual das medalhas teremos (indice dela).
                int medalha = (Ponto / 3) - 1;
                // Caso o indice for maior do que a quantidade de medalhas, atribuiremos a ultima medalha.
                if (medalha > medalhas.Length - 1)
                {
                    medalha = medalhas.Length - 1;
                }
                // Colocaremos a sprite correspondente na imagem (ui) da medalha.
                pontuacaoMedalha.sprite = medalhas[medalha];
            }
            // Se for menor do que 3, não terá medalha.
            else
            {
                // Desabilitaremos o objeto da medalha.
                pontuacaoMedalha.enabled = false;
            }
        }
    }
}
