using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Scrips
{
    public class PlayerHealthManager : MonoBehaviour
    {
        public static PlayerHealthManager instance;
        private void Awake() { instance = this; }

        public int health;
        public TextMeshProUGUI healthText;
        
        void Start()
        {
            health = GameManager.instance.levelData.startingHealth;
            healthText.text = health.ToString();
        }

        public void AddHealth(int amount)
        {
            if (health < 0)
                return;
            
            if (health + amount <= 0 && GameManager.state != GameState.PlayerLost)
                GameManager.SetGameState(GameState.PlayerLost);
        
            health += amount;
            healthText.text = Mathf.Max(health,0).ToString();
            
        }

        public void SubtractHealth(int amount) { AddHealth(-amount); }
    }
}
