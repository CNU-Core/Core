using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class Surface : MonoBehaviour {

	// 바닥 인식된 Point의 정보를 가져오는 변수
	public DetectedPlane detectedPlane;

	// 인식된 바닥에 Mesh Render하여 가상의 바닥을 입혀줌.
	MeshRenderer meshRenderer;

	// 생성된 바닥에 콜라이더를 씌우기 위한 변수
	MeshCollider meshCollider;
	Mesh mesh;

	public List<Vector3> savedVertices;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();

		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		savedVertices = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
		if(detectedPlane == null){
			return;
		}
		else if(detectedPlane.SubsumedBy != null){
			Destroy(gameObject);
			return;
		}
		// 다른 곳을 봐도 추적중인 것들은 사라지지 않게끔 함
		else if(detectedPlane.TrackingState != TrackingState.Tracking){
			meshRenderer.enabled = false;
			return;
		}

		meshRenderer.enabled = true;

		UpdateVertices();
	}

	void UpdateVertices(){
		var vertices = new List<Vector3>();
		detectedPlane.GetBoundaryPolygon(vertices);
		detectedPlane.GetBoundaryPolygon(savedVertices);	
		vertices.Add(detectedPlane.CenterPose.position);
		savedVertices.Add(detectedPlane.CenterPose.position);
		
		var triangles = new List<int>();
		// 센터를 중심으로 주변 점들을 연결하여 삼각형을 만듦.
		for(int i = 0; i < vertices.Count - 1; i ++){
			triangles.Add(vertices.Count - 1);
			triangles.Add(i);
			triangles.Add(i == vertices.Count - 2 ? 0 : i + 1);
		}
		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles.ToArray(), 0);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		meshCollider.sharedMesh = mesh;
	}

	public void MakeDoor(GameObject door){
		Debug.Log("문 생성!");
		Debug.Log("좌표 갯수: " + savedVertices.Count);
		int rand = Random.Range(0, savedVertices.Count);
		Debug.Log("랜덤 숫자: " + rand);
		Vector3 randVertice = savedVertices[rand];
		Debug.Log("랜덤 좌표: " + randVertice);
		GameObject.Instantiate(door, randVertice, Quaternion.identity);
	}
}
