using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleBackgroundManager : MonoBehaviour {

    public SpriteRenderer bgRenderer;
    public Sprite[] battleBG;

    public static battleBackgroundManager instance;

	// Use this for initialization
	void Start () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void changePic(int picNumber)
    {
        bgRenderer.sprite = battleBG[picNumber];
    }
}
