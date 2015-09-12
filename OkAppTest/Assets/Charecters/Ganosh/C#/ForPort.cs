using UnityEngine;
using System.Collections;

public class ForPort : MonoBehaviour {

	private Animator anim;

	private void Awake () {
		anim = GetComponent <Animator> ();
	}

	private void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.SetTrigger ("Op");
		}

		Application.CaptureScreenshot (Time.frameCount.ToString()+ ".png");
	}
}
