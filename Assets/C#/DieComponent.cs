using UnityEngine;
using System.Collections;

public class DieComponent : MonoBehaviour
{	
	[SerializeField]
	private float
		_descentSpeed;

	private Animator _anim;
	private MoveComponent _moveComponent;
	private BoxCollider _box;

	private void Awake ()
	{
		_anim = gameObject.GetComponent <Animator> ();
		_moveComponent = gameObject.GetComponent <MoveComponent> ();
		_box = gameObject.GetComponent <BoxCollider> ();

		if (!_anim) {
			Debug.Log ("Animator is null");
			return;
		}

		if (!_moveComponent) {
			Debug.Log ("MoveComponent is null");
			return;
		}
	}

	private void OnEnable ()
	{
		_box.enabled = true;
		StopAllCoroutines ();
		CancelInvoke ();
	}

	public void Die ()
	{
		StopAllCoroutines ();
		CancelInvoke ();

		if (!gameObject.activeInHierarchy)
			return;

		_anim.SetTrigger ("Die");
		_moveComponent.Speed = 0;
		_box.enabled = false;
		Invoke ("Descent", 2.0f);
	}

	private void Descent ()
	{
		if (gameObject.activeInHierarchy)
			StartCoroutine (GoDown ());
	}

	private IEnumerator GoDown ()
	{
		float target = -5;

		while (transform.position.y > target) {
			transform.position = Vector3.MoveTowards (transform.position, 
			                                          new Vector3 (transform.position.x, transform.position.y + target, transform.position.z),
			                                          _descentSpeed * Time.deltaTime);
			yield return null;

		}

		gameObject.SetActive (false);
	}

	public void TurnOff ()
	{	
		CancelInvoke ();
		StopAllCoroutines ();
		gameObject.SetActive (false);
	}

	private void OnDisable ()
	{
		StopAllCoroutines ();
		CancelInvoke ();
	}

}
