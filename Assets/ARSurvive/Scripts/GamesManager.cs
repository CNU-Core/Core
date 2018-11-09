using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GamesManager : MonoBehaviour {
	private static GamesManager Instance;

<<<<<<< HEAD
	void Start(){
		
	}	

	void Update(){
=======
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
	
	// Update is called once per frame
	void Update () {
>>>>>>> 0b934fa9d5aef09c5c5b8d5effd61dcc86745217
		
	}
}
