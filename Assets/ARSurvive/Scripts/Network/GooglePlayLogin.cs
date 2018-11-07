using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;

public class GooglePlayLogin : MonoBehaviour {
	Firebase.Auth.FirebaseAuth auth;
	private static GooglePlayLogin Instance;

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
			Debug.Log("구글 플레이 로그인 Destroy 가동");
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
			
			InitGoogleGameService();
		}
	}
	
	public static GooglePlayLogin GetInstance(){
		return Instance;
	}
	// Update is called once per frame
	void Update () {
		
	}

	// 구글 플레이 게임 로그인 시도
	private void InitGoogleGameService(){
		// 구글 플레이 기능 설정 및 초기화
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
		.RequestServerAuthCode(false)
		.RequestEmail()
		.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();
	}

	public void GoogleGameLogin(){
		Social.localUser.Authenticate((bool success) => {
			if(success){
				string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

				auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
				Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
				auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
					if(task.IsCanceled){
						Debug.LogError("SignInWithCredentialAsync was canceled.");
						return;
					}
					if(task.IsFaulted){
						Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
						return;
					}

					Firebase.Auth.FirebaseUser newUser = task.Result;
					Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
					Login();
				});
			}
		});
	}

	private void Login(){
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		if(user != null){
			string playerName = user.DisplayName;
			string uid = user.UserId;
			Debug.Log("Player Name: " + playerName);
			Debug.Log("User ID: " + uid);
		}
	}
}
