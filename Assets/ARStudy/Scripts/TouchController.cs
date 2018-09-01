using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {

   // Use this for initialization
   void Start () {
      Debug.Log("시작!");
   }
   
   // Update is called once per frame
   void Update () {
      if(Input.touchCount > 0){
         Debug.Log("들어왔니");
         Vector2 pos = Input.GetTouch(0).position;//터치한 위치 저장
         Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);

         Ray ray = Camera.main.ScreenPointToRay(theTouch); //터치한 좌표를 ray로
         RaycastHit hit; // 정보를 저장할 구조체, 충돌객체 정보
         if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
            if(hit.collider.CompareTag("Cube")){
               Debug.Log("야앗");
            }
         }
      }
   }
}