using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ObjManager : MonoBehaviour {
 
    // 싱글톤
    static ObjManager st;
    public static ObjManager Call() { return st; }
    void Awake()                    { st = this; }
    // 게임종료 후 메모리 날려버림.
    void OnDestroy()                
    {
        MemoryDelete();
        st = null;
    }
    

 
    // 공개
    public GameObject[] Origin;         // 프리팹 원본.
    public List<GameObject> Manager;    // 생성된 객체들을 저장할 리스트.
 
    // 비공개.
    private Player.PINFO pInfo;         // 플레이어 정보.
 
    // 오브젝트를 받아 생성.
    public void SetObject( GameObject _Obj, int _Count, string _Name)
    {
        Debug.Log(_Name + "현재 count는" + _Count);
        for (int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(_Obj) as GameObject;
            obj.transform.name = _Name;                     // 이름을 정한다.
            obj.transform.localPosition = Vector3.zero;     // 위치를 정한다.
            obj.SetActive(false);                           // 객체를 비활성화.
            obj.transform.parent = transform;               // 매니저 객체의 자식으로.
            
            if(_Name == "Bullet"){ // 총알이 아니면 색을 랜덤으로 설정.
                obj.transform.GetComponent<SphereCollider>().isTrigger = true;
            }
            else if (_Name == "Enemy"){
                Debug.Log("좀비생성");    
            }
            else
                obj.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
 
            Manager.Add(obj);                               // 리스트에 저장.
        }
    }
 
    public void SetObject(string _Name, int _Count = 20)
    {
        GameObject obj = null;
        int Count = Origin.Length;
        for(int i = 0; i < Count; i++)
        {
            if (Origin[i].name == _Name)
                obj = Origin[i];
        }
 
        SetObject(obj, _Count, _Name);
    }
 
    // 필요한 오브젝트를 찾아 반환.
    public GameObject GetObject(string _Name)
    {
        // 리스트가 비어있으면 종료.
        if (Manager == null)
            return null;
 
        int Count = Manager.Count;
        for (int i = 0; i < Count; i++)
        {
            // 이름이 같지 않으면.
            if (_Name != Manager[i].name)
                continue;
 
            GameObject Obj = Manager[i];
 
            // 활성화가 되어있다면.
            if (Obj.active == true)
            {
                // 리스트의 마지막까지 돌았지만 모든 객체가 사용중이라면.
                if (i == Count - 1)
                {
                    // 총알을 새롭게 생성.
                    SetObject(Obj, 1, _Name);
                    return Manager[i + 1];
                }
                continue;
            }
            return Manager[i]; 
        }
        return null;
    }
 
    // 메모리 삭제.
    public void MemoryDelete()
    {
        if (Manager == null)
            return;
 
        int Count = Manager.Count;
 
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Manager[i];
            GameObject.Destroy(obj);
        }
        Manager = null;
    }
 
    // 플레이어의 정보갱신.
    public void PlayerInfoUpdate(Player.PINFO _Info)
    {
        // 플레이어 정보 업데이트.
        pInfo = _Info;
 
        int Count = Manager.Count;
 
        for (int i = 0; i < Count; i++)
        {
            GameObject obj = Manager[i];
            if (obj.name == "Bullet")
                obj.GetComponent<Bullet>().BulletPower = _Info.BulletPower;
        }
    }
 
    // 플레이어의 정보를 가져간다.
    public object GetPlayerInfo(string _Type)
    {
        switch (_Type)
        {
            case "Life":        return pInfo.Life;
            case "MAXHP":       return pInfo.MAX_HP;
            case "HP":          return pInfo.HP;
            case "Speed":       return pInfo.MoveSpeed;
            case "BulletPower": return pInfo.BulletPower;
            case "All":         return pInfo;
        }
        return null;
    }
}
