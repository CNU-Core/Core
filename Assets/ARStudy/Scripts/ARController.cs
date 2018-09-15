using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구글 ARCore API 사용 선언
using GoogleARCore;

public class ARController : MonoBehaviour {

	/// <summary>
	/// 현재 Frame에서 ARCore로 인식된 Plane들을 List에 채운다.
	/// 이전> TrackedPlane에서 ARCore 1.2부터 DetectedPlane으로 이름 바뀜
	/// </summary> 
	private List<DetectedPlane> m_NewDetectedPlanes = new List<DetectedPlane>();

	/// <summary>
	/// DetectedPlane의 Prefab을 GameObject 변수로 저장
	/// </summary> 
	private GameObject DetectedPlanePrefab;

	/// <summary>
	/// A gameobject parenting UI for displaying the "searching for planes" snackbar.
	/// </summary>
	public GameObject SearchingForPlaneUI;

	/// <summary>
	/// A gameobject parenting UI for displaying the "scanning for planes" view.
	/// </summary>
	public GameObject ScanningForPlaneUI;

	public Camera firstPersonCamera;


	// Use this for initialization
	void Start () {
		QuitOnConnectionErrors();
	}
	
	// Update is called once per frame
	void Update () {
		// ARCore Session의 상태를 확인
		if(Session.Status != SessionStatus.Tracking){
			int lostTrackingSleepTimeout = 15;
			Screen.sleepTimeout = lostTrackingSleepTimeout;
			return;
		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		// 아래의 함수는 현재 Frame에서 ARCore로 인식된 Plane들을 m_NewDetectedPlanes에 채운다.
		Session.GetTrackables<DetectedPlane>(m_NewDetectedPlanes, TrackableQueryFilter.New);

		// m_NewDetectedPlanes 안에 있는 각각의 DetectedPlane을 격자로 나타냄
		for(int i = 0; i < m_NewDetectedPlanes.Count; ++ i){
			GameObject detectedPlane = Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);

			// 이 함수는 detectedPlane와 mesh와 접촉되어 변경된 vertex들의 위치를 설정 
			// detectedPlane.GetComponent<DetectedPlaneVisualizer>().Initialize(m_NewDetectedPlanes[i]);
		}
		ProcessTouches();
	}

	void ProcessTouches(){
		Touch touch;
		if (Input.touchCount != 1 ||
			(touch = Input.GetTouch (0)).phase != TouchPhase.Began)
		{
			return;
		} 

		TrackableHit hit;
		TrackableHitFlags raycastFilter =
			TrackableHitFlags.PlaneWithinBounds |
			TrackableHitFlags.PlaneWithinPolygon;

		if (Frame.Raycast (touch.position.x, touch.position.y, raycastFilter, out hit))
		{
			SetSelectedPlane(hit.Trackable as DetectedPlane);
		}
	}

	void SetSelectedPlane (DetectedPlane selectedPlane)
	{
		Debug.Log ("Selected plane centered at " + selectedPlane.CenterPose.position);
	}

	void QuitOnConnectionErrors()
	{
		if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted)
		{
			_ShowAndroidToastMessage("Camera permission is needed to run this application.");
			Invoke("_DoQuit", 0.5f);
		}
		else if (Session.Status.IsError())
		{
			// This covers a variety of errors.  See reference for details
			// https://developers.google.com/ar/reference/unity/namespace/GoogleARCore
			_ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
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
