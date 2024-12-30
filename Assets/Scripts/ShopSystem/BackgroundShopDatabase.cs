using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BackGroundShopDatabase", menuName = "Shopping/BackGround shop database")]
public class BackgroundShopDatabase : ScriptableObject
{
    public BackGround[] backgrounds;

    public int BackGroundsCount
    {
        get { return backgrounds.Length; }
    }

    public BackGround GetBackground(int index)
    {
        return backgrounds[index];
    }

    public void PurchaseBackGround(int index)
    {
        backgrounds[index].isPurchased = true;
    }
}