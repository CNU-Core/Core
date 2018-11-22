using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	// PlayerManager Instance
	private static PlayerManager Instance;
	// Player 객체 클래스
	public PlayerInfomation player;

	// UI GameObject

	private Transform HUD;
	private Transform hp_Bar;
	private Transform score_View;

	private int sound_count=0;
	void Awake(){
	}

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;

			HUD = GameObject.Find("Canvas").transform.GetChild(4);
			score_View = HUD.GetChild(1);
			hp_Bar = HUD.GetChild(3);
			this.InitPlayerInformation();
		}

	}
	
	public static PlayerManager GetInstance(){
		return Instance;
	}
	// Update is called once per frame
	void Update () {

	}

	// Player 정보 초기화
	public void InitPlayerInformation(){
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
	public void PlayerAttacked(){
		player.player_HP -= player.BulletPower;
		this.SetHPBar();
		
		if(sound_count == 0){
			SoundManager.I.PlaySFX("attacked_from_Zombie");
			sound_count++;
		}else if(sound_count == 1){
			SoundManager.I.PlaySFX("scream1");
			sound_count++;
		}else{
			SoundManager.I.PlaySFX("scream2");
			sound_count = 0;
		}
		
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
		hp_Bar.gameObject.GetComponent<Image>().fillAmount = player.player_HP * 0.01f;
	}
	
	public void AddPlayerScore(int score){
		this.player.player_Score += score;
		this.AddPlayerPoint(score / 10);
		this.SetScoreView();
	}

	private void AddPlayerPoint(int point){
		this.player.player_Point += point;
	}

	public void SubPlayerPoint(int point){
		this.player.player_Point -= point;
	}

	void SetScoreView(){
		score_View.gameObject.GetComponent<Text>().text = this.player.player_Score.ToString();
	}
}
