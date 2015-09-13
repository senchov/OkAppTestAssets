using UnityEngine;
using System.Collections;

public class MoveComponent : MonoBehaviour
{

	[SerializeField]
	protected float
		_speed;

	[SerializeField]
	private bool
		_isEnemy;

	protected GameController _gameCtrl;

	private float _startSpeed;

	private Transform myTransform;
	protected new Transform transform {
		get {
			if (myTransform == null)
				myTransform = base.transform;
			return myTransform;
		}
		set {}
	}


	public float Speed {
		set { _speed = value;}
	}

	private void Awake ()
	{
		_gameCtrl = GameObject.Find ("GameController").GetComponent<GameController> ();

		_startSpeed = _speed;
	}

	private void Update ()
	{
		Move ();
	}

	protected virtual void Move ()
	{
		transform.Translate (transform.forward * Time.deltaTime * _speed);
		
		
		if (_gameCtrl && transform.position.z > _gameCtrl.Bottom.position.z + 2 * _gameCtrl.UnitSize) {
			gameObject.SetActive (false);
			if (_isEnemy)
				_gameCtrl.LoseLife ();
		}
	}

	private void OnEnable ()
	{
		_speed = _startSpeed + _gameCtrl.Level;

		int temp = Random.Range (0, 4);

		_speed = (temp == 3) ? _speed * 2 : _speed;

	}

}
