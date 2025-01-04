using TMPro;
using UnityEngine;
using MyGame.Core.Utilities;

namespace MyGame.UI
{
    /// <summary>
    /// Tek sat�rl�k mesajlar� UI'da g�r�nt�ler. 
    /// �rnek Singleton kullan�m�.
    /// </summary>
    public class SingleLineOutput : Singleton<SingleLineOutput>
    {
        [SerializeField] private TMP_Text outputText;

        protected override void Awake()
        {
            base.Awake();
            if (outputText == null)
            {
                Debug.LogError("SingleLineOutput: OutputText referans� atanmam��!");
            }
        }

        /// <summary>
        /// UI'da bir mesaj g�r�nt�ler.
        /// </summary>
        public void DisplayOutput(string message)
        {
            if (outputText != null)
            {
                outputText.text = message;
            }
            else
            {
                Debug.LogError("SingleLineOutput: OutputText referans� eksik!");
            }
        }
    }
}
