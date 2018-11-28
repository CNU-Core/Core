using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamesManager : MonoBehaviour {
	private static GamesManager Instance;

	private Transform HUD;

	private Transform currentBulletNumber;

	public int realTimeBulletNumber;

	public float Speed;         // 움직이는 스피드.
    public float AttackGap;     // 총알이 발사되는 간격.
	
    private bool ContinuouFire; // 게속 발사할 것인가? 에 대한 플래그.

	public GameObject camera_Obj;

	public int stage;

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;

			this.realTimeBulletNumber = 15;
			this.stage = 1;
			HUD = GameObject.Find("Canvas").transform.GetChild(4);
			currentBulletNumber = HUD.gameObject.transform.GetChild(5);

			currentBulletNumber.gameObject.GetComponent<Text>().text = realTimeBulletNumber.ToString();
			this.InitBullet();
		}   
   }

	
	public static GamesManager GetInstance(){
		return Instance;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void InitBullet(){
		AttackGap = 30.0f;
		ContinuouFire = true;
	}

	public void OnButtonFire(){
		if(realTimeBulletNumber <= 1){
			realTimeBulletNumber --;
			currentBulletNumber.gameObject.GetComponent<Text>().text = (realTimeBulletNumber).ToString();
			StartCoroutine("NextFire");
			HUD.GetChild(9).GetChild(0).gameObject.GetComponent<Button>().enabled = false;
			HUD.GetChild(7).gameObject.SetActive(true);
			HUD.GetChild(8).gameObject.SetActive(true);
			StartCoroutine("ReloadBar");
			Invoke("ReloadBullet", 3f);
		}else{
			realTimeBulletNumber --;
			currentBulletNumber.gameObject.GetComponent<Text>().text = (realTimeBulletNumber).ToString();
			StartCoroutine("NextFire");
		}
	}
	
	IEnumerator ReloadBar(){
		float count = 3.0f;
		while(count > 0){
			HUD.GetChild(8).gameObject.GetComponent<Image>().fillAmount = count / 3;
			count -= 0.1f;
            yield return new WaitForSeconds(0.1f);
		}
	}

	private void ReloadBullet(){
		HUD.GetChild(7).gameObject.SetActive(false);
		HUD.GetChild(8).gameObject.SetActive(false);
		HUD.GetChild(9).GetChild(0).gameObject.GetComponent<Button>().enabled = true;
		realTimeBulletNumber = 15;
		currentBulletNumber.gameObject.GetComponent<Text>().text = realTimeBulletNumber.ToString();
	}
	public void GameOver(){
		GameObject.Find("Controller").GetComponent<ARSurvive.ARController>().GameOver();
	}

	public void SaveScore(){
		NetworkManager.GetInstance().ReportScore(PlayerManager.GetInstance().player.player_Score);
	}
	
	// 총알정보 셋팅.
    void BulletInfoSetting(GameObject _Bullet)
    {
        if (_Bullet == null) return;
        Vector3 v = new Vector3(0,-2,0);
        // Vector3 screenBottom = new Vector3();
        // screenBottom = transform.position-v;
        
        Debug.Log(transform.position);
        _Bullet.transform.position = camera_Obj.transform.position;                // 총알의 위치 설정
        _Bullet.transform.rotation = camera_Obj.transform.rotation;                // 총알의 회전 설정.
        _Bullet.SetActive(true);                                        // 총알을 활성화 시킨다.
        _Bullet.GetComponent<Bullet>().StartCoroutine("MoveBullet");    // 총알을 움직이게 한다.
    }

	// 총알 키 체크.
    void KeyCheck()
    {
        if (Input.GetButtonDown("Jump")){ //스페이스바로 총알 발사
           	if(realTimeBulletNumber == 1){
            	realTimeBulletNumber -= 1;
            	currentBulletNumber.gameObject.GetComponent<Text>().text = (realTimeBulletNumber).ToString();
            	StartCoroutine("NextFire");
            	Invoke("reloadBullet", 3f);
         	}else{
            	realTimeBulletNumber -= 1;
            	currentBulletNumber.gameObject.GetComponent<Text>().text = (realTimeBulletNumber).ToString();
            	StartCoroutine("NextFire");
        	 }
      	}
	  	else if (Input.GetButtonUp("Jump")) 
            ContinuouFire = false;
 
        if (Input.GetKeyDown(KeyCode.Q))
            ObjManager.Call().MemoryDelete();
 
     //   if (Input.GetKeyDown(KeyCode.E))
     //       ObjManager.Call().CreateObject("Bullet", 20);
 
    }

	
	// 연속발사.
    IEnumerator NextFire()
    {
        // ContinuouFire = true;
        // while (ContinuouFire)
        // {
        //     // 총알을 리스트에서 가져온다.
		BulletInfoSetting(ObjManager.Call().GetObject("Bullet"));
		yield return new WaitForSeconds(AttackGap);
        // }
    }

	public void StageSetting(){
		ObjManager.Call().MemoryDelete();
		ObjManager.Call().GetComponent<EnemyArea>().RespawnArea.Clear();
		GameObject.Destroy(GameObject.Find("EnemyArea"));
		// for(int i = 0; i < ObjManager.Call().transform.childCount; i ++){
		// 	GameObject.Destroy(ObjManager.Call().transform.GetChild(i).gameObject);	
		// }
		GameObject.Destroy(ObjManager.Call().gameObject.GetComponent<WreckageManager>());
		
		ObjManager.Call().gameObject.AddComponent<WreckageManager>();
		ObjManager.Call().SetObject("Bullet");
		// ObjManager.Call().PlayerInfoUpdate(); //총알의 각각의 파워를 정의
	}

	public void RestartStage(){
		this.stage = 1;
		this.realTimeBulletNumber = 15;

		this.StageSetting();

		ObjManager.Call().CreateEnemyArea(10, 0, 1, 0);
	}
	public void NextStage(){
		this.stage ++;
		GameObject.Find("Controller").GetComponent<ARSurvive.ARController>().NextStage();
		this.StageSetting();
		if(this.stage == 2){
			ObjManager.Call().CreateEnemyArea(15, 0, 1, 5);
		}
		else if(this.stage == 3){
        	ObjManager.Call().CreateEnemyArea(15, 0, 1, 10);
      	}
	}

	public void ClearStage(){
		GameObject.Find("Controller").GetComponent<ARSurvive.ARController>().ClearStage();
	}
}
