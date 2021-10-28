using UnityEngine;

namespace BhorGames
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Animator playerAnimator;

        public bool isGameStarted;

        void Awake()
        {
            if (Instance == null) Instance = this;
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (!isGameStarted)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isGameStarted = true;
                    playerAnimator.GetComponent<Animator>().SetTrigger("walk");
                    
                }
            }
        }
    }
}