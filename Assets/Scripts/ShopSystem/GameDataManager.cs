using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Shop Data Holder
[System.Serializable]
public class CharactersShopData
{
    public List<int> purchasedCharactersIndexes = new();
    public List<int> purchasedBackgroundsIndexes = new(); // Yeni: Satýn alýnan arka planlarý tutar
}



//Player Data Holder
[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int selectedCharacterIndex = 0;
    public int selectedBackgroundIndex = 0;

}

public static class GameDataManager
{
    static PlayerData playerData = new();
    static CharactersShopData charactersShopData = new();
    private static List<int> purchasedCharacters = new List<int>();
    private static List<int> purchasedBackgrounds = new List<int>();

    static Character selectedCharacter;
    static BackGround selectedBackground;

    static GameDataManager()
    {
        LoadPlayerData();
        LoadCharactersShopData();
        Debug.Log($"Player Data Path: {Application.persistentDataPath}");

    }


    //Player Data Methods -----------------------------------------------------------------------------
    public static Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public static BackGround GetSelectedBackground()
    { 
        return selectedBackground;
    }



    public static void SetSelectedCharacter(Character character, int index)
    {
        selectedCharacter = character;
        playerData.selectedCharacterIndex = index;
        SavePlayerData();
    }

    public static void SetSelectedBackground(BackGround background, int index)
    {
        selectedBackground = background;
        playerData.selectedBackgroundIndex = index;
        SavePlayerData();
    }

    public static int GetSelectedCharacterIndex()
    {
        return playerData.selectedCharacterIndex;
    }
    public static int GetSelectedBackgroundIndex() 
    {
        return playerData.selectedBackgroundIndex;
    
    }
    public static int GetCoins()
    {
        return playerData.coins;
    }

    public static void AddCoins(int amount)
    {
        playerData.coins += amount;
        SavePlayerData();
    }

    public static bool CanSpendCoins(int amount)
    {
        return (playerData.coins >= amount);
    }

    public static void SpendCoins(int amount)
    {
        playerData.coins -= amount;
        SavePlayerData();
    }

    static void LoadPlayerData()
    {
        playerData = BinarySerializer.Load<PlayerData>("player-data.txt");
        UnityEngine.Debug.Log("<color=green>[PlayerData] Loaded.</color>");
    }

    public static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "player-data.txt");
        UnityEngine.Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
    }

    //Characters Shop Data Methods -----------------------------------------------------------------------------
    public static void AddPurchasedCharacter(int characterIndex)
    {
        charactersShopData.purchasedCharactersIndexes.Add(characterIndex);
        SaveCharactersShoprData();
    }

    public static void AddPurchasedBackground(int backgroundIndex) 
    { 
        charactersShopData.purchasedBackgroundsIndexes.Add(backgroundIndex);
        SaveCharactersShoprData();
    
    }

    public static List<int> GetAllPurchasedCharacter()
    {
        return charactersShopData.purchasedCharactersIndexes;
    }

    public static List<int> GetAllPurchasedBackground() 
    { 
        return charactersShopData.purchasedBackgroundsIndexes;
    
    }

    public static int GetPurchasedCharacter(int index)
    {
        return charactersShopData.purchasedCharactersIndexes[index];
    }

    public static int GetPurchasedBackground(int index)
    {
        return charactersShopData.purchasedBackgroundsIndexes[index];
    }

    static void LoadCharactersShopData()
    {
        charactersShopData = BinarySerializer.Load<CharactersShopData>("characters-shop-data.txt");
        UnityEngine.Debug.Log("<color=green>[CharactersShopData] Loaded.</color>");
    }

    public static void SaveCharactersShoprData()
    {
        BinarySerializer.Save(charactersShopData, "characters-shop-data.txt");
        UnityEngine.Debug.Log("<color=magenta>[CharactersShopData] Saved.</color>");
    }

}