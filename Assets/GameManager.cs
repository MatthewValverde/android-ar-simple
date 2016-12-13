using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class GameManager : MonoBehaviour
{
	public float puckSpeed = .5f;
	public GameObject puck;
	public GameObject player;
	public Animator playerAnimator;
	public Collider puckCollider;
	public Transform puckOriginalTransform;
	private bool mIsShootingPuck = false;
	private PuckController mPuckController;
	private GameObject mPuck;
	public GameObject puckParent;
	public GameObject goal;

	public GameObject cube;
	public GameObject mainContainer;

	public float puckInFrontBy = .5f;
	public MenuStateController menuStateController;
	public GameObject explosion;
	private GameObject mExplosion;

	public GameObject sparks;

	void Start ()
	{
		InitPuck ();
	}

	void InitPuck ()
	{
		mPuck = GameObject.Instantiate (puck, puckOriginalTransform) as GameObject;
		mPuck.transform.parent = puckParent.transform;
		mPuckController = mPuck.GetComponent<PuckController> ();
		mPuck.transform.localPosition = puckOriginalTransform.localPosition + new Vector3 (0f, 0f, puckInFrontBy);

		puckOriginalTransform.position = player.transform.position;

		cube.transform.localPosition = puckOriginalTransform.localPosition + new Vector3 (8f, 0f, puckInFrontBy);
		mPuck.SetActive (true);
	}

	float puckMover = 0f;

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {

			PlayAnimation ("StartShot");
			Debug.Log ("Pressed left click.");
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			print ("you got it dude");
			PlayAnimation ("StartShot");
		}

		if (mIsShootingPuck) {
			puckMover += puckSpeed;
			mPuck.transform.position = Vector3.MoveTowards (mPuck.transform.position, cube.transform.position, 20 * Time.deltaTime);
			if (mPuck.transform.position == cube.transform.position) {
				mIsShootingPuck = false;
			}
		} else {
			mPuck.transform.localPosition = puckOriginalTransform.localPosition + new Vector3 (0f, 0f, puckInFrontBy);

			puckOriginalTransform.position = player.transform.position;

			cube.transform.localPosition = puckOriginalTransform.localPosition + new Vector3 (8f, 0f, puckInFrontBy);

			puckMover = 0f;
		}

	}

	void LateUpdate ()
	{
		if (mPuckController.hasCollision) {
			mPuckController.hasCollision = false;
			MarkGoal ();
		}

		if (mPuckController.hasMissedCollision) {
			mPuckController.hasMissedCollision = false;
			MarkMiss ();
		}
	}

	private void PlayAnimation (string name)
	{
		if (playerAnimator == null) {
			print ("YOU NEED AN ANIMATOR");
			return;
		}

		playerAnimator.Play (name);
	}

	public void ShootPuck ()
	{
		print ("SHOOT PUCK");
		mIsShootingPuck = true;
	}

	public void MarkGoal ()
	{
//		PubNubComm.Instance.SendGoal (MenuStateController.Instance.Name.text);
		CreateExplosion ();
		print ("MarkGoal");
		mIsShootingPuck = false;
		menuStateController.AddScore ();
		Invoke ("Check", 1f);
	}

	private void CreateExplosion ()
	{
		mExplosion = GameObject.Instantiate (explosion, goal.transform.position, Quaternion.identity) as GameObject;
		mExplosion.layer = 9;

		GameObject sparksExplode = GameObject.Instantiate (sparks, goal.transform.position, Quaternion.identity) as GameObject;
		sparks.layer = 9;


		//mExplode = GameObject.Instantiate (explode, goal.transform.position, Quaternion.identity) as GameObject;
		//mExplode.layer = 9;

		//Transform explodingObject = (Transform) Instantiate (mExplode.transform, goal.transform.position, Quaternion.identity) as Transform;
		//ParticleSystemMultiplier particleMultiplier = mExplode.GetComponent<ParticleSystemMultiplier>();

		//particleMultiplier.multiplier = 1;
		//ParticleSystem ps = mExplode.GetComponent<ParticleSystem>();
	//ps.Play();

		//mExplode = (Transform) Instantiate(explode.transform, goal.transform.position, Quaternion.identity);
		//mExplode.layer = 9;
		//ParticleSystemMultiplier particleMultiplier = mExplode.transform.GetComponent<ParticleSystemMultiplier>();

	//	m_Instance.GetComponent<ParticleSystemMultiplier>().multiplier = 1;
	//	particleMultiplier.

		//mExplosion.transform.parent = puckParent.transform;

		//ParticleSystem cool = mExplosion.GetComponent<ParticleSystem>();
	}

	public void MarkMiss ()
	{
		print ("MarkMiss");
		mIsShootingPuck = false;
		Invoke ("Check", 1f);
	}

	private void Check ()
	{
		//CreateExplosion();
		//DestroyImmediate (explosion);
		//ParticleSystem cool = mExplosion.GetComponent<ParticleSystem>();
		//cool.Stop();
		if (mExplosion != null) {
			Destroy (mExplosion);
			mExplosion = null;
		}
		Destroy (mPuck);
		mPuck = null;
		InitPuck ();
	}
}