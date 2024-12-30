using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CharacterShopUI : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] float itemSpacing = .5f;
    [SerializeField] float startOffset = 100f; // Üst boþluðu azaltmak için baþlangýç pozisyonu (UI birimi)
    float itemHeight; 
    [SerializeField] Image selectedCharacterIcon;
    [SerializeField] Transform ShopMenu;
    [SerializeField] Transform ShopItemsContainer;
    [SerializeField] GameObject itemPrefab;
    [Space(20)]
    [SerializeField] CharacterShopDatabase characterDB;
    

    [Space(20)]
    [Header("Shop Events")]
    [SerializeField] GameObject shopUI;
    [SerializeField] Button openShopButton;
    [SerializeField] Button closeShopButton;
    [Space(20)]
    [Header("Main Menu")]
    [SerializeField] Image mainMenuCharacterImage;
 
    int newSelectedItemIndex = 0;
    int previousSelectedItemIndex = 0;

    void Start()
    {
        AddShopEvents();

        //Fill the shop's UI list with items
        GenerateShopItemsUI();

        // Set selected character in the playerDataManager .

        SetSelectedCharacter();

        //Select UI item
        SelectItemUI(GameDataManager.GetSelectedCharacterIndex());

        //update player skin (Main menu)
        ChangePlayerSkin();


    }

    void SetSelectedCharacter()
    {
        //Get saved index
        int index = GameDataManager.GetSelectedCharacterIndex();

        //Set selected character
        GameDataManager.SetSelectedCharacter(characterDB.GetCharacter(index), index);
    }

    void GenerateShopItemsUI()
    {
        //Loop save purchased items and make them as purchased in the Database array
        for (int i = 0; i < GameDataManager.GetAllPurchasedCharacter().Count; i++)
        {
            int purchasedCharacterIndex = GameDataManager.GetPurchasedCharacter(i);
            characterDB.PurchaseCharacter(purchasedCharacterIndex);
        }

        //Delete itemTemplate after calculating item's Height :
        itemHeight = ShopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(ShopItemsContainer.GetChild(0).gameObject);
        //DetachChildren () will make sure to delete it from the hierarchy, otherwise if you 
        //write ShopItemsContainer.ChildCount you w'll get "1"
        ShopItemsContainer.DetachChildren();

        

        //Generate Items
        for (int i = 0; i < characterDB.CharactersCount; i++)
        {
            //Create a Character and its corresponding UI element (uiItem)
            Character character = characterDB.GetCharacter(i);
            CharacterItemUI uiItem = Instantiate(itemPrefab, ShopItemsContainer).GetComponent<CharacterItemUI>();

            //Move item to its position
            //uiItem.SetItemPosition((itemHeight + itemSpacing) * i * Vector2.down);
            // Karakterin pozisyonunu hesaplayarak yukarý kaydýrma
            Vector2 position = (itemHeight + itemSpacing) * i * Vector2.down + Vector2.up * startOffset;
            uiItem.SetItemPosition(position);

            //Set Item name in Hierarchy (Not required)
            uiItem.gameObject.name = "Item" + i + "-" + character.name;

            //Add information to the UI (one item)
            uiItem.SetCharacterName(character.name);
           

            uiItem.SetCharacterImage(character.image);
            uiItem.SetCharacterPrice(character.price);

            if (character.isPurchased)
            {
                //Character is Purchased
                uiItem.SetCharacterAsPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                //Character is not Purchased yet
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }

            //Resize Items Container
            ShopItemsContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((itemHeight + itemSpacing) * characterDB.CharactersCount + itemSpacing);


        }

    }

    void ChangePlayerSkin()
    {
        Character character = GameDataManager.GetSelectedCharacter();
        if (character.image != null)
        {
            //Change Main menu's info (image & text)
            mainMenuCharacterImage.sprite = character.image;
            

            //Set selected Character Image at the top of shop menu
            selectedCharacterIcon.sprite = GameDataManager.GetSelectedCharacter().image;
        }
    }

    void OnItemSelected(int index)
    {
        // Select item in the UI
        SelectItemUI(index);

        //Save Data
        GameDataManager.SetSelectedCharacter(characterDB.GetCharacter(index), index);
        //Change Player Skin
        ChangePlayerSkin();


    }

    void SelectItemUI(int itemIndex)
    {
        previousSelectedItemIndex = newSelectedItemIndex;
        newSelectedItemIndex = itemIndex;

        CharacterItemUI prevUiItem = GetItemUI(previousSelectedItemIndex);
        CharacterItemUI newUiItem = GetItemUI(newSelectedItemIndex);

        prevUiItem.DeselectItem();
        newUiItem.SelectItem();

    }

    CharacterItemUI GetItemUI(int index)
    {
        return ShopItemsContainer.GetChild(index).GetComponent<CharacterItemUI>();
    }

    void OnItemPurchased(int index)
    {
        Character character = characterDB.GetCharacter(index);
        CharacterItemUI uiItem = GetItemUI(index);

        if (GameDataManager.CanSpendCoins(character.price))
        {
            //Proceed with the purchase operation
            GameDataManager.SpendCoins(character.price);

            
            //Update Coins UI text
            GameSharedUI.Instance.UpdateCoinsUIText();

            //Update DB's Data
            characterDB.PurchaseCharacter(index);

            uiItem.SetCharacterAsPurchased();
            uiItem.OnItemSelect(index, OnItemSelected);

            //Add purchased item to Shop Data
            GameDataManager.AddPurchasedCharacter(index);

        }
        
    }

    void AddShopEvents()
    {
        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(OpenShop);

        closeShopButton.onClick.RemoveAllListeners();
        closeShopButton.onClick.AddListener(CloseShop);

       
    }

    void OpenShop()
    {
        shopUI.SetActive(true);
    }

    void CloseShop()
    {
        shopUI.SetActive(false);
    }

    
}
