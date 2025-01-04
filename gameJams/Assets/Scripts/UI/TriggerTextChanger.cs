using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Oyuncu bir trigger'a girdiðinde, ekrandaki metni önce siler sonra yeni metni yazdýrýr.
/// Ardýndan kendi objesini yok eder.
/// </summary>
public class TriggerTextChanger : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private string newText = "Ýlerle!!!";
    [SerializeField] private float eraseSpeed = 0.1f;
    [SerializeField] private float typeSpeed = 0.1f;

    private bool isTriggered = false;

    private void Start()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Cabbar");
        newText = newText.Replace("{name}", playerName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(HandleTextChangeCoroutine());
        }
    }

    private IEnumerator HandleTextChangeCoroutine()
    {
        if (targetText == null) yield break;

        // 1. Mevcut metni harf harf sil
        for (int i = targetText.text.Length; i >= 0; i--)
        {
            targetText.text = targetText.text.Substring(0, i);
            yield return new WaitForSeconds(eraseSpeed);
        }

        // 2. Yeni metni harf harf yaz
        for (int i = 0; i <= newText.Length; i++)
        {
            targetText.text = newText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }

        Destroy(gameObject);
    }
}
