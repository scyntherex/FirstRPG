using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour {

    public bool isPlayer;
    public string[] movesAvailable;

    public string charName, equippedWpn, equippedArmr;
    public int currentHP, maxHP, currentMP, maxMP, strength, defence,
        wpnPwr, armrPwr;
    public bool hasDied;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
