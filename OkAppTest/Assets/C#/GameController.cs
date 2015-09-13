using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
	#region Fields
	[SerializeField]
	private Transform
		_top;
	[SerializeField]
	private Transform
		_bottom;
	[SerializeField]
	private Transform
		_right;
	[SerializeField]
	private Transform
		_left;

	[SerializeField]
	private Camera
		_myCamera;

	[SerializeField]
	private GameObject
		_unit_1;
	[SerializeField]
	private GameObject
		_unit_2;
	[SerializeField]
	private GameObject
		_unit_3;

	[SerializeField]
	private UIManager
		_uiManager;

	[SerializeField]
	private ParticleSystem
		_explosion;

	[SerializeField]
	[Range (5.0f,30.0f)]
	private float
		_explosionRadius;

	private float _unitSize = 2.26f;
	private float _spawnTime = 2;
	private float _minTimeSpawn = 1;
	private float _maxTimeSpawn = 2;
	private int _lives = 2;
	private int _level = 1;
	private int _bombAmount = 3;
	private Vector3[] _spawnPoints;

	private List <GameObject> _units;
	private List <GameObject> _enemies;
	#endregion 

	#region Properties
	public Transform Top {
		get { return _top;}
	}

	public Transform Bottom {
		get { return _bottom;}
	}

	public Transform Left {
		get { return _left;}
	}

	public Transform Right {
		get { return _right;}
	}

	public float UnitSize {
		get { return _unitSize;}
	}

	public int Level {
		get { return _level;}
	}

	public int BombAmount {
		get { return _bombAmount;}
	}
	#endregion

	#region Events
	public event Action LevelUp;

	private void LevelUpHandler ()
	{
		if (LevelUp != null)
			LevelUp ();
	}

	public event Action LoseGame;

	private void LoseGameHandler ()
	{
		if (LoseGame != null)
			LoseGame ();
	}

	public event Action<int> LoseLifeEvent;

	private void LoseLifeEventHandler (int temp)
	{
		if (LoseLifeEvent != null)
			LoseLifeEvent (temp);
	}

	public event Action KillUnit;

	private void KillUnitHandler ()
	{
		if (KillUnit != null)
			KillUnit ();
	}
	#endregion

	private void Awake ()
	{

		#region NullCheck
		if (!_top) {
			Debug.Log ("top null");
			return;
		}
		
		if (!_bottom) {
			Debug.Log ("bottom null");
			return;
		}
		
		if (!_right) {
			Debug.Log ("right null");
			return;
		}
		
		if (!_left) {
			Debug.Log ("left null");
			return;
		}
		
		if (!_myCamera) {
			Debug.Log ("Camera null");
			return;
		}
		
		if (!_unit_1) {
			Debug.Log ("unit1 null");
			return;
		}
		
		if (!_unit_2) {
			Debug.Log ("unit2 null");
			return;
		}
		
		if (!_unit_3) {
			Debug.Log ("unit3 null");
			return;
		}
		
		if (!_uiManager) {
			Debug.Log ("UIManager is null");
			return;
		}

		if (!_explosion) {
			Debug.Log ("Explosion Particle is null");
			return;
		}
		#endregion
	
		DetermineBorders ();
		CreateSpawnPointArr ();
	
		_units = new List<GameObject> ();
		_enemies = new List<GameObject> ();

		_uiManager.StartGame += HandleStartGame;
		_uiManager.TimeIsOver += HandleTimeIsOver;
		_uiManager.Restart += HandleRestart;
		_uiManager.Explosion += HandleExplosion;
		LevelUp += LeveUpMethod;
		LoseGame += LoseGameMethod;


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

	private void SpawnUnits ()
	{
		GameObject obj = _units [UnityEngine.Random.Range (0, _units.Count)].Spawn ();

		if (!_enemies.Contains (obj))
			_enemies.Add (obj);


		obj.transform.position = _spawnPoints [UnityEngine.Random.Range (0, _spawnPoints.Length)];
		obj.transform.rotation = Quaternion.identity;
		_spawnTime = UnityEngine.Random.Range (_minTimeSpawn, _maxTimeSpawn);
	}

	private void CreateSpawnPointArr ()
	{
		float tempDis = Vector3.Distance (_right.position, _left.position);
		int tempInt = (int)((tempDis) / _unitSize);
		
		_spawnPoints = new Vector3[tempInt];
		
		for (int i = 0; i < _spawnPoints.Length; i++) {
			_spawnPoints [i] = new Vector3 (_left.position.x + (i + 1) * _unitSize, 0, _left.position.z - 3 * _unitSize);
		}
	}

	private void DetermineBorders ()
	{
		_top.position = RaycastMethod (new Vector2 (Screen.width * 0.5f, Screen.height));
		_right.position = RaycastMethod (new Vector2 (0, Screen.height));
		_left.position = RaycastMethod (new Vector2 (Screen.width, Screen.height));
		_bottom.position = RaycastMethod (new Vector2 (Screen.width * 0.5f, 0));

		float temp = Vector3.Distance (_top.position, _bottom.position);
		_explosion.transform.position = new Vector3 (_bottom.position.x, 0, _bottom.position.z - 0.5f * temp);
	}

	private Vector3 RaycastMethod (Vector2 temp)
	{
		RaycastHit hit;

		Ray ray = _myCamera.ScreenPointToRay (temp);
		
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.tag == "Unit") {
				DieComponent dieComp = hit.collider.gameObject.GetComponent <DieComponent> ();
				dieComp.Die ();
				KillUnitHandler ();
			}

			if (hit.collider.tag == "Civil") {
				LoseGameHandler ();
			}

			return hit.point;
		}

		return Vector3.zero;
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

	private void HandleStartGame ()
	{
		_units.Add (_unit_1); 
		InvokeRepeating ("SpawnUnits", 2, _spawnTime);
	}

	private void HandleTimeIsOver ()
	{
		CancelInvoke ();
		StartCoroutine (IsAllEnemiesDead ());
	}

	private void HandleRestart ()
	{
		_lives = 2;
		_bombAmount = 3;
	}

	private void HandleExplosion ()
	{
		if (_bombAmount <= 0)
			return;

		_explosion.Play ();

		for (int i = 0; i < _enemies.Count; i++) {
			float temp = Vector3.Distance (_enemies [i].transform.position, _explosion.transform.position);
			if (temp < _explosionRadius && _enemies [i].tag == "Unit") {
				DieComponent die = _enemies [i].GetComponent <DieComponent> ();
				die.Die ();
			}
		}

		_bombAmount--;
	}

	private void LeveUpMethod ()
	{
		_level++;

		
		_minTimeSpawn -= _level * 0.1f;
		
		_minTimeSpawn = (_minTimeSpawn < 0.3f) ? 0.3f : _minTimeSpawn;
		
		_maxTimeSpawn -= _level * 0.1f;
		
		_maxTimeSpawn = (_maxTimeSpawn < 0.7f) ? 0.7f : _maxTimeSpawn;
		
		if (_level == 2)
			_units.Add (_unit_2);
		
		if (Level == 3)
			_units.Add (_unit_3);
	}

	private IEnumerator IsAllEnemiesDead ()
	{
		while (IsEnemiesDead () == false) {
			yield return null;
		}

		LevelUpHandler ();
	}

	private bool IsEnemiesDead ()
	{
		for (int i = 0; i < _enemies.Count; i++) {
			if (_enemies [i].activeInHierarchy)
				return false;
		}

		return true;
	}

	public void LoseLife ()
	{
		_lives--;

		if (_lives < 0)
			LoseGameHandler ();
		else
			LoseLifeEventHandler (_lives + 1);
	}

	private void LoseGameMethod ()
	{
		CancelInvoke ();
		for (int i = 0; i < _enemies.Count; i++) {
			DieComponent die = _enemies [i].GetComponent<DieComponent> ();
			die.TurnOff ();
		}
	}

	private void OnDrawGizmos ()
	{
		Gizmos.DrawWireSphere (_explosion.transform.position, _explosionRadius);
	}


	private void OnDestroy ()
	{
		CancelInvoke ();
		StopAllCoroutines ();

		if (_uiManager) {
			_uiManager.StartGame -= HandleStartGame;
			_uiManager.TimeIsOver -= HandleTimeIsOver;
			_uiManager.Restart -= HandleRestart;
			_uiManager.Explosion -= HandleExplosion;
		}

		LevelUp -= LeveUpMethod;
		LoseGame -= LoseGameMethod;
	}

}
