using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TMP_Text coinCountText;
    private int sessionCoins = 0;

    [SerializeField] private int coinValue = 10; // Value of each coin collected

    void Start()
    {
        UpdateCoinCountText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            sessionCoins += coinValue;


            // Update the coin count on the UI
            UpdateCoinCountText();


            // Destroy the collected coin
            Destroy(collision.gameObject);
        }
    }

    void UpdateCoinCountText()
    {
        if (coinCountText != null)
        {
            coinCountText.text = sessionCoins.ToString();
        }
    }

    public int EndGameAndResetSessionCoins()
    {
        int coinsCollected = sessionCoins;
        sessionCoins = 0;
        UpdateCoinCountText();
        return coinsCollected;
    }

}
