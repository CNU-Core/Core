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

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    //편집기에서 InstantPreview를 사용하는 동안 터치식 입력 전파를 설정합니다.
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// 패스 스루 카메라 이미지(즉, AR배경)를 렌더링 하는 데 사용되는 1인칭 카메라입니다.
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// 탐지된 평면을 추적하고 시각화하기 위한 사전 장치입니다.
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// 사용자가 터치한 후의 Raycast가 평면에 닿을 때 배치할 모델.
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;

        /// <summary>
        /// "평면 검색"snackbar를 표시하기 위한 게임 개체 관리 UI입니다.
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// "평면 스캔"보기를 표시하기 위한 게임 개체 관리 UI입니다.
        /// A gameobject parenting UI for displaying the "scanning for planes" view.
        /// </summary>
        public GameObject ScanningForPlaneUI;

        /// <summary>
        /// 앤디 모델을 배치할 때 모델에 도 단위로 회전을 적용해야 합니다.
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// ARCore가 현재 프레임에서 추적 중인 모든 평면을 포함하는 목록입니다. 이 개체는 프레임별 할당을 방지하기 위해 애플리케이션 전체에서 사용됩니다.
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// ARCore연결 오류로 인해 앱이 종료되는 중이라면 True이고, 그렇지 않으면 False입니다.
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;
        private bool m_IsStart = false;

        private Transform canvas;

        public void StartMenu(){
            canvas.GetChild(0).gameObject.SetActive(false);
            this.m_IsStart = true;
        }

        public void Start(){
            canvas = GameObject.Find("Canvas").transform;
            canvas.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener( delegate{ StartMenu(); } );
        }
        
        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            // 시작화면인지 아닌지 확인
            if(!m_IsStart){
                return;
            }
            // 하나 이상의 평면을 현재 추적할 때 snackbar를 숨깁니다.
            // Hide snackbar when currently tracking at least one plane.
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

            //플레이어가 화면을 터치하지 않으면 이 업데이트가 완료된 것입니다.
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }
            //선수가 비행기를 찾기 위해 접촉한 위치에 대한 레이스트.
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                //타격 자세와 카메라 자세를 사용하여 비행기 뒤쪽에서 가장 무거운 물체가 있는지 확인하고, 그렇다면 앵커를 만들 필요가 없습니다.
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // 앤디 모델을 히트 자세로 소개하세요.
                    // Instantiate Andy model at the hit pose.
                    var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation);
                    
                    // Raycast(카메라)에서 떨어진 곳에 있는 hitPose회전에 대해 보상합니다.
                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // ARCore가 물리적 세계의 이해가 진화함에 따라 하이프 포인트를 추적할 수 있도록 앵커를 작성합니다.
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // 앤디가 모델이 앵커의 아이가 되도록 만들어라.
                    // Make Andy model a child of the anchor.
                    andyObject.transform.parent = anchor.transform;
                }
            }
        }

        /// <summary>
        /// 애플리케이션 수명 주기를 확인하고 업데이트합니다.
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // '뒤로 '버튼을 누르면 앱을 종료합니다.
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            //추적하지 않을 때만 화면을 절전 모드로 설정합니다.
            // Only allow the screen to sleep when not tracking.
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

            // ARCore를 연결할 수 없는 경우 종료하고 Unity에 토스트가 나타날 시간을 줍니다.
            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
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
