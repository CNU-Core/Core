using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;
using System.Globalization;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour {
	private static NetworkManager Instance;
	private string user_Name;
	private string user_ID;
    DatabaseReference user_Ref;
	FirebaseAuth auth;

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
			Debug.Log("구글 플레이 로그인 Destroy 가동");
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ar-zombie-survive-93013241.firebaseio.com/");
			user_Ref = FirebaseDatabase.DefaultInstance.RootReference;
			InitGoogleGameService();
		}
	}
	
	public static NetworkManager GetInstance(){
		return Instance;
	}
	// Update is called once per frame
	void Update () {
		
	}

	// 구글 플레이 게임 로그인 시도
	void InitGoogleGameService(){
		// 구글 플레이 기능 설정 및 초기화
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
		.RequestServerAuthCode(false)
		.RequestEmail()
        .RequestIdToken()
		.Build();

		PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}

	// 구글 로그인 버튼을 누를 시 Google Game Login API에 로그인 처리하여 정보를 얻는 함수
	public void OnClickGoogleGameLogin(){
		Debug.Log("On Click Google Game Login 실행");

		if(!PlayGamesPlatform.Instance.IsAuthenticated()){
            Social.localUser.Authenticate(success =>
            {
                if (success) {
					Debug.Log("Login 시도중..");
                    StartCoroutine(Login());
                }
                else {
					Debug.Log("On Click 구글 로그인에 실패하였습니다.");
                }
            });
        }
        else {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Debug.Log("다른 버전의 소셜 로컬유저 아이디: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
            Debug.Log("소셜 로컬유저 아이디: " + Social.localUser.id);
		}
	}

	IEnumerator Login(){
		while (System.String.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;

		string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(
        task =>
        {
            if (task.IsFaulted){
				Debug.Log("구글 로그인에 실패하였습니다.");
            }
            else if(task.IsCanceled){
				Debug.Log("구글 로그인이 취소되었습니다.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Task NewUser OK");
                // User is now signed in.
                Firebase.Auth.FirebaseUser newUser = task.Result;

                // Get the root reference location fo Database
                user_Name = newUser.DisplayName;
                user_ID = newUser.UserId;
                Debug.Log("USER NAME : " + user_Name + ", USER EMAIL : " + newUser.Email + ", USER ID : " + newUser.UserId);
                user_Ref.Child("users").Child(newUser.UserId).Child("name").SetValueAsync(user_Name);
                // ScenesManager.getInstance().SceneChange("Main");
            }
        });
        Debug.Log("Login END. Check User");
		// Debug.Log("Login 실행");
		// Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		// if(user != null){
		// 	Debug.Log("유저 정보 있음");
        //     Transform controller = GameObject.Find("Controller").transform;
		// 	string playerName = user.DisplayName;
		// 	string uid = user.UserId;
		// 	Debug.Log("Player Name: " + playerName);
		// 	Debug.Log("User ID: " + uid);

		// 	controller.gameObject.GetComponent<ARSurvive.ARController>().StartMenu();
		// }
		// else {
		// 	Debug.Log("유저 정보 없음");
		// }
	}
}
