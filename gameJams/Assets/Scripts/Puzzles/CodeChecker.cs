using UnityEngine;
using MyGame.UI;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Program ��kt�s�n� kontrol eder ve bulmacan�n ��z�l�p ��z�lmedi�ini belirler.
    /// </summary>
    public class CodeChecker : MonoBehaviour, ICodeChecker
    {
        [Header("Puzzle Data")]
        [SerializeField] private PuzzleData currentPuzzle;

        [Header("Walls To Control")]
        [SerializeField] private MonoBehaviour[] wallsToControl;
        // Inspector'da WallController component'li objeler atars�n�z.
        // Onlar� IMovable'a cast ederek MoveToTarget() �a��raca��z.

        // Puzzle ��z�ld���nde haber verecek event
        public static event System.Action<PuzzleData> OnPuzzleSolved;

        public void CheckPuzzleOutput(string output)
        {
            if (currentPuzzle == null)
            {
                Debug.LogError("CodeChecker: currentPuzzle is null!");
                return;
            }

            Debug.Log($"CodeChecker: Al�nan ��kt�: '{output}'");
            Debug.Log($"CodeChecker: Beklenen ��kt�: '{currentPuzzle.expectedOutput}'");

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
