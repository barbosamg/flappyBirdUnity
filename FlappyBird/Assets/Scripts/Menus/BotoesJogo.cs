using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script responsável por dar funções aos botões do jogo
/// </summary>
public class BotoesJogo : MonoBehaviour
{
    /// <summary>
    /// Função que vai para a cena do Jogo
    /// </summary>
    public void Jogar()
    {
        SceneManager.LoadScene("Jogo");
    }

    /// <summary>
    /// Função que vai para a cena do Menu;
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
