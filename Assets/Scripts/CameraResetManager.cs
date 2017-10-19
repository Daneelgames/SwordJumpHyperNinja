using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraResetManager : MonoBehaviour {
	
	public Camera cam;
	public GUILayer gUILayer;
	public FlareLayer flareLayer;
	public Animator animator;
	public AudioListener audioListener;
	public PostProcessingBehaviour postProcessingBehaviour;
	public void Reset()
	{
		cam.enabled = true;
		gUILayer.enabled = true;
		flareLayer.enabled = true;
		animator.enabled = true;
		audioListener.enabled = true;
		postProcessingBehaviour.enabled = true;
	}
}