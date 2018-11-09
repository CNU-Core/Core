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
				// 로그인 성공
                if (success) {
					Debug.Log("Login 성공");
                    StartCoroutine(Login());
                }
				// 로그인 실패
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
        Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
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
                FirebaseUser newUser = task.Result;
				GameObject controller = GameObject.Find("Controller");
                // Get the root reference location fo Database
                user_Name = newUser.DisplayName;
                user_ID = newUser.UserId;
                Debug.Log("USER NAME : " + user_Name + ", USER EMAIL : " + newUser.Email + ", USER ID : " + newUser.UserId);
                user_Ref.Child("users").Child(newUser.UserId).Child("name").SetValueAsync(user_Name);
				controller.GetComponent<ARSurvive.ARController>().StartMenu();
            }
        });
        Debug.Log("Login END. Check User");
	}

	// 점수를 Play Game에 저장
	public void ReportScore(int score){
		PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_score, (bool success) =>
		{
			if(success){
				Debug.Log("저장 성공");
			}
			else {
				Debug.Log("저장 실패");
			}
		});
	}

	// Play Game의 리더보드 점수 확인
	public void ShowLeaderboardUI(){
		// 로그인이 되어 있지 않았다면, 로그인 후 리더보드 UI표시 요청
		if(Social.localUser.authenticated == false){
			Social.localUser.Authenticate((bool success) =>
			{
				if(success){
					Debug.Log("리더보드 UI 가동");
					Social.ShowLeaderboardUI();
					return;
				}
				else {
					Debug.Log("리더보드 UI 실패");
					return;
				}
			});
		}
		else {
			PlayGamesPlatform.Instance.ShowLeaderboardUI();
		}
	}
}
