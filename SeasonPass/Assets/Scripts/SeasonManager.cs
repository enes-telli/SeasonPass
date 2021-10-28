using System;
using UnityEngine;
using BhorGames;

public enum SeasonTheme
{
    WINTER, SUMMER, AUTUMN
}
 
public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;
    public static SeasonTheme currentSeason;
    public static GameObject seasonsGO;
    public static int lives = 2;

    public GameObject platform;
    public GameObject[] sidePlatforms;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        seasonsGO = GameObject.Find("Seasons");
        
        foreach (Transform child in seasonsGO.transform)
        {
            if (child.gameObject.activeSelf)
            {
                currentSeason = (SeasonTheme) Enum.Parse(typeof(SeasonTheme), child.name);
                UIManager.Instance.ChangeSeasonTextAndImages(currentSeason);
                ChangePlatformMaterials(currentSeason);
                break;
            }
        }
    }

    public static void LoseLives()
    {
        lives--;
        
        if (lives == 0)
        {
            GameManager.Instance.playerAnimator.SetTrigger("fail");
            FindObjectOfType<PlayerController>().enabled = false;
            UIManager.Instance.OpenLosePanel();
        }
    }

    public static void RestoreLives()
    {
        lives = 2;
    }

    public void ChangePlatformMaterials(SeasonTheme season)
    {
        platform.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + season);
        sidePlatforms[0].GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + season + "2");
        sidePlatforms[1].GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + season + "2");
    }
}
