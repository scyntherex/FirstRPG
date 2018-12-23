﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int basEXP = 1000;


    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus;

    public int strength;
    public int defence;
    public int wpnPwr;
    public int armPwr;
    public string equippedWpn;
    public string equippedArmr;
    public Sprite charImage;

	// Use this for initialization
	void Start () {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = basEXP;

        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i-1] * 1.05f);
        }
    }
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(500);
        }*/
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;
        if (playerLevel < maxLevel)
        {
            if (currentEXP > expToNextLevel[playerLevel])
            {
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;

                //deteremining whether to add str or def based on odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }

                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP += mpLvlBonus[playerLevel];
                currentMP = maxMP;
            }
        }
        if (playerLevel >= maxLevel) {
            currentEXP = 0;
        }
    }
}
