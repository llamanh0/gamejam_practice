using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Bir metni harf harf silip, ardýndan yeni bir metni harf harf yazma efektini uygular.
/// Tek seferlik veya butonla tekrar tetiklenebilir.
/// </summary>
public class TextEraseEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Button changeButton;

    [Header("Settings")]
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private string newText;
    [SerializeField] private bool allowSkip = true;

    private bool hasTextBeenChanged = false;
    private Coroutine currentCoroutine;

    private void Start()
    {
        if (changeButton != null)
        {
            changeButton.onClick.AddListener(StartReplace);
        }
    }

    private void StartReplace()
    {
        if (!hasTextBeenChanged)
        {
            hasTextBeenChanged = true;

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ReplaceTextCoroutine());
        }
        else if (allowSkip)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            if (targetText != null)
            {
                targetText.text = newText;
            }
        }
    }

    private IEnumerator ReplaceTextCoroutine()
    {
        if (targetText == null) yield break;

        string currentText = targetText.text;

        // 1. Aþama: Mevcut yazýyý harf harf sil
        for (int i = currentText.Length; i >= 0; i--)
        {
            targetText.text = currentText.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }

        // 2. Aþama: Yeni yazýyý harf harf yaz
        for (int i = 0; i <= newText.Length; i++)
        {
            targetText.text = newText.Substring(0, i);
            yield return new WaitForSeconds(speed);
        }
    }
}
