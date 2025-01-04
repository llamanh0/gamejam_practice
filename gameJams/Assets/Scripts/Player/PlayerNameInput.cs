using UnityEngine;
using TMPro;

/// <summary>
/// Oyuncu adýnýn girilmesini ve PlayerPrefs'e kaydedilmesini saðlar.
/// Boþ býrakýlýrsa varsayýlan bir isim atar.
/// </summary>
public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string defaultName = "Cabbar";

    public void SavePlayerName()
    {
        if (inputField == null)
        {
            Debug.LogError("PlayerNameInput: InputField referansý atanmamýþ!");
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
