using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CreateEnemy : MonoBehaviour {
 
    public float SummonTimeGap;   // 소환간격.
    public int MaxCount;          // 최대 소환 가능한 수.
    public int EnemyCount;        // 현재 소환된 머리 수.
    
    public bool Summon;           // 소환 여부.
    public GameObject EnemyPref;
    // 값 초기화.
    void Init(float _SummonTimeGap, int _MaxCount, bool _Summon)
    {
        SummonTimeGap = _SummonTimeGap;
        MaxCount = _MaxCount;
        Summon = _Summon;
 
        StartCoroutine("SummonEnemy");
    }
 
    void Start () 
    {
        Init(Random.Range(5f,10f),1,false);
    }
    
    // 랜덤 위치.
    Vector3 RandomPos()
    {
        Vector3 Pos = new Vector3(Random.Range(0f, 5f), 1, Random.Range(0f, 5f));
 
        // 현재 에리어의 위치 + 랜덤한 위치
        return transform.position + Pos;
    }
 
    // 소환정보 체크 후 소환 결정.
    IEnumerator SummonEnemy()
    {
        while (true)
        {
            // 적 머리수가 0이면 소환여부를 false(소환가능)로 전환.
            if (EnemyCount <= 0)
            {
                // Enemy가 다 죽고 리스폰까지 시간지연.
                yield return new WaitForSeconds(SummonTimeGap);
                Summon = false;
            }
 
            // 소환된 적이 없으면 적 소환.
            if (!Summon)
                ESummon();
 
            // 1초마다 체크.
            yield return new WaitForSeconds(1f);
        }
    }
 
    // 소환!
    void ESummon()
    {
        for (int i = 0; i < MaxCount; i++)
        {
            GameObject obj = ObjManager.Call().GetObject("Enemy");      // Enemy객체 요청.
            obj.transform.position = RandomPos();                       // 위치 랜덤 설정.
            obj.transform.parent = transform;                           // 부모 설정.
            obj.SetActive(true);                                        // 적 활성화.
            obj.GetComponent<Enemy>().Init();                           // 적 정보 초기화.
            EnemyCount++;
        }
        Summon = true;
    }
 
    // 에너미가 죽었을 때 수신하는 함수.
    public void DeadEnemy()
    {
        EnemyCount--;
    }
}
