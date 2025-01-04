using TMPro;
using UnityEngine;
using MyGame.Core.Utilities;

namespace MyGame.UI
{
    /// <summary>
    /// Tek satýrlýk mesajlarý UI'da görüntüler. 
    /// Örnek Singleton kullanýmý.
    /// </summary>
    public class SingleLineOutput : Singleton<SingleLineOutput>
    {
        [SerializeField] private TMP_Text outputText;

        protected override void Awake()
        {
            base.Awake();
            if (outputText == null)
            {
                Debug.LogError("SingleLineOutput: OutputText referansý atanmamýþ!");
            }
        }

        /// <summary>
        /// UI'da bir mesaj görüntüler.
        /// </summary>
        public void DisplayOutput(string message)
        {
            if (outputText != null)
            {
                outputText.text = message;
            }
            else
            {
                Debug.LogError("SingleLineOutput: OutputText referansý eksik!");
            }
        }
    }
}
