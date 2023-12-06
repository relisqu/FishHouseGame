using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] HudUI hud;
        [SerializeField] float LevelTime;
        [SerializeField] GameMode gameMode;
        public int RecipeCompleted;
        public int RecipeFailed;
        public int HP;
        private bool _isOver;

        float _timeLeft;
        private void Start()
        {
            if (Instance == null)
                Instance = this;
            _timeLeft = LevelTime;
        }

        public enum GameMode
        {
            Infinite, LevelBased
        }

        private void Update()
        {
            _timeLeft = Mathf.Clamp(_timeLeft -= Time.deltaTime, 0, float.MaxValue);
            if (gameMode == GameMode.LevelBased)
                hud.SetTimeSlider(1 - _timeLeft / LevelTime);
            //hud.SetTime(_timeLeft);

            if (_timeLeft <= 0 && gameMode == GameMode.LevelBased)
                Win();
            
        }

        public void Win()
        {
            if (_isOver)
                return;

            _isOver = true;
            hud.Win();
            int ml = PlayerPrefs.GetInt("MaxLevel");
            PlayerPrefs.SetInt("MaxLevel", Mathf.Max(ml, UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1));
        }

        public void Lose()
        {
            hud.Lose();
        }
    }
}