using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Puzzles
{
    /// <summary>
    /// Farklý puzzle'lar için dinamik kod template'leri saðlar.
    /// Kullanýcýnýn yazdýðý kodu belirli bir þablonun içine yerleþtirir.
    /// </summary>
    public static class CodeTemplate
    {
        private static readonly Dictionary<string, string> templates = new Dictionary<string, string>
        {
            {
                "1",
                "#include <stdio.h>\n#include <string.h>\n#include <stdlib.h>\n#include <stdbool.h>\n\nint main() {\n    // GOREV 1\n    // -> Ekrana cikti olarak sifreyi yaz.\n    // -> sifre : CrimeOfComputer\n\n    %USER_CODE%\n\n    return 0;\n}"
            },
            {
                "2",
                "#include <stdio.h>\n#include <math.h>\n\nint main() {\n    // GOREV 2\n    // -> Ekrana cikti olarak pi sayisini yaz.\n    // -> pi : 3.14159\n\n    %USER_CODE%\n\n    return 0;\n}"
            }
            // Baþka puzzle'lar eklenebilir
        };

        public static string GetFullCode(string puzzleId, string userCode)
        {
            if (templates.ContainsKey(puzzleId))
            {
                return templates[puzzleId].Replace("%USER_CODE%", userCode);
            }
            else
            {
                Debug.LogError($"CodeTemplate: Belirtilen puzzle ID'si bulunamadý: {puzzleId}");
                throw new System.NotImplementedException("Belirtilen puzzle ID'si için kod template'i yok.");
            }
        }
    }
}
