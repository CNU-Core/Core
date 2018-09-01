using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class AnchorManager : MonoBehaviour {

	public GameObject anchoredPrefab;
	public GameObject unanchoredPrefab;

	Anchor anchor;

	Vector3 lastAnchoredPosition;
	Quaternion lastAnchoredRotation;

	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began){
			Pose pose = new Pose(transform.position, transform.rotation);
			anchor = Session.CreateAnchor(pose);
			GameObject.Instantiate(anchoredPrefab, 
			anchor.transform.position, 
			anchor.transform.rotation, 
			anchor.transform);
			GameObject.Instantiate(unanchoredPrefab,
			anchor.transform.position,
			anchor.transform.rotation);
			lastAnchoredPosition = anchor.transform.position;
			lastAnchoredRotation = anchor.transform.rotation;
		}
		if(anchor == null){
			return;
		}
		if(anchor.transform.position != lastAnchoredPosition){
			Debug.Log("Position Changed: " + Vector3.Distance(anchor.transform.position, lastAnchoredPosition));
			lastAnchoredPosition = anchor.transform.position;
		}
		if(anchor.transform.rotation != lastAnchoredRotation){
			Debug.Log("Angle Changed: " + Quaternion.Angle(anchor.transform.rotation, lastAnchoredRotation));
			lastAnchoredRotation = anchor.transform.rotation;
		}
	}
}
