using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class BackGroundShopUI : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] float itemSpacing = 0.5f;
    [SerializeField] float startOffset = 100f; // UI birimi
    float itemHeight;
    
    [SerializeField] Transform shopMenu;
    [SerializeField] Transform shopItemsContainer;
    [SerializeField] GameObject itemPrefab;
    [Space(20)]
    [SerializeField] BackgroundShopDatabase backgroundDB;

    [Space(20)]
    [Header("Shop Events")]
    [SerializeField] GameObject shopUI;
    [SerializeField] Button openShopButton;
    [SerializeField] Button closeShopButton;

    int newSelectedItemIndex = 0;
    int previousSelectedItemIndex = 0;

    void Start()
    {
        AddShopEvents();
        GenerateShopItemsUI();
        SetSelectedBackground();
        SelectItemUI(GameDataManager.GetSelectedBackgroundIndex());
        
    }

    void SetSelectedBackground()
    {
        int index = GameDataManager.GetSelectedBackgroundIndex();
        GameDataManager.SetSelectedBackground(backgroundDB.GetBackground(index), index);
    }

    void GenerateShopItemsUI()
    {
        for (int i = 0; i < GameDataManager.GetAllPurchasedBackground().Count; i++)
        {
            int purchasedBackgroundIndex = GameDataManager.GetPurchasedBackground(i);
            backgroundDB.PurchaseBackGround(purchasedBackgroundIndex);
        }

        itemHeight = shopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(shopItemsContainer.GetChild(0).gameObject);
        shopItemsContainer.DetachChildren();

        for (int i = 0; i < backgroundDB.BackGroundsCount; i++)
        {
            BackGround background = backgroundDB.GetBackground(i);
            BackgroundItemUI uiItem = Instantiate(itemPrefab, shopItemsContainer).GetComponent<BackgroundItemUI>();

            //Move item to its position
            //uiItem.SetItemPosition((itemHeight + itemSpacing) * i * Vector2.down);
            // Karakterin pozisyonunu hesaplayarak yukarý kaydýrma
            Vector2 position = (itemHeight + itemSpacing) * i * Vector2.down + Vector2.up * startOffset;
            uiItem.SetItemPosition(position);

            uiItem.gameObject.name = "Item" + i + "-" + background.name;
           
            uiItem.SetBackGroundName(background.name);
           
            uiItem.SetBackGroundImage(background.image);
           
            uiItem.SetBackGroundPrice(background.price);

            if (background.isPurchased)
            {
                uiItem.SetBackGroundAsPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }

            shopItemsContainer.GetComponent<RectTransform>().sizeDelta =
                Vector2.up * ((itemHeight + itemSpacing) * backgroundDB.BackGroundsCount + itemSpacing);
        }
    }

   

    void OnItemSelected(int index)
    {
        SelectItemUI(index);
        GameDataManager.SetSelectedBackground(backgroundDB.GetBackground(index), index);
        
    }

    void SelectItemUI(int itemIndex)
    {
        previousSelectedItemIndex = newSelectedItemIndex;
        newSelectedItemIndex = itemIndex;

        BackgroundItemUI prevUiItem = GetItemUI(previousSelectedItemIndex);
        BackgroundItemUI newUiItem = GetItemUI(newSelectedItemIndex);

        prevUiItem.DeselectItem();
        newUiItem.SelectItem();
    }

    BackgroundItemUI GetItemUI(int index)
    {
        return shopItemsContainer.GetChild(index).GetComponent<BackgroundItemUI>();
    }

    void OnItemPurchased(int index)
    {
        BackGround background = backgroundDB.GetBackground(index);
        BackgroundItemUI uiItem = GetItemUI(index);

        if (GameDataManager.CanSpendCoins(background.price))
        {
            GameDataManager.SpendCoins(background.price);
            GameSharedUI.Instance.UpdateCoinsUIText();

            backgroundDB.PurchaseBackGround(index);
            uiItem.SetBackGroundAsPurchased();
            uiItem.OnItemSelect(index, OnItemSelected);
            GameDataManager.AddPurchasedBackground(index);
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
