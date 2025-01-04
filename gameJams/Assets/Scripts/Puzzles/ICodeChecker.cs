namespace MyGame.Puzzles
{
    /// <summary>
    /// Kod ��kt�s�n� kontrol eden s�n�flar i�in ortak aray�z.
    /// </summary>
    public interface ICodeChecker
    {
        void CheckPuzzleOutput(string output);
        void DisplayError(string error);
        void DisplayOutput(string output);
    }
}
