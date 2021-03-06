﻿using UnityEngine;
using System.Collections;

public class SoudnController : MonoBehaviour
{
	
	[SerializeField]
	private InputController
		_inputController;

	[SerializeField]
	private GameController
		_gameCntrl;

	[SerializeField]
	private UIManager
		_uiManager;

	[SerializeField]
	private AudioClip
		_killUnitSound;
	[SerializeField]
	private AudioClip
		_explosionSound;

	private AudioSource _audioSource;

	private void Awake ()
	{
		_audioSource = gameObject.GetComponent <AudioSource> ();

		#region NullCheck
		if (!_audioSource) {
			Debug.Log ("AuidoSource is null");
			return;
		}

		if (!_inputController) {
			Debug.Log ("InputController is null");
			return;
		}

		if (!_killUnitSound) {
			Debug.Log ("Kill sound is null");
			return;
		}

		if (!_explosionSound) {
			Debug.Log ("Explosion sound is null");
			return;
		}

		if (!_uiManager) {
			Debug.Log ("UIManager is null");
			return;
		}

		if (!_gameCntrl) {
			Debug.Log ("GameController is null");
			return;
		}
		#endregion

		_inputController.KillUnit += HandleKillUnit;
		_uiManager.Explosion += HandleExplosion;
	}

	private void HandleExplosion ()
	{
		if (_gameCntrl.BombAmount <= 0)
			return;

		_audioSource.clip = _explosionSound;
		_audioSource.Play ();
	}

	private void HandleKillUnit ()
	{
		_audioSource.clip = _killUnitSound;
		_audioSource.Play ();
	}

	private void OnDestroy ()
	{
		if (_inputController)
			_inputController.KillUnit -= HandleKillUnit;

		if (_uiManager)
			_uiManager.Explosion -= HandleExplosion;
	}
}
