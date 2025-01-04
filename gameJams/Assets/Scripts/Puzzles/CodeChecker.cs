using UnityEngine;
using MyGame.UI;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Program çýktýsýný kontrol eder ve bulmacanýn çözülüp çözülmediðini belirler.
    /// </summary>
    public class CodeChecker : MonoBehaviour, ICodeChecker
    {
        [Header("Puzzle Data")]
        [SerializeField] private PuzzleData currentPuzzle;

        [Header("Walls To Control")]
        [SerializeField] private MonoBehaviour[] wallsToControl;
        // Inspector'da WallController component'li objeler atarsýnýz.
        // Onlarý IMovable'a cast ederek MoveToTarget() çaðýracaðýz.

        // Puzzle çözüldüðünde haber verecek event
        public static event System.Action<PuzzleData> OnPuzzleSolved;

        public void CheckPuzzleOutput(string output)
        {
            if (currentPuzzle == null)
            {
                Debug.LogError("CodeChecker: currentPuzzle is null!");
                return;
            }

            Debug.Log($"CodeChecker: Alýnan Çýktý: '{output}'");
            Debug.Log($"CodeChecker: Beklenen Çýktý: '{currentPuzzle.expectedOutput}'");

            if (output.Trim() == currentPuzzle.expectedOutput)
            {
                SingleLineOutput.Instance.DisplayOutput(currentPuzzle.successMessage);
                TriggerWallMovements();
                OnPuzzleSolved?.Invoke(currentPuzzle);
            }
            else
            {
                SingleLineOutput.Instance.DisplayOutput(currentPuzzle.failMessage);
            }
        }

        private void TriggerWallMovements()
        {
            foreach (var wallObj in wallsToControl)
            {
                if (wallObj is MyGame.Walls.IMovable movableWall)
                {
                    movableWall.MoveToTarget();
                }
            }
        }

        public void DisplayError(string error)
        {
            SingleLineOutput.Instance?.DisplayOutput("Error: " + error);
        }

        public void DisplayOutput(string output)
        {
            SingleLineOutput.Instance?.DisplayOutput(output);
        }
    }
}
