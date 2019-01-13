using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {

    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions, enemyPositions;

    public BattleChar[] playerPrefabs, enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;

    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;

    public Text[] playerName, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 35;
    private bool fleeing;

    public GameObject itemsMenu;
    public ItemButton[] itemBattleButtons;
    public Items activeItem;
    public Text itemName, itemDesc, useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public string gameOverScene;
    public int rewardXP, rewardGold;
    public string[] rewardItems;

    public GameObject enemyHealthBars;
    public Slider[] healthBars;

    public bool cannotFlee;

    // Use this for initialization
    void Start () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] {"Hatchling"}, false);
        }

        
        if (battleActive)
        {
            if (turnWaiting)
            {
                if(activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                }
                else
                {
                    uiButtonsHolder.SetActive(false);

                    //enemy should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if(!battleActive)
        {
            cannotFlee = setCannotFlee;

            battleActive = true;
            GameManager.instance.battleActive = true;

            transform.position = new Vector3(Camera.main.transform.position.x,
                Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            for(int i = 0; i < playerPositions.Length; i++)
            {
                if(GameManager.instance.playerStats[i].
                    gameObject.activeInHierarchy)
                {
                    for(int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if(playerPrefabs[j].charName == GameManager.instance.
                            playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j],
                                playerPositions[i].position, 
                                playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = 
                                GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].equippedWpn = thePlayer.equippedWpn;
                            activeBattlers[i].wpnPwr = thePlayer.wpnPwr;
                            activeBattlers[i].equippedArmr = thePlayer.equippedArmr;
                            activeBattlers[i].armrPwr = thePlayer.armPwr;
                        }
                    }


                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j],
                                enemyPositions[i].position, 
                                enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
            if (!cannotFlee)
            {
                AudioManager.instance.PlayBGM(0);
            }
            else
            {
                AudioManager.instance.PlayBGM(1);
            }
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
            UpdateUIStats();
        }
    }

    public void NexTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
            currentTurn = 0;

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
                activeBattlers[i].currentHP = 0;

            if(activeBattlers[i].currentHP == 0)
            {
                //Handle Dead
                if(activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite =
                        activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite =
                        activeBattlers[i].aliveSprite;
                } else {
                    allEnemiesDead = false;
                }
            }
        }

        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                //end to Victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end in failure
                StartCoroutine(GameOverCo());
            }

            /*battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;*/
        }
        else
        {
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                    currentTurn = 0;
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NexTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];

        //activeBattlers[selectedTarget].currentHP -= 10;

        int selectAttack = Random.Range(0,
            activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for(int i = 0; i < movesList.Length; i++)
        {
            if(movesList[i].moveName == 
                activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect,
                    activeBattlers[selectedTarget].transform.position,
                    activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePwr;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.
            position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength +
            activeBattlers[currentTurn].wpnPwr;
        float defPwr = activeBattlers[target].defence +
            activeBattlers[target].armrPwr;

        float damageCalc = (atkPwr / defPwr) * movePower *
            Random.Range(.9f, 1.1f);
        int dmgToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " +
            damageCalc + "(" + dmgToGive + ") damage to " +
            activeBattlers[target].charName);

        activeBattlers[target].currentHP -= dmgToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position,
            activeBattlers[target].transform.rotation).SetDamage(dmgToGive);
        UpdateUIStats();
        DisplayHealth();
    }

    public void UpdateUIStats()
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if(activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, 
                        int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0,
                        int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
        DisplayHealth();
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    { 
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect,
                    activeBattlers[selectedTarget].transform.position,
                    activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePwr;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.
            position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        CloseTargetSelection();

        NexTurn();

    }

    public void openTargetSelection(string moveName)
    {
        AudioManager.instance.PlaySFX(4);
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for(int i = 0; i < targetButtons.Length; i++)
        {
            if(Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattleTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]]
                    .charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void CloseTargetSelection()
    {
        targetMenu.SetActive(false);
    }

    public void OpenMagicSelection()
    {
        AudioManager.instance.PlaySFX(4);
        magicMenu.SetActive(true);

        for(int i = 0; i < magicButtons.Length; i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].
                    movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for(int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].
                            spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if(cannotFlee)
        {
            battleNotice.theText.text = "Can't flee Boss Battles!";
            battleNotice.Activate();
        }
        else
        {
            AudioManager.instance.PlaySFX(4);
            int fleeSucess = Random.Range(0, 100);
            if (fleeSucess < chanceToFlee)
            {
                //end battle
                //battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                NexTurn();
                battleNotice.theText.text = "Can't Escape!";
                battleNotice.Activate();
            }
        }
    }

    public void OpenItemSelection()
    {
        AudioManager.instance.PlaySFX(4);
        GameManager.instance.SortItems();
        itemsMenu.SetActive(true);
        CloseHealthBars();
        for(int i = 0; i < itemBattleButtons.Length; i++)
        {
            itemBattleButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemBattleButtons[i].buttonImage.gameObject.SetActive(true);
                itemBattleButtons[i].buttonImage.sprite =
                    GameManager.instance.GetItemDetails(GameManager.
                    instance.itemsHeld[i]).itemSprite;
                itemBattleButtons[i].amountText.text = GameManager.instance.
                    numberOfItems[i].ToString();
            }
            else
            {
                itemBattleButtons[i].buttonImage.gameObject.SetActive(false);
                itemBattleButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBattleItem(Items newItem)
    {
        activeItem = newItem;
        AudioManager.instance.PlaySFX(4);
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDesc.text = activeItem.itemDescription;
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);
        AudioManager.instance.PlaySFX(4);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {
            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].
                charName;
            itemCharChoiceNames[i].transform.parent.gameObject.
                SetActive(GameManager.instance.playerStats[i].gameObject.
                activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
        DisplayHealth();
    }

    public void UseItem(int selectChar)
    {
        AudioManager.instance.PlaySFX(4);
        activeItem.UseInBattle(selectChar);
        CloseItemCharChoice();
        GameManager.instance.SortItems();
        UpdateUIStats();
        CloseItemMenu();
        DisplayHealth();
        NexTurn();
    }

    public void CloseItemMenu()
    {
        itemsMenu.SetActive(false);
    }

    public void CloseHealthBars()
    {
        enemyHealthBars.SetActive(false);
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        CloseItemCharChoice();
        CloseItemMenu();
        CloseHealthBars();

        yield return new WaitForSeconds(0.5f);
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.
                        playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP =
                            activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP =
                            activeBattlers[i].currentMP;
                        GameManager.instance.playerStats[j].strength =
                            activeBattlers[i].strength;
                        GameManager.instance.playerStats[j].defence =
                            activeBattlers[i].defence;
                        GameManager.instance.playerStats[j].equippedWpn =
                            activeBattlers[i].equippedWpn;
                        GameManager.instance.playerStats[j].equippedArmr =
                            activeBattlers[i].equippedArmr;
                        GameManager.instance.playerStats[j].wpnPwr =
                            activeBattlers[i].wpnPwr;
                        GameManager.instance.playerStats[j].armPwr =
                            activeBattlers[i].armrPwr;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if(fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleRewards.instance.OpenRewardsScreen
                (rewardXP, rewardGold, rewardItems);
        }


       //AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().
         //   musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }

    public void DisplayHealth()
    {
        enemyHealthBars.SetActive(true);
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }
        for (int i = 0; i < Enemies.Count; i++)
        {
            healthBars[i].value = activeBattlers[Enemies[i]].currentHP;
            healthBars[i].maxValue = activeBattlers[Enemies[i]].maxHP;
        }
    }
}
