using UnityEngine;
using System.Collections;

public class PuckController : MonoBehaviour
{
	//public GameObject gameManagerScript;
	//private GameManager mGameManager;
	// Use this for initialization

	public bool hasCollision = false;
	public bool hasMissedCollision = false;

	void Start ()
	{
		//mGameManager = gameManagerScript.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider oth)
	{
		//if (oth.tag == "Line") {
			//  MenuStateController.Instance.AddScore();
		//	PubNubComm.Instance.SendGoal (MenuStateController.Instance.Name.text);
	//	}

		if (oth.name == "BackOfGoal") {
			hasCollision = true;
			//mGameManager.MarkGoal ();
		}

		if (oth.name == "MissedGoalCollider") {

			//print();

			hasMissedCollision = true;
			//mGameManager.MarkGoal ();
		}

		print ("TRIGGER EVENT: " + oth.name);
	}

	/*void OnCollisionEnter (Collision info)
	{
		if (info.collider.name == "BackOfGoal") {
			mGameManager.MarkGoal ();
		}
	}*/
}