using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class LightEstimationTest : MonoBehaviour {

	[Range(0f, 1f)]
	public float testValue = 0.5f;
	
	void OnValidate(){
		SetGlobalLightEstimation(testValue);
	}
	void SetGlobalLightEstimation(float lightValue){
		Shader.SetGlobalFloat("_GlobalLightEstimation", lightValue);
	}
	// Update is called once per frame
	void Update () {
		SetGlobalLightEstimation(Frame.LightEstimate.PixelIntensity);
	}
}
