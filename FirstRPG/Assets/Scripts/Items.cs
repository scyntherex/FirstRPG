using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmour;

    [Header("Item Appearance")]
    public string itemName;
    public string itemDescription;

    public int price;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Item Weapon/Armor Details")]
    public int weaponStrength;

    public int armorStrength; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use(int CharToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[CharToUseOn];

        if(isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;

                if (selectedChar.currentHP > selectedChar.maxHP)
                    selectedChar.currentHP = selectedChar.maxHP;
            }

            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if (selectedChar.currentMP > selectedChar.maxMP)
                    selectedChar.currentMP = selectedChar.maxMP;
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }

            GameMenu.instance.PlayUseSound1();
        }

        if(isWeapon)
        {
            if(selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }

            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;

            GameMenu.instance.PlayEquipSound();
        }

        if(isArmour)
        {
            if (selectedChar.equippedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            selectedChar.equippedArmr = itemName;
            selectedChar.armPwr = armorStrength;

            GameMenu.instance.PlayEquipSound();
        }

        GameManager.instance.RemoveItem(itemName);
    }
}
