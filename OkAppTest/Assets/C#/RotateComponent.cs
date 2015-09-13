using UnityEngine;
using System.Collections;

public class RotateComponent : MoveComponent
{
	[SerializeField]
	private float[]
		_turnAngles;


	private float _turnPoint;

	private bool _alreadyTurn;

	private void Start ()
	{
		DetermineTurnPoint ();
	}

	protected override void Move ()
	{
		base.Move ();

		if (!_gameCtrl) {
			return;
		}
		
		if (!_alreadyTurn && transform.position.z > _turnPoint) {
			Turn ();
			_alreadyTurn = true;
		}
		
		if (transform.position.x + _gameCtrl.UnitSize * 0.5f > _gameCtrl.Right.position.x) {
			transform.rotation = Quaternion.Euler (new Vector3 (0, _turnAngles [_turnAngles.Length - 1], 0));
		}
		
		if (transform.position.x - _gameCtrl.UnitSize * 0.5f < _gameCtrl.Left.position.x) {
			transform.rotation = Quaternion.Euler (new Vector3 (0, _turnAngles [0], 0));
		}
	}


	private void DetermineTurnPoint ()
	{
		if (!_gameCtrl) {
			Debug.Log ("GameController is null");
			return;
		}
		
		float pathLenght = Vector3.Distance (_gameCtrl.Top.position, _gameCtrl.Bottom.position);
		float[] tempArr = new float[2];
		tempArr [0] = _gameCtrl.Top.position.z + pathLenght * 0.33f;
		tempArr [1] = _gameCtrl.Top.position.z + pathLenght * 0.66f;
		_turnPoint = tempArr [Random.Range (0, 2)];
	} 

	private void Turn ()
	{
		float temp = _turnAngles [Random.Range (0, _turnAngles.Length)];
		transform.rotation = Quaternion.Euler (new Vector3 (0, temp, 0));
	}

	private void OnDisable ()
	{
		_alreadyTurn = false;
	}
}
