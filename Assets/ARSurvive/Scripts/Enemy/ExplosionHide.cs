using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ExplosionHide : MonoBehaviour {
 
    // 블럭이 사라진다.
    IEnumerator StartExplosionHide()
    {
        int Rand = Random.Range(3, 7);
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if(timer > Rand)
                break;
 
            yield return null;
        }
 
        // 물체가 사라진 후 다시 소환됐을 때 전에 받던 물리력을 없애주기위한 체크.
        transform.GetComponent<Rigidbody>().isKinematic = true;
        // 물리력을 다시 받기 위해 체크 해제.
        transform.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.SetActive(false);
    }
}
