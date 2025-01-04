using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Manages puzzle interactions and user inputs.
    /// </summary>
    public class PuzzleManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private Button runButton;

        [Header("Puzzle Data")]
        [SerializeField] private PuzzleData currentPuzzle;

        [Header("Compiler / Checker")]
        [SerializeField] private CCompiler compiler;
        [SerializeField] private CodeChecker codeChecker;

        private void Awake()
        {
            CodeChecker.OnPuzzleSolved += HandlePuzzleSolved;

            if (compiler == null)
                Debug.LogError("PuzzleManager: CCompiler is null!");
            if (codeChecker == null)
                Debug.LogError("PuzzleManager: CodeChecker is null!");
            if (runButton != null)
                runButton.onClick.AddListener(OnRunButtonClicked);
            else
                Debug.LogError("PuzzleManager: RunButton is null!");
        }

        private void OnDestroy()
        {
            CodeChecker.OnPuzzleSolved -= HandlePuzzleSolved;
        }

        private void Start()
        {
            DisplayPuzzleDescription();
        }

        private void DisplayPuzzleDescription()
        {
            // Burada puzzle açýklamasýný bir UI text'e yansýtabilirsiniz
            // Örneðin: puzzleDescriptionText.text = currentPuzzle.puzzleDescription;
        }

        private void OnRunButtonClicked()
        {
            if (compiler == null || codeChecker == null)
                return;

            string userCode = codeInputField.text;
            if (string.IsNullOrWhiteSpace(userCode))
            {
                codeChecker.DisplayOutput("Code input cannot be empty.");
                return;
            }

            compiler.CompileAndRun(userCode);
        }

        private void HandlePuzzleSolved(PuzzleData solvedPuzzle)
        {
            Debug.Log($"PuzzleManager: Puzzle çözüldü => {solvedPuzzle.puzzleID}");
            // Burada ek iþlemler yapabilirsiniz (yeni sahne aç, kapý aç, vb.)
        }
    }
}
