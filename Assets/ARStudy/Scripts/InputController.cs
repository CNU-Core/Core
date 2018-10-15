using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
	
	//public GameObject cubePrefab;
	public GameObject doorPrefab;

	// Update is called once per frame
	void Update () {
		foreach(var touch in Input.touches){
			if(touch.phase != TouchPhase.Began){
				continue;
			}
			// if(Input.touchCount > 1 && touch.phase == TouchPhase.Began){
			// 	AddCube();
			// }
			// Shoot(touch.position);
			// if(touch.phase == TouchPhase.Began){
			// 	StackBox(touch.position);
			// 	continue;
			// }
			if(touch.phase == TouchPhase.Began){
				BuildDoor();
				continue;
			}
		}
	}

	// void AddCube(){
	// 	GameObject.Instantiate(cubePrefab, transform.position + transform.forward * 0.3f, Random.rotation);
	// }

	void BuildDoor(){
		GameObject.Find("Surface").transform.GetComponent<Surface>().MakeDoor(doorPrefab);
	}

	void Shoot(Vector2 screenPoint){
		var ray = Camera.main.ScreenPointToRay(screenPoint);
		var hitInfo = new RaycastHit();
		if(Physics.Raycast(ray, out hitInfo)){
			hitInfo.rigidbody.AddForceAtPosition(ray.direction, hitInfo.point);
		}
	}
	// void StackBox(Vector2 screenPoint){
	// 	var ray = Camera.main.ScreenPointToRay(screenPoint);
	// 	RaycastHit hitInfo;
	// 	if(Physics.Raycast(ray, out hitInfo)){
	// 		var go = GameObject.Instantiate(cubePrefab, hitInfo.point + Vector3.up, Quaternion.identity);
	// 		go.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
	// 	}
	// }
}
