using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour {

    public static BattleRewards instance;

    public Text xpText, itemText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardsScreen(54, new string[] { "Iron Armor", "Wooden Sword" });
        }
    }

    public void OpenRewardsScreen(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;

        xpText.text = "Players earned " + xpEarned + " exp points.";
        itemText.text = "";

        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

    public void CloseRewardsScreen()
    {

    }
}
