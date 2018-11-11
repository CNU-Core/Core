using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamesManager : MonoBehaviour {
	private static GamesManager Instance;
	private PlayerInfomation player;
	public Text Current_point;
	
	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
		}	
	}
	void Awake(){
	}	

	void Update(){
		//Current_point.GetComponent<Text>().text = player.player_Point.ToString();
	}
}
