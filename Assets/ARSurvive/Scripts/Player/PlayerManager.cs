using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	// PlayerManager Instance
	private static PlayerManager Instance;
	// Player 객체 클래스
	private PlayerInfomation player;

	// UI GameObject
	private GameObject HUD;
	private GameObject hp_Bar;
	private GameObject score;

	void Awake(){
		HUD = GameObject.Find("HUD");
		score = HUD.transform.GetChild(1).gameObject;
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
			this.InitPlayerInformation();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Player 정보 초기화
	void InitPlayerInformation(){
		player = new PlayerInfomation();

		player.player_HP = 100;
		player.player_Score = 0;
		player.player_Point = 0;

		this.SetHPBar();
	}

	// Player의 HP 차감
	void PlayerAttacked(int damage){
		player.player_HP -= damage;
		this.SetHPBar();
	}

	// Player의 HP 회복
	void PlayerHPHeal(int heal){
		player.player_HP += heal;
		this.SetHPBar();
	}

	// HP Bar 현재 Player의 HP에 따라 길이 달라짐
	void SetHPBar(){
		hp_Bar.GetComponent<Image>().fillAmount = player.player_HP * 0.01f;
	}
}
