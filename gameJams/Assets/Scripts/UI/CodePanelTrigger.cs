using UnityEngine;
using MyGame.Player;

public class CodePanelTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject codePanel; // Açýlacak kod paneli

    [Header("Player References")]
    [SerializeField] private Animator playerAnimator;

    private bool isPlayerNearby;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("CodePanelTrigger: PlayerMovement yok!");
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenCodePanel();
        }
    }

    private void OpenCodePanel()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(true);

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isIdle", true);
            }

            if (playerMovement != null)
            {
                playerMovement.SetCodePanelState(true);
            }

            Time.timeScale = 0f;
        }
    }

    public void CloseCodePanel()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(false);

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isIdle", false);
            }

            if (playerMovement != null)
            {
                playerMovement.SetCodePanelState(false);
            }

            Time.timeScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
