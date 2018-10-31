using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GamesManager : MonoBehaviour {

	// Use this for initialization
	public static GamesManager instance = null;

	public Text Current_point;
	public int point = 1000;
	public static GamesManager Instance{
		get{
			return instance;
		}
	}
	void Awake(){
		if(instance!=null){
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}	

	void Update(){
		Current_point.GetComponent<Text>().text = point.ToString();
	}
}
