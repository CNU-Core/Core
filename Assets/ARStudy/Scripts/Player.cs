using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Player : MonoBehaviour {
 
    public float Speed;         // 움직이는 스피드.
 
    private Transform Vec;      // 카메라 벡터.
    private Vector3 MovePos;    // 플레이어 움직임에 대한 변수.
 
    void Init()
    {
        MovePos = Vector3.zero;
    }
 
    void Start()
    {
        Vec = GameObject.Find("CameraVector").transform;
        Init();
    }
 
    void Update () 
    {
        Run();
    }
 
    // 플레이어 움직임.
    void Run()
    {
        int ButtonDown = 0;
        if (Input.GetKey(KeyCode.LeftArrow))    ButtonDown = 1;
        if (Input.GetKey(KeyCode.RightArrow))   ButtonDown = 1;
        if (Input.GetKey(KeyCode.UpArrow))      ButtonDown = 1;
        if (Input.GetKey(KeyCode.DownArrow))    ButtonDown = 1;
 
        // 플레이어가 움직임. 버튼에서 손을 땠을 때 Horizontal, Vertical이 0으로 돌아감으로써
        // 플레이어의 회전상태가 다시 원상태로 돌아가지 않게 하기 위해서.
        if (ButtonDown != 0)
            Rotation();
        else
            return;
 
        transform.Translate(Vector3.forward * Time.deltaTime * Speed * ButtonDown);
    }
 
    // 플레이어 회전.
    void Rotation()
    {
        MovePos.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));   // 벡터 셋팅.
        Quaternion q = Quaternion.LookRotation(Vec.TransformDirection(MovePos));  // 회전
 
        if (MovePos != Vector3.zero)
            transform.rotation = q;
    }
}
