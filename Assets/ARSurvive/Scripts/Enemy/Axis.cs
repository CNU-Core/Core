using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Axis : MonoBehaviour {
 
    public Quaternion TargetRotation;  // 최종적으로 축적된 Gap이 이 변수에 저장됨.
    public Transform CameraVector;
 
    public float RotationSpeed;        // 회전 스피드.
    public float ZoomSpeed;            // 줌 스피드.
    public float Distance;             // 카메라와의 거리.
 
    private Vector3 AxisVec;           // 축의 벡터.
    private Vector3 Gap;               // 회전 축적 값.
    
    private Transform MainCamera;      // 카메라 컴포넌트.
 
    void Start()
    {
        MainCamera = Camera.main.transform;
    }
 
    void Update()
    {
        Zoom();
        CameraRotation();
    }
 
    // 카메라 줌.
    void Zoom()
    {
        Distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * -1;
        Distance = Mathf.Clamp(Distance, 5f, 20f);
 
        AxisVec = transform.forward * -1;
        AxisVec *= Distance;
        MainCamera.position = transform.position + AxisVec;
    }
 
    // 카메라 회전.
    void CameraRotation()
    {
        if (transform.rotation != TargetRotation)
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, RotationSpeed * Time.deltaTime);
 
        if (Input.GetMouseButton(1))
        {
            // 값을 축적.
            Gap.x += Input.GetAxis("Mouse Y") * RotationSpeed * -1;
            Gap.y += Input.GetAxis("Mouse X") * RotationSpeed;
 
            // 카메라 회전범위 제한.
            Gap.x = Mathf.Clamp(Gap.x, -5f, 85f);
            // 회전 값을 변수에 저장.
            TargetRotation = Quaternion.Euler(Gap);
 
            // 카메라벡터 객체에 Axis객체의 x,z회전 값을 제외한 y값만을 넘긴다.
            Quaternion q = TargetRotation;
            q.x = q.z = 0;
            CameraVector.transform.rotation = q;
        }
    }
}