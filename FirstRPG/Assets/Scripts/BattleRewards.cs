using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour {

    public static BattleRewards instance;

    public Text xpText, itemText, goldText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned, goldEarned;

    public bool markQuestComplete;
    public string questToMark;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void OpenRewardsScreen(int xp, int gold, string[] rewards)
    {
        xpEarned = xp;
        goldEarned = gold;
        rewardItems = rewards;

        xpText.text = "Players earned " + xpEarned + " exp points.";
        goldText.text = "Gold earned " + goldEarned + " g.";
        itemText.text = "";


        for(int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
        AudioManager.instance.PlayBGM(6);
    }

    public void CloseRewardsScreen()
    {
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if(GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);

            }
        }

        GameManager.instance.currentGold += goldEarned;

        for(int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }

        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().
           musicToPlay);

        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
