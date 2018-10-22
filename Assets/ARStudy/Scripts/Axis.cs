using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Axis : MonoBehaviour {
 
    // 공개
    public float Distance;          // 카메라와의 거리.
 
    // 비공개
    private Vector3 AxisVec;        // 축의 벡터.
    private Transform MainCamera;   // 카메라 컴포넌트.
 
    void Start()
    {
        MainCamera = Camera.main.transform;
    }
 
    void Update()
    {
        DisCamera();
    }
 
    void DisCamera()
    {
        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }
}
