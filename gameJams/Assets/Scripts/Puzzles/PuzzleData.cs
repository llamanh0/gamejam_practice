using UnityEngine;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Her bir bulmaca i�in gerekli verileri saklar.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPuzzleData", menuName = "Puzzles/PuzzleData", order = 1)]
    public class PuzzleData : ScriptableObject
    {
        [TextArea]
        [Tooltip("Bulmacan�n a��klamas� veya ipu�lar�.")]
        public string puzzleDescription;

        [Tooltip("Beklenen program ��kt�s�.")]
        public string expectedOutput;

        [Tooltip("Bulmaca ba�ar� mesaj�.")]
        public string successMessage;

        [Tooltip("Bulmaca ba�ar�s�zl�k mesaj�.")]
        public string failMessage;

        [Header("Optional Fields")]
        [Tooltip("Bulmaca tamamland���nda a��lacak objeler.")]
        public GameObject unlockableObject;

        [Tooltip("Bulmaca i�in benzersiz kimlik.")]
        public string puzzleID;
    }
}
