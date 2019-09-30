using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	public void startGame ()
	{
		SceneManager.LoadScene ("Level 1");
	}

	public void highscore ()
	{
		SceneManager.LoadScene ("Highscore");
	}

	public void quit ()
	{
		// to delete all prefs just uncomment code bellow and delete on closing game
		//PlayerPrefs.DeleteAll ();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
