using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private CharacterShopDatabase characterDatabase;
    [SerializeField] private BackgroundShopDatabase backgroundShopDatabase;

    public void ResetPlayerData()
    {
        // 1. Oyuncunun parasýný sýfýrla
        GameDataManager.AddCoins(-GameDataManager.GetCoins());

        // 2. Varsayýlan karakteri seç (örneðin, ilk karakter)
        if (characterDatabase != null && characterDatabase.CharactersCount > 0)
        {
            Character defaultCharacter = characterDatabase.GetCharacter(0); // Ýlk karakter varsayýlan
            GameDataManager.SetSelectedCharacter(defaultCharacter, 0);
        }

        if(backgroundShopDatabase != null && backgroundShopDatabase.BackGroundsCount> 0) 
        {
            BackGround defaultBackground= backgroundShopDatabase.GetBackground(0);
            GameDataManager.SetSelectedBackground(defaultBackground, 0);
        }

        // 3. Satýn alýnan tüm karakterleri temizle
        GameDataManager.GetAllPurchasedCharacter().Clear();
        GameDataManager.GetAllPurchasedBackground().Clear();

        // 4. Deðiþiklikleri kaydet
        GameDataManager.SavePlayerData();
        GameDataManager.SaveCharactersShoprData();

        Debug.Log("Player data has been reset.");
    }
}