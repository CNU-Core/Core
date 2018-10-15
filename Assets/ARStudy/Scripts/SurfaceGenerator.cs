using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class SurfaceGenerator : MonoBehaviour {

	public GameObject surfacePrefab;

	void Update(){
		// Check that motion tracking is tracking.
		if (Session.Status != SessionStatus.Tracking)
		{
			return;
		}

		var newPlanes = new List<DetectedPlane>();

		Session.GetTrackables<DetectedPlane>(newPlanes, TrackableQueryFilter.New);

		foreach(var p in newPlanes){
			var go = GameObject.Instantiate(surfacePrefab, Vector3.zero, Quaternion.identity);
			go.GetComponent<Surface>().detectedPlane = p;
		}	
	}
}
