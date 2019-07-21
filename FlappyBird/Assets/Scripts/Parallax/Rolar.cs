using System;
using UnityEngine;

namespace Assets.Scripts.Parallax
{
    /// <summary>
    /// Script responsável por fazer o efeito de 'scroll' do material utilizado no parallax.
    /// </summary>
    public class Rolar : MonoBehaviour
    {
        /// <summary>
        /// Variavel correspondente a velocidade que rolaremos a textura do nosso material.
        /// </summary>
        public float velocidade = 0;

        // Função que acontece a cada frame.
        void Update()
        {
            // Pegamos o componente do material que é renderizado no nosso objeto.
            Material material = this.gameObject.GetComponent<Renderer>().material;
            // Mudaremos a posição do Offset deste material com base na velocidade definida.
            float posicao = material.mainTextureOffset.x + velocidade;
            material.mainTextureOffset = new Vector2(posicao, material.mainTextureOffset.y);
        }
    }
}
