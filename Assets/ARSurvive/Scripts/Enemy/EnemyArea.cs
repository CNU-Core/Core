using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyArea : MonoBehaviour {
 
    public List<Transform> RespawnArea; // 리스폰 구역.
 
    public int AreaCount;   // 에리어 개수.
    public int AreaGap;     // 에리어간 간격.
    public int CreateCount; // 적 머리수.
    
    void Start ()
    {
        CreateArea(AreaCount);
 
        // 에너미 생산 요청.
        ObjManager.Call().SetObject("Enemy");
        ObjManager.Call().SetObject("Enemy2");
    }
 
    // 위치 랜덤 설정.
    Vector3 RandomPos()
    {
        // AreaGap : 에리어와 에리어간의 간격.
        // 예를들어 에리어간 간격이 5라고 정해줬는데, 랜덤으로 돌려서 나온 위치가
        // 다른 에리어와 가까웠을 경우 다시 랜덤으로 돌려주는데, AreaGap보다 큰 위치 값으로 지정해줘야 한다.
        // 그렇지 않으면 AreaGap이하의 값만 나오게 되므로 무한 루프를 돌게 된다.
        float dis = AreaGap + 2;
        
        Vector3 Pos = new Vector3(this.transform.position.x + Random.Range(-dis,dis),(float)-0.88,this.transform.position.z + Random.Range(-dis,dis));
        return Pos;
    }

 
    // 리스폰 구역 생성.
    void CreateArea(int _Count = 1)
    {
        // 각 에리어의 부모가 될 객체를 생성.
        GameObject obj = new GameObject();
        obj.name = "EnemyArea";
 
        for(int i = 0; i < _Count; i++)
        {
            // 에리어의 위치를 랜덤으로 하기위한 position설정.
            Vector3 Pos = RandomPos();
            
            // 처음 객체는 아무곳에나 생성되도 된다.
            if(i != 0)
            {
                int Count = RespawnArea.Count;
 
                while(true)
                {
                    for(int j = 0 ;j < Count; j++)
                    {
                        // 새로 할당된 위치와 리스트에있는 에리어간 거리를 잰다.
                        float Dis = Vector3.Distance( Pos, RespawnArea[j].position);
 
                        // 그 거리가 설정한 간격보다 작으면
                        if(Dis < AreaGap)
                        {
                            // 위치를 재 설정한다.
                            Pos = RandomPos();
                            break;
                        }
 
                        // 모든 에리어간 거리가 일정하면 모든 반복문을 한 번에 빠져나간다.
                        if(j >= Count - 1)
                            goto EXIT;
                    }
                }
            }
 
        // goto문은 많이 사용하면 좋지 않지만, 이런경우에는 유용하게 사용 가능하다.
        EXIT:
            // 에리어 생성.
            GameObject Area = Instantiate(obj,Pos, Quaternion.identity) as GameObject;  // Area생성.
            Area.name = "EnemyArea_" + i;                                               // Area의 이름을 정한다.
            RespawnArea.Add(Area.transform);                                            // 리스트에 Area추가.
        }
 
        // 모든 에리어를 자식으로 두는 부모객체.
        for(int i = 0; i < RespawnArea.Count; i++)
        {
            Transform Area = RespawnArea[i];
            Area.parent = obj.transform;                    // 부모 객체지정.
            if(i==RespawnArea.Count-1){
                Area.gameObject.AddComponent<CreateEnemy>().Name("Enemy2");
            }
            else{
                Area.gameObject.AddComponent<CreateEnemy>().Name("Enemy");
            }
            //Area.gameObject.AddComponent<CreateEnemy>();    // Area객체에 CreateEnemy 스크립트를 추가.
        }
    }
 
    // 에너미 부모 에리어의 스크립트를 찾아준다.
    public CreateEnemy GetParentAreaScript(string _Name)
    {
        int Count = RespawnArea.Count;
        for(int i = 0; i < Count; i++)
        {
            if(RespawnArea[i].name == _Name)
                return RespawnArea[i].GetComponent<CreateEnemy>();
        }
        return null;
    }
}