using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	private int player_Score;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetPlayerScore(){
		return player_Score;
	}
	
	public void SetPlayerScore(int score){
		this.player_Score = score;
	}

	public void AddPlayerScore(int score){
		this.player_Score += score;
	}


}
