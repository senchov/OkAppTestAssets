using UnityEngine;
using System.Collections;

public class TaurenCTRL : MonoBehaviour {

	[SerializeField] private string idleInt, movingInt,atack_SH,on_Sh,die_SH, gd_Int, atack_1_H,atack_2_H,atack_3_H,on_H,die_H;


	private Animator anim;

	private void Awake () {
		anim = GetComponent <Animator> ();


	}

	private void FixedUpdate () {
		//if (Input.GetKeyDown (KeyCode.S)) {
		//print (Time.deltaTime);
		//	Application.CaptureScreenshot (Time.frameCount.ToString() + ".png");
			
		//}
	}

	private void OnGUI () {
		if (GUI.Button (new Rect (10, 10, 110, 50), "LookAround_SH")) {
			anim.SetInteger (movingInt,0);

			anim.SetInteger (idleInt,1);
		}

		if (GUI.Button (new Rect (10, 60, 110, 50), "Roar")) {						
			anim.SetInteger (movingInt,0);

			anim.SetInteger (idleInt,2);
		}

		if (GUI.Button (new Rect (10, 110, 110, 50), "Scratching")) {						
			anim.SetInteger (movingInt,0);
			
			anim.SetInteger (idleInt,3);
		}

		if (GUI.Button (new Rect (10, 170, 110, 50), "Walk")) {		
			anim.SetInteger (idleInt,0);
			anim.SetInteger (movingInt,1);
		}

		if (GUI.Button (new Rect (10, 220, 110, 50), "Run_SH")) {		
			anim.SetInteger (idleInt,0);
			anim.SetInteger (movingInt,2);
		}

		if (GUI.Button (new Rect (10, 280, 110, 50), "Atack_SH")) {		
			SetIntToZero ();

			anim.SetTrigger (atack_SH);
		}

		if (GUI.Button (new Rect (10, 330, 110, 50), "GetDamage_0_SH")) {		
			SetIntToZero ();
			
			anim.SetInteger (gd_Int,1);
		}

		if (GUI.Button (new Rect (10, 380, 110, 50), "GetDamage_1_SH")) {		
			SetIntToZero ();
			
			anim.SetInteger (gd_Int,2);
		}

		if (GUI.Button (new Rect (10, 440, 110, 50), "OnShoulder")) {		
			SetIntToZero ();
			
			anim.SetTrigger (on_Sh);
		}

		if (GUI.Button (new Rect (10, 500, 110, 50), "Die_SH")) {		
			SetIntToZero ();
			
			anim.SetTrigger (die_SH);
		}






		if (GUI.Button (new Rect (Screen.width - 110, 10, 110, 50), "LookAround_H")) {
			SetIntToZero ();
			
			anim.SetInteger (idleInt,4);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 60, 110, 50), "Breathing")) {
			SetIntToZero ();
			
			anim.SetInteger (idleInt,5);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 120, 110, 50), "Run_0_H")) {
			SetIntToZero ();
			
			anim.SetInteger (movingInt,3);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 170, 110, 50), "Run_1_H")) {
			SetIntToZero ();
			
			anim.SetInteger (movingInt,4);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 230, 110, 50), "Atack_1_H")) {
			SetIntToZero ();
			
			anim.SetTrigger (atack_1_H);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 280, 110, 50), "Atack_2_H")) {
			SetIntToZero ();
			
			anim.SetTrigger (atack_2_H);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 320, 110, 50), "Atack_3_H")) {
			SetIntToZero ();
			
			anim.SetTrigger (atack_3_H);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 370, 110, 50), "GetDamage_1_H")) {		
			SetIntToZero ();
			
			anim.SetInteger (gd_Int,3);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 420, 110, 50), "GetDamage_2_H")) {		
			SetIntToZero ();
			
			anim.SetInteger (gd_Int,4);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 480, 110, 50), "On_Hand")) {		
			SetIntToZero ();
			
			anim.SetTrigger (on_H);
		}

		if (GUI.Button (new Rect (Screen.width - 110, 540, 110, 50), "Die_H")) {		
			SetIntToZero ();
			
			anim.SetTrigger (die_H);
		}

		GUI.Label (new Rect (Screen.width-200,Screen.height-100,200,100),"RMB stop camera rotation, MMB zoom");
	}

	private void SetIntToZero () {
		anim.SetInteger (idleInt,0);			
		anim.SetInteger (movingInt,0);
		anim.SetInteger (gd_Int,0);
	}
}
