using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    public static Economy instance;

    public float startingMoney;

    public static float money;

    //Should maybe be changed to a polled value
    //based on other game metrics like population, etc
    public static float earningRate;

    [SerializeField] private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        money = startingMoney;
    }

    //Called by the GameController
    public void EarnMoney()
    {
        money += earningRate *2;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + ((int)money).ToString();
    }
}
