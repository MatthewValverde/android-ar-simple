using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown (0)) {
			LoadNewScene ();
		}
	}

	private void LoadNewScene ()
	{
		SceneManager.LoadScene ("MainScene");
	}
}
