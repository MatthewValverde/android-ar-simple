using UnityEngine;
using System.Collections;

public class PlayerAnimationEvents : MonoBehaviour
{

	public GameObject gameManagerScript;
	private GameManager mGameManager;
	// Use this for initialization
	void Start ()
	{
		mGameManager = gameManagerScript.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void ShootPuck ()
	{
		print ("SHOOT PUCK PlayerAnimationEvents");
		mGameManager.ShootPuck();
	}

}
