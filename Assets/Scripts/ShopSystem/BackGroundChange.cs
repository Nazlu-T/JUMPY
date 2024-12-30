using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundChange : MonoBehaviour
{
    [SerializeField] GameObject[] BackGrounds;

    void Start()
    {
        ChangeBackground();
    }

    void ChangeBackground() 
    {
        BackGround backGround = GameDataManager.GetSelectedBackground();

        if (backGround.image != null)
        {
            // Get selected character's index:
            int selectedBackground = GameDataManager.GetSelectedBackgroundIndex();

            // show selected skin's gameobject:
            BackGrounds[selectedBackground].SetActive(true);

            // hide other skins (except selectedSkin) :
            for (int i = 0; i < BackGrounds.Length; i++)
                if (i != selectedBackground)
                    BackGrounds[i].SetActive(false);
        }
    }
    
}
