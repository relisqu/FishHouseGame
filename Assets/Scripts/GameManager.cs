using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

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
            
        }

        public void Lose()
        {
            
        }
    }
}