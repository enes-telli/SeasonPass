using UnityEngine;
using UnityEngine.SceneManagement;

namespace BhorGames
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public int currentLevel;

        void Awake()
        {
            if (Instance == null) Instance = this;
        }

        void Start()
        {
            UIManager.Instance.UpdateLevelTxt();
        }

        public void NextLevel()
        {
            currentLevel += 1;
            PlayerPrefs.SetInt("level", currentLevel);
            SceneManager.LoadScene((currentLevel % (SceneManager.sceneCountInBuildSettings - 1)) + 1);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}