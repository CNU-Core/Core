using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Bullet : MonoBehaviour {
 
    public float BulletPower;
 
    // 총알의 움직임 및 일정 시간뒤 비 활성화.
    IEnumerator MoveBullet()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;    // 시간 축적
            if(timer > 2)               // 2초뒤 반복문을 빠져나간다.
                break;
 
            transform.Translate(Vector3.forward * Time.deltaTime * 40f);    // 총알을 움직인다.
            yield return null;
        }
 
        // 총알 비활성화.
        gameObject.SetActive(false);
    }
    
    void OnCollisionEnter(Collision col){
        if(col.transform.CompareTag("Wall")){
            Destroy(col.transform.GetComponent<SphereCollider>());
        }
    }
    
    // 충돌체크
    void OnTriggerEnter(Collider _Col)
    {
        // 총알이 적과 충돌.
        if (_Col.transform.CompareTag("Enemy"))
        {
            // 총알에 맞은 객체의 Enemy컴포넌트를 가져와 Enemy에게 데미지를 준다.
            _Col.GetComponent<Enemy>().InfoUpdate(BulletPower);
            gameObject.SetActive(false);
        }
    }
}
