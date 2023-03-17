using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Stats : NetworkBehaviour
{
    NetworkVariable<int> playerHealth_net = new NetworkVariable<int>();
    NetworkVariable<int> currency_net = new NetworkVariable<int>();

    [SerializeField] private int playerHealth;
    [SerializeField] private int currency;

    [SerializeField] private TMP_Text playerHealthTxt;
    [SerializeField] private TMP_Text currencyTxt;

    #region Singleton
    public static Stats stats;

    private void Awake()
    {
        stats = this;
    }
    #endregion


    public override void OnNetworkSpawn()
    {
        playerHealth_net.Value = playerHealth;
        currency_net.Value = currency;

        playerHealth_net.OnValueChanged += changePlayerHealthTxt;
        currency_net.OnValueChanged += changeCurrencyTxt;

        base.OnNetworkSpawn();
    }

    private void Start()
    {
        playerHealthTxt.text = playerHealth.ToString();
        currencyTxt.text = currency.ToString();
    }


    private void changePlayerHealthTxt(int oldval, int newval)
    {
        playerHealthTxt.text = newval.ToString();
    }
    private void changeCurrencyTxt(int oldval, int newval)
    {
        currencyTxt.text = newval.ToString();
    }


    public void decreaseHealth(int damage)
    {
        if(IsServer) playerHealth_net.Value -= damage;
    }
    public void increaseCurrency()
    {
        if(IsServer) currency_net.Value += 10;
    }
    public void decreaseCurrency(int cost)
    {
        if(IsServer) currency_net.Value -= cost;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            decreaseHealth(1);
        }
    }
}
