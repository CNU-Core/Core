using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Enemy : MonoBehaviour
{
    public enum STATE { IDLE, WALK, ATTACK }
 
    // 공개
    public STATE state;          // 에너미의 상태.
    public float HP;             // 현재 체력
    public float Speed;          // 움직임 스피드.
    public float TraceDis;       // 추적 거리.
 
    // 비공개
    private Transform player;    // 플레이어.
    private Vector3 RandomPoint; // 랜덤한 위치. 
    //private Vector3 AreaPos;     // 부모 에리어의 처음 위치.
 
    private float MAX_HP;        // 최대 체력
    private bool Life;           // 살아있는지에 대한 여부.
    private bool trace;          // 추적 플래그.
 
    // 각 상태의 코루틴을 시작하기 위한 플래그.
    private bool idle;      
    private bool walk;
    private bool attack;
 
    private GameObject playerManager;

    // 정보 초기화 함수.
    public void Init()
    {
 
        MAX_HP = 100;
        HP = MAX_HP;
 
        Life   = true;
        trace  = false;
        state  = STATE.IDLE;
 
        idle   = false;
        walk   = false;
        attack = false;
 
        RandomPos();
        StartCoroutine("DisCheck");
        StartCoroutine("StateCheck");
    }
 
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("First Person Camera").transform;
        playerManager = GameObject.Find("PlayerManager");
    }
 
    // 플레이어와의 거리를 체크.
    IEnumerator DisCheck()
    {
        while (Life)
        {
            // 거리측정.
            float dis = Vector3.Distance(player.position, transform.position);
 
            // 범위 5안에 플레이어가 있으면.
            if (dis <= TraceDis)
                trace = true;   // 추적 플래그 on
            else
                trace = false;  // 추적 플래그 off
 
            // 0.2초마다 1번씩 거리를 체크한다.
            yield return new WaitForSeconds(0.2f);
        }
    }
 
    // 상태체크.
    IEnumerator StateCheck()
    {
        // 적 AI
        // 살아있는 동안에만 반복문을 돌림.
        while (Life)
        {
            switch (state)
            {
                case STATE.IDLE:
                {
                    if (!idle)
                    {
                        idle = true;
                        StartCoroutine("IdleState");
                    }
                    break;
                }
                case STATE.WALK:
                {
                    if (!walk)
                    {
                        walk = true;
                        StartCoroutine("WalkState");
                    }
                    break;
                }
                case STATE.ATTACK:
                {
                    if (!attack)
                    {
                        attack = true;
                        StartCoroutine("AttackState");
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
 
    // 정보 갱신.
    public void InfoUpdate(float _Damage)
    {
        HP -= _Damage;
 
        if (HP <= 0)
        {
            // 폭발한다.
            explosion();
 
            HP   = 0;       // 체력의 수치가 음의 값으로 갔을 경우를 대비한 초기화.
            Life = false;   // 죽었음을 알림.
            
            playerManager.GetComponent<PlayerManager>().AddPlayerScore(100);
            // 내 죽음을 부모에리어에게 알려라!
            // 부모 에리어가 가진 스크립트를 가져와 DeadEnemy()함수를 호출.
            transform.parent.GetComponent<CreateEnemy>().DeadEnemy();
            gameObject.SetActive(false);

            
            
        }
    }
 
    // 폭발은 예술이다.
    void explosion()
    {
        string Name = "NoData";
        // 잔해물 소환.
        for (int i = 0; i < 4; i++)
        {
            // 잔해물 4종류.
            switch (i)
            {
                case 0: Name = "Cube";      break;
                case 1: Name = "Cylinder";  break;
                case 2: Name = "Capsule";   break;
                case 3: Name = "Sphere";    break;
            }
 
            // 잔해물을 4종류를 5개씩 총 20개를 생성 후 뿌린다.
            for (int j = 0; j < 5; j++)
            {
                GameObject obj = ObjManager.Call().GetObject(Name);
                
                if (obj == null)
                    continue;
 
                obj.transform.position = transform.position;
                obj.SetActive(true);
                obj.GetComponent<ExplosionHide>().StartCoroutine("StartExplosionHide");
            }
        }
    }
 
    // 랜덤한 위치 선정.
    void RandomPos()
    {
        RandomPoint = new Vector3(Random.Range(-5, 5f), transform.position.y, Random.Range(-5, 5f));
    }
 
    // 목표(매개변수) 방향으로 회전.
    void Rotations(Vector3 _Pos)
    {
        Vector3 vec = (_Pos - transform.position).normalized;
        Quaternion q = Quaternion.LookRotation(vec);
        Vector3 eu = q.eulerAngles;
        eu.x = eu.z = 0;
 
        transform.rotation = Quaternion.Euler(eu);
    }
 
    // 거리체크 및 상태변화.
    void DisCheckToState(Vector3 _Pos, float _Dis, STATE _state, bool _UpDown)
    {
        float dis = Vector3.Distance(transform.position, _Pos);
 
        if (_UpDown)
        {
            if (dis <= _Dis)
                state = _state;
        }
        else
        {
            if (dis > _Dis)
                state = _state;
        }
    }
 
//------------- 각 상태에 따른 코루틴-----------------//
 
    // 평화상태.
    IEnumerator IdleState()
    {
        float timer = 0;
        float RandomTime = Random.Range(3, 7);
 
        while(Life)
        {
            timer += Time.deltaTime; // 시간을 축적.
 
            // 시간이 다 되면 다른상태로 전환하기 위해 반복문을 빠져나감.
            if (timer >= RandomTime || trace)
                break;
 
            yield return null;
        }
 
        RandomPos();           // 다음 걸어갈 위치를 정한다.
        state = STATE.WALK;        // 걷는 상태로 전환.
        idle = false;
    }
 
 
    // 걷는상태.
    IEnumerator WalkState()
    {
        while (Life)
        {
            if(state != STATE.WALK)
                break;
 
            if (trace) // 플레이어 추적.
            {
                Rotations(player.position); // 회전
                DisCheckToState(player.position, 2f, STATE.ATTACK, true); // 거리체크 및 상태변환.
            }
            else // 랜덤한 위치로 이동.
            {
                Rotations(RandomPoint);
                DisCheckToState(RandomPoint, 2f, STATE.IDLE, true); 
            }
            
            // 전진.
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            yield return null;
        }
 
        walk = false;
    }
 
    // 공격 상태.
    IEnumerator AttackState()
    {
        while (Life)
        {
            if(state != STATE.ATTACK)
                break;
 
            Rotations(player.position);
            DisCheckToState(player.position, 3f, STATE.IDLE, false);
 
            yield return null;
        }
 
        attack = false;
    }
}
