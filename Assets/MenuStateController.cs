using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuStateController : MonoBehaviour
{
	public static MenuStateController Instance;
	public GameObject StartPanel, PlayerNamePanel, GamePanel, ResultPanel, SwipeField, ScoresObj, TimerObj, ArCamera, LerpedObject;
	public int ScoresInt;
	public int StartTimer;
	private int _startTimer;
	public Text Scores, Name, Timer, ReadyText, ScoresText2;
	public InputField field;
	public GameObject[] ArGameObjects;
	public String P_Name;
	public float distance;
	public ARCameraRotation arCameraRotation;
	public bool swipeIsEnabled;

	public enum State
	{
		Start,
		PlayerName,
		Game,
		Result
	}

	private State _appState;

	public State AppState { 
		get { return _appState; }
		set {
			_appState = value;
			switch (value) {
			case State.Start:
				//StartPanel.SetActive (true);
				//PlayerNamePanel.SetActive (false);
				//GamePanel.SetActive (false);
				//ResultPanel.SetActive (false);
				break;
			case State.PlayerName:
				//StartPanel.SetActive (false);
				//PlayerNamePanel.SetActive (true);
				//GamePanel.SetActive (false);
				//ResultPanel.SetActive (false);
				break;
			case State.Game:
				//StartPanel.SetActive (false);
				//PlayerNamePanel.SetActive (false);
				//GamePanel.SetActive (true);
				//ResultPanel.SetActive (false);
		          /*  foreach (var obj in ArGameObjects)
		            {
		                obj.SetActive(true);
		            }*/
				//if (StartTimer == 0) {
					//SetTimerVal (48);
				//}
				//print ("WORK---------------------------------------------------");
				break;
			case State.Result:

				//LerpedObject.SetActive (false);
				//StartPanel.SetActive (false);
				//PlayerNamePanel.SetActive (false);
				//GamePanel.SetActive (false);
				//ResultPanel.SetActive (true);
				break;
			}
		}
	}
	// Use this for initialization
	void Awake ()
	{
		Instance = this;
	  
	}

	public void EnableSwipe ()
	{
		swipeIsEnabled = true;
		SwipeField.SetActive (true);
	}

	public void SetReady (string value)
	{
		int valueInt = int.Parse (value);
		valueInt++;
		ReadyText.text = valueInt.ToString ();
	}

	public void SetGo ()
	{
		ReadyText.enabled = false;
		EnableSwipe ();
	}

	public void SetScore (string name, string value)
	{
		if (name == Name.text) {
			ScoresInt = int.Parse (value);
			Scores.text = "" + ScoresInt;
		}
	}

	public void SetScore (string value)
	{
		ScoresInt = int.Parse (value);
		Scores.text = "" + ScoresInt;
	}

	public void SetResults (string name, string score)
	{
		AppState = State.Result;
	}

	public void SetTimerVal (int timer)
	{
		print ("SetTimerVal: " + timer);
		_startTimer = timer;
		StartTimer = _startTimer;
		Timer.text = "" + StartTimer;
	}

	public void TimerDown ()
	{
		if (StartTimer != 0) {
			StartTimer--;
			Timer.text = "" + StartTimer;
		} else {
			//StartTimer = _startTimer;
			AppState = State.Start;
			CancelInvoke ("TimerDown");
		}
	}

	public void Start ()
	{
		//LerpedObject.transform.position = ArCamera.transform.position + ArCamera.transform.forward * distance;
		//LerpedObject.transform.rotation = Quaternion.Euler(0, ArCamera.transform.localEulerAngles.y, 0);

	}

	public void AddScore ()
	{
		ScoresInt++;
		Scores.text = "" + ScoresInt;
	}

	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnEndCalibrate ()
	{
		//SetTimerVal (20);
		//arCameraRotation.SetRotation();
		ScoresObj.SetActive (true);
		//ScoresText2.text="";
		//TimerObj.SetActive (true);
		//ReadyText.text = "GET READY!";
		//TimerObj.SetActive (true);
		LerpedObject.SetActive (true);
		LerpedObject.transform.position = ArCamera.transform.position + ArCamera.transform.forward * distance;
		LerpedObject.transform.rotation = ArCamera.transform.rotation;
	}

	public void OnPlayButtonClick ()
	{
		AppState = State.PlayerName;
	}

	public void OnApplyButtonClick ()
	{
		Name.text = field.text;
		P_Name = field.text;
		PubNubComm.Instance.SendName (field.text);
		AppState = State.Game;
	}
}
