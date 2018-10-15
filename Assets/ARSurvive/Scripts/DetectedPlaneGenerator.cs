//-----------------------------------------------------------------------
// <copyright file="DetectedPlaneGenerator.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
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

    /// <summary>
    /// 씬(scene)에서 검출된 평면의 시각화를 관리합니다.
    /// Manages the visualization of detected planes in the scene.
    /// </summary>
    public class DetectedPlaneGenerator : MonoBehaviour
    {
        /// <summary>
        /// 탐지된 평면을 추적하고 시각화하기 위한 사전 장치입니다.
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// ARCore가 현재 프레임에서 추적하기 시작한 새 평면을 보유하기 위한 목록. 이 개체는 프레임별 할당을 방지하기 위해 애플리케이션 전체에서 사용됩니다.
        /// A list to hold new planes ARCore began tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        private List<Vector3> m_MeshVertices = new List<Vector3>();

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // 모션트래킹에서 계속 트래킹하고 있는지 확인
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }
            // 이 프레임에서 발견된 평면에 대해 반복하고 해당 GameObjects를 인스턴스화하여 시각화합니다.
            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                // 평면 시각화 도구를 인스턴스화하고 새 평면을 추적하도록 설정합니다. Prefab의 메쉬가 UnityWorld좌표에서 업데이트되므로 변환은 ID를 회전하여 오리진으로 설정됩니다.
                // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                // coordinates.
                GameObject planeObject = Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
            }
        }

        /// <summary>
        /// 좀비가 Respawn될 문의 위치를 설정하여 생성
        /// </summary>
        /// <param name="doorPref">문에 대한 프리팹을 GameObject로 넣음</param>
        public void InitRespawn(GameObject doorPref){
            // DetectedPlane에서 인식된 모든 Plane을 m_AllPlanes 리스트 안에 저장
            Session.GetTrackables<DetectedPlane>(m_AllPlanes, TrackableQueryFilter.All);
            Debug.Log("newPlane 갯수: " + m_AllPlanes.Count);
            
            // m_AllPlanes 리스트 안에 0번째 인덱스(바닥이며 스캔할 때 처음 흰색부분을 기준) Plane의 Vertices를 m_MeshVertices에 저장
            m_AllPlanes[0].GetBoundaryPolygon(m_MeshVertices);
            Debug.Log("갯수는?: " + m_MeshVertices.Count);
            Debug.Log("설마 위치가?: " + m_MeshVertices[0]);

            // 문의 위치 설정
            doorPref.transform.position = m_MeshVertices[0];

            // 문의 방향 설정 (카메라가 있는 곳을 중심으로 회전됨)
            Vector3 vec = transform.position - doorPref.transform.position;
            vec.y = 0f;
            vec.Normalize();
            Quaternion q = Quaternion.LookRotation(vec);
            doorPref.transform.rotation = q;
            Debug.Log("====== 위치 배정! ======");
        }
    }
}