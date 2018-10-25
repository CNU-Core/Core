using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeAttack : MonoBehaviour {
	private float done =4000.0F;
	public Text gui_text;
	public GameObject block;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(done > 0F){
			done -= 100*Time.deltaTime;
			gui_text.GetComponent<Text>().text = Math.Round((Math.Round(done,0)*0.01),0)+"sec";
		}else{
			block.SetActive(true);
			
		}
	}
}
