using UnityEngine;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Her bir bulmaca için gerekli verileri saklar.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPuzzleData", menuName = "Puzzles/PuzzleData", order = 1)]
    public class PuzzleData : ScriptableObject
    {
        [TextArea]
        [Tooltip("Bulmacanýn açýklamasý veya ipuçlarý.")]
        public string puzzleDescription;

        [Tooltip("Beklenen program çýktýsý.")]
        public string expectedOutput;

        [Tooltip("Bulmaca baþarý mesajý.")]
        public string successMessage;

        [Tooltip("Bulmaca baþarýsýzlýk mesajý.")]
        public string failMessage;

        [Header("Optional Fields")]
        [Tooltip("Bulmaca tamamlandýðýnda açýlacak objeler.")]
        public GameObject unlockableObject;

        [Tooltip("Bulmaca için benzersiz kimlik.")]
        public string puzzleID;
    }
}
