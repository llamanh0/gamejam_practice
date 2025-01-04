using UnityEngine;
using TMPro;

/// <summary>
/// Oyuncu ad�n�n girilmesini ve PlayerPrefs'e kaydedilmesini sa�lar.
/// Bo� b�rak�l�rsa varsay�lan bir isim atar.
/// </summary>
public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string defaultName = "Cabbar";

    public void SavePlayerName()
    {
        if (inputField == null)
        {
            Debug.LogError("PlayerNameInput: InputField referans� atanmam��!");
            return;
        }

        string playerName = inputField.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = defaultName;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
    }
}
