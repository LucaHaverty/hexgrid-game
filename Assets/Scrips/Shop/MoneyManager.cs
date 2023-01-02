using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    private void Awake() { instance = this; }

    public int money { get; private set; }
    public TextMeshProUGUI moneyText;

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
}
