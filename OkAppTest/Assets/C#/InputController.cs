using UnityEngine;
using System.Collections;
using System;

public class InputController : MonoBehaviour
{

	[SerializeField]
	private Camera
		_myCamera;

	[SerializeField]
	private GameController
		_gameCtrl;

	public event Action LoseGame;
	
	private void LoseGameHandler ()
	{
		if (LoseGame != null)
			LoseGame ();
	}

	public event Action KillUnit;
	
	private void KillUnitHandler ()
	{
		if (KillUnit != null)
			KillUnit ();
	}

	private void LateUpdate ()
	{
		#if UNITY_ANDROID
		TouchControll ();
		#elif UNITY_EDITOR
		if (Input.GetMouseButtonDown (0)) {
			RaycastMethod (Input.mousePosition);
		}
		#endif
		
	}

	private void RaycastMethod (Vector2 temp)
	{
		RaycastHit hit;
		
		Ray ray = _myCamera.ScreenPointToRay (temp);
		
		if (Physics.Raycast (ray, out hit)) {

			DieComponent dieComp = hit.collider.gameObject.GetComponent <DieComponent> ();
			if (!dieComp)
				return;

			if (dieComp.Unit == Unit.Enemy) {
				dieComp.Die ();
				KillUnitHandler ();
			} else
				LoseGameHandler ();

		}
		

	}
	
	private void TouchControll ()
	{
		if (Input.touches.Length <= 0) {
			
		} else {
			for (int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					RaycastMethod (Input.GetTouch (i).position);
				}
			}
			
		}
	}
}
