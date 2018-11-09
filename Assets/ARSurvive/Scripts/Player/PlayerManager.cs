﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	// PlayerManager Instance
	private static PlayerManager Instance;
	// Player 객체 클래스
	public PlayerInfomation player;

	// UI GameObject

	private GameObject HUD;
	private GameObject hp_Bar;
	private GameObject score_View;

	
	void Awake(){
		HUD = GameObject.Find("HUD");
		score_View = HUD.transform.GetChild(1).gameObject;
		hp_Bar = HUD.transform.GetChild(3).gameObject;
	}

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
			ObjManager.Call().SetObject("Bullet");
			this.InitPlayerInformation();
			ObjManager.Call().PlayerInfoUpdate(); //총알의 각각의 파워를 정의
		}

	}
	
	public static PlayerManager GetInstance(){
		return Instance;
	}
	// Update is called once per frame
	void Update () {

	}

	// Player 정보 초기화
	void InitPlayerInformation(){
		player = new PlayerInfomation();
		player.MAX_HP = 100;
		player.player_HP = player.MAX_HP;
		player.player_Score = 0;
		player.player_Point = 0;
		player.BulletPower = 20;

		this.SetHPBar();
		this.SetScoreView();
	}

	// Player의 HP 차감
	public void PlayerAttacked(int damage){
		player.player_HP -= damage;
		this.SetHPBar();
		if(player.player_HP <= 0){
			GamesManager.GetInstance().GameOver();
		}
	}

	// Player의 HP 회복
	public void PlayerHPHeal(int heal){
		player.player_HP += heal;
		this.SetHPBar();
	}

	// HP Bar 현재 Player의 HP에 따라 길이 달라짐
	public void SetHPBar(){
		hp_Bar.GetComponent<Image>().fillAmount = player.player_HP * 0.01f;
	}
	
	public void AddPlayerScore(int score){
		this.player.player_Score += score;
		this.SetScoreView();
	}

	void SetScoreView(){
		score_View.GetComponent<Text>().text = this.player.player_Score.ToString();
	}
}
