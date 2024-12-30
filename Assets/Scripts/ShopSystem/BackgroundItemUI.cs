using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class BackgroundItemUI : MonoBehaviour
{
    [SerializeField] Color itemNotSelectedColor;
    [SerializeField] Color itemSelectedColor;

    [Space(20f)]
    [SerializeField] Image backgroundImage;
    [SerializeField] TMP_Text backgroundName;
    [SerializeField] TMP_Text backgroundPrice;
    [SerializeField] Button backgroundPurchaseButton;

    [Space(20F)]
    [SerializeField] Button itemButton;
    [SerializeField] Image itemImage;
    [SerializeField] Outline itemOutline;
    //--------------------------------------------------------------
    public void SetItemPosition(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }


    public void SetBackGroundImage(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public void SetBackGroundName(string name)
    {
        backgroundName.text = name;
    }

    public void SetBackGroundPrice(int price)
    {
        backgroundPrice.text = price.ToString();
    }

    public void SetBackGroundAsPurchased()
    {
        backgroundPurchaseButton.gameObject.SetActive(false);
        itemButton.interactable = true;

        itemImage.color = itemNotSelectedColor;
    }

    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        backgroundPurchaseButton.onClick.RemoveAllListeners();
        backgroundPurchaseButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        itemButton.interactable = true;

        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void SelectItem()
    {
        itemOutline.enabled = true;
        itemImage.color = itemSelectedColor;
        itemButton.interactable = false;
    }

    public void DeselectItem()
    {
        itemOutline.enabled = false;
        itemImage.color = itemNotSelectedColor;
        itemButton.interactable = true;
    }

}
