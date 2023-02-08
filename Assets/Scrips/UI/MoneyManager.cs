using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    private void Awake() { instance = this; }

    public int money { get; private set; }
    public TextMeshProUGUI moneyText;
    public GameObject animatedCoinPrefab;

    public Transform coinTarget;

    void Start()
    {
        money = GameManager.instance.levelData.startingMoney;
        moneyText.text = money.ToString();
    }

    public bool AttemptAddMoney(int amount)
    {
        if (money + amount < 0)
            return false;

        money += amount;
        moneyText.text = money.ToString();

        return true;
    }

    public bool AttemptSubtractMoney(int amount)
    {
        return AttemptAddMoney(-amount);
    }

    public void SpawnAnimatedCoins(Vector2 pos, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(animatedCoinPrefab, pos, Quaternion.identity);
        }
    }

    public IEnumerator WaveOverCoinAnim(int numCoins, int coinValue)
    {
        for (int i = 0; i < numCoins; i++)
        {
            Instantiate(animatedCoinPrefab, new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)), Quaternion.identity).GetComponent<AnimatedCoin>().SetValue(coinValue);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
