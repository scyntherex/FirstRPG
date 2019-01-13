using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour {

    public BattleType[] potentialBattles;

    private bool inZone;
    public float timeBetweenBattles = 6f;
    private float betweenBattleCounter;

    public bool activateOnEnter, activateOnStay, activateOnExit;

    public bool deactivateAfterStarting;

    public bool cannotFlee;

    public bool shouldCompleteQuest;
    public string questToComplete;

	// Use this for initialization
	void Start () {
        betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f,
            timeBetweenBattles * 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if(inZone && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || 
            Input.GetAxisRaw("Vertical") != 0)
            {
                betweenBattleCounter -= Time.deltaTime;
            }

            if(betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f,
                    timeBetweenBattles * 1.5f);
                StartCoroutine(StartBattleCo());
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inZone = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(activateOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inZone = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].
            rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].
            rewardXP;
        BattleManager.instance.rewardGold = potentialBattles[selectedBattle].
            rewardGold;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].
            enemies, cannotFlee);

        UIFade.instance.FadeFromBlack();

        if(deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleRewards.instance.markQuestComplete = shouldCompleteQuest;
        BattleRewards.instance.questToMark = questToComplete;

    }
}
