using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;

public class GooglePlayLogin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitGoogleService();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitGoogleService(){
		// 구글 플레이 기능 설정 및 초기화
		PlayGamesPlatform.Activate();
	}
}
