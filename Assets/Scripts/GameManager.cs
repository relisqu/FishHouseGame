using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] HudUI hud;
        private void Start()
        {
            if (Instance == null)
                Instance = this;
        }

        public enum GameMode
        {
            Infinite, LevelBased
        }

        public void Win()
        {
            hud.Win();
        }

        public void Lose()
        {
            hud.Lose();
        }
    }
}