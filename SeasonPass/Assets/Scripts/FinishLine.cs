using UnityEngine;
using BhorGames;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.levelText.gameObject.SetActive(false);
            GameManager.Instance.playerAnimator.SetTrigger("win");
            FindObjectOfType<PlayerController>().enabled = false;
            UIManager.Instance.OpenWinPanel();
        }
    }
}
