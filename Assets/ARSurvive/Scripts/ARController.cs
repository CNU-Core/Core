//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace ARSurvive
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = GoogleARCore.InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class ARController : MonoBehaviour
    {
        [Header("=========== 테스트용 로그인 화면 끄기 ============")]
        [SerializeField]
        private bool PlayingLoginView = true;


        [Header("=========== ARCore Controller 설정 ============")]
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// A gameobject parenting UI for displaying the "scanning for planes" view.
        /// </summary>
        public GameObject ScanningForPlaneUI;

        public GameObject door;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        private Transform canvas;

        public void StartMenu(){
            canvas.GetChild(0).gameObject.SetActive(false);
            this.PlayingLoginView = false;
        }

        public void Start(){
            canvas = GameObject.Find("Canvas").transform;
            canvas.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener( delegate{ StartMenu(); } );
            ScanningForPlaneUI.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener( delegate{ MakeRespawn();});

            if(!this.PlayingLoginView){
                this.StartMenu();
            }
        }
        
        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            // 시작화면인지 아닌지 확인
            if(this.PlayingLoginView){
                return;
            }

            // 트레킹 중에는 Snakbar가 사라지게끔 함
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);
            ScanningForPlaneUI.SetActive(!showSearchingUI);

            // 화면에 터치가 되지 않을 경우, Update함수를 여기까지만 사용할 수 있게 설정
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // // Raycast against the location the player touched to search for planes.
            // TrackableHit hit;
            // TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            //     TrackableHitFlags.FeaturePointWithSurfaceNormal;

            // if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            // {
            //     // Use hit pose and camera pose to check if hittest is from the
            //     // back of the plane, if it is, no need to create the anchor.
            //     if ((hit.Trackable is DetectedPlane) &&
            //         Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
            //             hit.Pose.rotation * Vector3.up) < 0)
            //     {
            //         Debug.Log("Hit at back of the current DetectedPlane");
            //     }
            //     else
            //     {
            //         // Instantiate Andy model at the hit pose.
            //         var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation);

            //         // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
            //         andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

            //         // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
            //         // world evolves.
            //         var anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //         // Make Andy model a child of the anchor.
            //         andyObject.transform.parent = anchor.transform;
            //     }
            // }
        }

        // /// <summary>
        // /// 스캔완료 버튼을 누를 시 모서리를 알려주는 함수
        // /// </summary>
        // private void CheckEdge(){
        //     DetectedPlanePrefab.GetComponent<DetectedPlaneVisualizer>()._FindEdge(AndyAndroidPrefab);
        //     Debug.Log("Main LeftTop: " + leftTop);
        //     Debug.Log("Main RightTop: " + rightTop);
        //     Debug.Log("Main leftBottom: " + leftBottom);
        //     Debug.Log("Main rightBottom: " + rightBottom);
        // }

        /// <summary>
        /// 스캔완료 버튼을 누를 시 가동되는 함수
        /// </summary>
        private void MakeRespawn(){
            Debug.Log("버튼눌림");
            GameObject doorPreb = GameObject.Instantiate(door, Vector3.forward, Quaternion.identity);
            Debug.Log("생성됨");
            GameObject.Find("Plane Generator").GetComponent<DetectedPlaneGenerator>().InitRespawn(doorPreb);
            Debug.Log("버튼종료");
        }

        /// <summary> 
        /// 어플리케이션의 생명주기를 확인 및 업데이트하는 함수
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // '뒤로가기' 버튼을 눌렀을 경우 어플리케이션이 꺼질 수 있게 함
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // 트레킹 되지 않을 경우에만 화면이 꺼지도록 함
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("현재 어플리케이션을 실행하려면 카메라 접근을 허용해주셔야 합니다.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore 연결 중 오류가 발생되었습니다. 어플리케이션을 다시 시작해주세요.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// 어플리케이션을 종료할 때 사용
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// 안드로이드 Toast 메세지 보여주는 함수
        /// </summary>
        /// <param name="message">Toast에 보여줄 메세지를 적음.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
