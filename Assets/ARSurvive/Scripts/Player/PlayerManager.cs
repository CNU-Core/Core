using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	// PlayerManager Instance
	private static PlayerManager Instance;
	// Player 객체 클래스
	public PlayerInfomation player;

	// UI GameObject

	private GameObject HUD;
	private GameObject hp_Bar;
	private GameObject score;

	public GameObject camera;

	public float Speed;         // 움직이는 스피드.
    public float AttackGap;     // 총알이 발사되는 간격.
	
    private bool ContinuouFire; // 게속 발사할 것인가? 에 대한 플래그.
	
	void Awake(){
	}

	// Use this for initialization
	void Start () {
		if(Instance != null){
			GameObject.Destroy(gameObject);
		}
		else {
			GameObject.DontDestroyOnLoad(gameObject);
			Instance = this;
			// ObjManager.Call().SetObject("Bullet");
			
			HUD = GameObject.Find("HUD");
			score = HUD.transform.GetChild(1).gameObject;
			hp_Bar = HUD.transform.GetChild(3).gameObject;

			this.InitPlayerInformation();
			// ObjManager.Call().PlayerInfoUpdate(); //총알의 각각의 파워를 정의
		}

	}
	
	public static PlayerManager GetInstance(){
		return Instance;
	}

	// Update is called once per frame
	void Update () {
		KeyCheck();
	}

	// Player 정보 초기화
	void InitPlayerInformation(){
		player = new PlayerInfomation();
		player.MAX_HP = 100;
		player.player_HP = player.MAX_HP;
		player.player_Score = 0;
		player.player_Point = 0;
		player.BulletPower = 20;

		AttackGap = 0.2f;

		ContinuouFire = true;

		this.SetHPBar();
	}

	// Player의 HP 차감
	void PlayerAttacked(int damage){
		player.player_HP -= damage;
		this.SetHPBar();
	}

	// Player의 HP 회복
	void PlayerHPHeal(int heal){
		player.player_HP += heal;
		this.SetHPBar();
	}

	// HP Bar 현재 Player의 HP에 따라 길이 달라짐
	void SetHPBar(){
		hp_Bar.GetComponent<Image>().fillAmount = player.player_HP * 0.01f;
	}
	
	public void AddPlayerScore(int score){
		this.player.player_Score += score;

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
        ContinuouFire = true;
        while (ContinuouFire)
        {
            // 총알을 리스트에서 가져온다.
            BulletInfoSetting(ObjManager.Call().GetObject("Bullet"));
            yield return new WaitForSeconds(AttackGap);
        }
    }
 
    // 총알정보 셋팅.
    void BulletInfoSetting(GameObject _Bullet)
    {
        if (_Bullet == null) return;
        Vector3 v = new Vector3(0,-2,0);
        // Vector3 screenBottom = new Vector3();
        // screenBottom = transform.position-v;
        
        Debug.Log(transform.position);
        _Bullet.transform.position = camera.transform.position+v;                // 총알의 위치 설정
        _Bullet.transform.rotation = camera.transform.rotation;                // 총알의 회전 설정.
        _Bullet.SetActive(true);                                        // 총알을 활성화 시킨다.
        _Bullet.GetComponent<Bullet>().StartCoroutine("MoveBullet");    // 총알을 움직이게 한다.
    }

	
}
