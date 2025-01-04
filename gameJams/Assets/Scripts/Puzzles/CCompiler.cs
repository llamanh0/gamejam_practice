using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MyGame.UI;
using Debug = UnityEngine.Debug;

namespace MyGame.Puzzles
{
    /// <summary>
    /// C kodunu derler ve �al��t�r�r, ard�ndan sonu�lar� CodeChecker'a (ICodeChecker) iletir.
    /// </summary>
    public class CCompiler : MonoBehaviour
    {
        [Header("Checker (Interface Reference)")]
        [SerializeField] private MonoBehaviour codeCheckerObject;
        private ICodeChecker codeChecker;

        [Header("Puzzle Settings")]
        [SerializeField] private PuzzleData currentPuzzle;
        [SerializeField] private CompilerSettings compilerSettings;

        private void Awake()
        {
            codeChecker = codeCheckerObject as ICodeChecker;
            if (codeChecker == null)
            {
                Debug.LogError("CCompiler: codeCheckerObject bir ICodeChecker de�il!");
            }

            if (currentPuzzle == null)
            {
                Debug.LogError("CCompiler: currentPuzzle is null!");
            }

            if (compilerSettings == null)
            {
                Debug.LogError("CCompiler: compilerSettings is null!");
            }
        }

        public async void CompileAndRun(string userCode)
        {
            if (codeChecker == null || currentPuzzle == null || compilerSettings == null)
            {
                Debug.LogError("CCompiler: Eksik referanslar!");
                return;
            }

            Debug.Log("Derleme i�lemi ba�lat�l�yor...");

            if (!IsCodeSafe(userCode))
            {
                codeChecker.DisplayError("Hata: G�venli olmayan kod tespit edildi.");
                return;
            }

            string compilerPath = compilerSettings.GetCompilerPath();
            if (!File.Exists(compilerPath))
            {
                codeChecker.DisplayError("Hata: Derleyici bulunamad�.");
                return;
            }

            string tempDir = Path.Combine(Application.temporaryCachePath, "CCompiler");
            Directory.CreateDirectory(tempDir);

            string sourcePath = Path.Combine(tempDir, "temp_code.c");
            string executablePath = Path.Combine(tempDir, "temp_program");

#if UNITY_STANDALONE_WIN
            executablePath += ".exe";
#endif

            string fullCode = CodeTemplate.GetFullCode(currentPuzzle.puzzleID, userCode);
            await File.WriteAllTextAsync(sourcePath, fullCode);
            Debug.Log($"C kodu ge�ici dosyaya yaz�ld�: {sourcePath}");

            string compileArguments = compilerSettings.GetCompileArguments(sourcePath, executablePath);
            var compileResult = await RunProcessAsync(compilerPath, compileArguments, compilerSettings.CompilerWorkingDirectory);

            if (compileResult.ExitCode == 0)
            {
                Debug.Log("Derleme ba�ar�l�.");
                codeChecker.DisplayOutput(currentPuzzle.successMessage);
                await RunExecutableAsync(executablePath);
            }
            else
            {
                Debug.LogError($"Derleme hatas�: {compileResult.StandardError}");
                codeChecker.DisplayError($"Derleme Hatalar�:\n{compileResult.StandardError}");
            }
        }

        private async Task RunExecutableAsync(string executablePath)
        {
            if (!File.Exists(executablePath))
            {
                codeChecker.DisplayError("Hata: �al��t�r�labilir dosya bulunamad�.");
                return;
            }

            var runResult = await RunProcessAsync(executablePath, string.Empty, Path.GetDirectoryName(executablePath));

            if (runResult.ExitCode == 0)
            {
                string trimmedOutput = runResult.StandardOutput.Trim();
                Debug.Log($"Program ba�ar�yla �al��t�r�ld�. ��kt�: {trimmedOutput}");
                codeChecker.DisplayOutput(trimmedOutput);
                codeChecker.CheckPuzzleOutput(trimmedOutput);
            }
            else
            {
                Debug.LogError($"Program hatal� bitti: {runResult.StandardError}");
                codeChecker.DisplayError($"Program Hatalar�:\n{runResult.StandardError}");
            }
        }

        private async Task<ProcessResult> RunProcessAsync(string fileName, string arguments, string workingDirectory)
        {
            return await Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    return new ProcessResult
                    {
                        ExitCode = process.ExitCode,
                        StandardOutput = stdout,
                        StandardError = stderr
                    };
                }
            });
        }

        private bool IsCodeSafe(string code)
        {
            string[] forbiddenFunctions = { "system", "exec", "fork", "kill" };
            foreach (var func in forbiddenFunctions)
            {
                if (code.Contains(func + "("))
                {
                    return false;
                }
            }
            return true;
        }

        private class ProcessResult
        {
            public int ExitCode;
            public string StandardOutput;
            public string StandardError;
        }
    }
}
