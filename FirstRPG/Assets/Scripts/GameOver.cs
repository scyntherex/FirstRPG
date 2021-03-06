﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public string mainMenuScene, loadGameScene;

	// Use this for initialization
	void Start () {
        AudioManager.instance.PlayBGM(4);

        PlayerController.instance.gameObject.SetActive(false);
        GameMenu.instance.gameObject.SetActive(false);
        //battleBackgroundManager.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QuitToMain()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLastSave()
    {
        //battleBackgroundManager.instance.changePic(10);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        //Destroy(BattleBackgroundManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);

        SceneManager.LoadScene(loadGameScene);
    }
}
