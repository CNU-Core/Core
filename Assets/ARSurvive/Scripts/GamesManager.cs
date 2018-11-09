using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GamesManager : MonoBehaviour {
	private static GamesManager Instance;

	public float Speed;         // 움직이는 스피드.
    public float AttackGap;     // 총알이 발사되는 간격.
	
    private bool ContinuouFire; // 게속 발사할 것인가? 에 대한 플래그.

	public GameObject camera_Obj;

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
			
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
		AttackGap = 0.2f;
		ContinuouFire = true;
	}

	public void OnButtonFire(){
		StartCoroutine("NextFire");
	}
	
	public void GameOver(){
		ObjManager.Call().MemoryDelete();
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
        if (Input.GetButtonDown("Jump")) //스페이스바로 총알 발사
            StartCoroutine("NextFire");
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
}
