using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace BhorGames
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public RectTransform winLosePanel;
        public TMP_Text levelText;
        public TMP_Text seasonText;
        public Image[] images;

        void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public void ChangeSeasonTextAndImages(SeasonTheme season)
        {
            seasonText.text = season.ToString();

            foreach (Image image in images)
            {
                string path = "Sprites/" + season;
                image.sprite = Resources.Load<Sprite>(path);
            }
        }

        public void UpdateLevelTxt()
        {
            LevelManager.Instance.currentLevel = PlayerPrefs.GetInt("level", 0);
            levelText.text = "LEVEL " + (LevelManager.Instance.currentLevel + 1).ToString();
        }

        public void OpenLosePanel()
        {
            winLosePanel.GetChild(0).GetComponent<Image>().DOFade(0.4f, 0.2f);
            winLosePanel.GetChild(1).DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }

        public void OpenWinPanel()
        {
            winLosePanel.GetChild(0).GetComponent<Image>().DOFade(0.4f, 0.2f);
            winLosePanel.GetChild(2).DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }

        public void CloseWinLosePanel()
        {
            winLosePanel.GetChild(0).GetComponent<Image>().DOFade(0, 0.2f);
            winLosePanel.GetChild(1).DOScale(0, 0.3f).SetEase(Ease.Linear);
            winLosePanel.GetChild(2).DOScale(0, 0.3f).SetEase(Ease.Linear);
        }
    }
}