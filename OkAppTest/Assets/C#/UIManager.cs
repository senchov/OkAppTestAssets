using UnityEngine;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{

	#region Fields
	[SerializeField]
	private GameObject
		_startView;

	[SerializeField]
	private GameObject
		_gameView;

	[SerializeField]
	private GameObject
		_level2View;

	[SerializeField]
	private GameObject
		_level3View;

	[SerializeField]
	private GameObject
		_wellDoneView;

	[SerializeField]
	private GameObject
		_loseView;

	[SerializeField]
	private GameObject[]
		_hearts;

	[SerializeField]
	private UISlider
		_timeSlider;

	[SerializeField]
	private GameController
		_gameCntrl;

	[SerializeField]
	private UILabel
		_bombAmounText;

	private bool _isTime = false;
	#endregion

	#region Events
	public event Action StartGame;
	private void StartGameHandler ()
	{
		if (StartGame != null)
			StartGame ();
	}

	public event Action TimeIsOver;
	private void TimeIsOverHandler ()
	{
		if (TimeIsOver != null)
			TimeIsOver ();
	}

	public event Action Restart;
	private void RestartHandler ()
	{
		if (Restart != null)
			Restart ();
	}

	public event Action Explosion;

	private void ExplosionHandler ()
	{
		if (Explosion != null)
			Explosion ();
	}

	#endregion

	private void Awake ()
	{
		#region NullCheck
		if (!_startView) {
			Debug.Log ("StartView is null");
			return;
		}

		if (!_gameView) {
			Debug.Log ("GameView is null");
			return;
		}

		if (!_timeSlider) {
			Debug.Log ("Slider is null");
			return;
		}

		if (!_gameCntrl) {
			Debug.Log ("GameController is null");
			return;
		}

		if (!_level2View) {
			Debug.Log ("Level 2 view is null");
			return;
		}

		if (!_level3View) {
			Debug.Log ("Level 3 view is null");
			return;
		}

		if (!_wellDoneView) {
			Debug.Log ("WellDoneView is null");
			return;
		}

		if (!_loseView) {
			Debug.Log ("LoseView is null");
			return;
		}

		if (!_bombAmounText) {
			Debug.Log ("Bomb label is null");
			return;
		}
		#endregion

		_startView.SetActive (true);
		_gameView.SetActive (false);
		_level2View.SetActive (false);
		_level3View.SetActive (false);
		_wellDoneView.SetActive (false);
		_loseView.SetActive (false);

		_gameCntrl.LevelUp += ChoseView;
		_gameCntrl.LoseGame += HandleLoseGame;
		_gameCntrl.LoseLifeEvent += HandleLoseLifeEvent;
	}

	private void Update ()
	{
		_bombAmounText.text = _gameCntrl.BombAmount.ToString ();
	}

	private void HandleLoseLifeEvent (int obj)
	{
		_hearts [obj].SetActive (false);
	}

	private void HandleLoseGame ()
	{
		StopAllCoroutines ();
		_gameView.SetActive (false);
		_loseView.SetActive (true);
	}


	private IEnumerator TimeSlide ()
	{
		float temp = Time.time + 60;
		while (_timeSlider.value > 0) {
			_timeSlider.value = (temp - Time.time) / 60;
		
			yield return null;
		}

		TimeIsOverHandler ();
	}

	private void ChoseView ()
	{
		_gameView.SetActive (false);

		print (_gameCntrl.Level);
		if (_gameCntrl.Level == 2)
			_level2View.SetActive (true);

		if (_gameCntrl.Level == 3)
			_level3View.SetActive (true);
	
		if (_gameCntrl.Level > 3)
			_wellDoneView.SetActive (true);
	}





	public void StartButtonPress ()
	{
		TurnOffWindow (_startView);
	}

	public void Level2ButtonPress ()
	{
		TurnOffWindow (_level2View);
	}

	public void Level3ButtonPress ()
	{
		TurnOffWindow (_level3View);
	}

	public void WellDoneButtonPress ()
	{
		TurnOffWindow (_wellDoneView);
	}

	public void RestartButtonPress ()
	{
		RestartHandler ();
		TurnOffWindow (_loseView);
		for (int i = 0; i < _hearts.Length; i++) {
			_hearts [i].SetActive (true);
		}

	}

	public void BombButtonPress ()
	{
		ExplosionHandler ();
	}

	private void TurnOffWindow (GameObject obj)
	{
		obj.SetActive (false);
		_gameView.SetActive (true);

		_timeSlider.value = 1;
		StartGameHandler ();
		StartCoroutine (TimeSlide ());
	}

	private void OmDestroy ()
	{
		if (!_gameCntrl) {
			Debug.Log (" is null");
			return;
		}

		_gameCntrl.LevelUp -= ChoseView;
		_gameCntrl.LoseGame -= HandleLoseGame;
		_gameCntrl.LoseLifeEvent -= HandleLoseLifeEvent;
	}

}
