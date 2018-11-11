using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Player : MonoBehaviour {
 
    // 구조체 정보.
    public struct PINFO
    {
        public float MAX_HP;            // 체력의 최대치.
        public float HP;                // 현재 체력
        public float BulletPower;       // 총알의 힘.
        public float MoveSpeed;         // 움직임 스피드.
        public bool Life;               // 플레이어의 생명.
    }
 
    public float Speed;         // 움직이는 스피드.
    public float AttackGap;     // 총알이 발사되는 간격.
 
    private PINFO pInfo;        // 플레이어 정보.
    private Transform Vec;      // 카메라 벡터.
    private Vector3 MovePos;    // 플레이어 움직임에 대한 변수.
 
    private bool ContinuouFire; // 게속 발사할 것인가? 에 대한 플래그.
 
    void Init()
    {
        // 구조체 정보 초기화.
        pInfo.MAX_HP        = 100;
        pInfo.HP            = pInfo.MAX_HP;
        pInfo.BulletPower   = 20;
        pInfo.MoveSpeed     = 5;
        pInfo.Life          = true;
 
        //공개.
        AttackGap = 0.2f;
 
        // 비공개
        MovePos = Vector3.zero;
        ContinuouFire = true;
 
        // 플레이어 정보갱신.
        ObjManager.Call().PlayerInfoUpdate();
    }
 
    void Start()
    {
        // 총알 생성 요청.
        ObjManager.Call().SetObject("Bullet");
        Vec = GameObject.Find("CameraVector").transform;
 
        Init();
    }
 
    void Update () 
    {
        Run();
        KeyCheck();
    }
 
    // 플레이어 움직임.
    void Run()
    {
        int ButtonDown = 0;
        if (Input.GetKey(KeyCode.LeftArrow))    ButtonDown = 1;
        if (Input.GetKey(KeyCode.RightArrow))   ButtonDown = 1;
        if (Input.GetKey(KeyCode.UpArrow))      ButtonDown = 1;
        if (Input.GetKey(KeyCode.DownArrow))    ButtonDown = 1;
 
        // 플레이어가 움직임 버튼에서 손을 땠을 때 Horizontal, Vertical이 0으로 돌아감으로써
        // 플레이어의 회전상태가 다시 원상태로 돌아가지 않게 하기 위해서.
        if (ButtonDown != 0)
            Rotation();
        else
            return;
 
        transform.Translate(Vector3.forward * Time.deltaTime * pInfo.MoveSpeed * ButtonDown);
    }
 
    // 플레이어 회전.
    void Rotation()
    {
        MovePos.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));   // 벡터 셋팅.
        Quaternion q = Quaternion.LookRotation(Vec.TransformDirection(MovePos));  // 회전
 
        if (MovePos != Vector3.zero)
            transform.rotation = q;
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
        _Bullet.transform.position = transform.position+v;                // 총알의 위치 설정
        _Bullet.transform.rotation = transform.rotation;                // 총알의 회전 설정.
        _Bullet.SetActive(true);                                        // 총알을 활성화 시킨다.
        _Bullet.GetComponent<Bullet>().StartCoroutine("MoveBullet");    // 총알을 움직이게 한다.
    }
}