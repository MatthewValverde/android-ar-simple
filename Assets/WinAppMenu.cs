using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player
{
	public string Name;
	public  string ID;

	public Player (string c_name, string c_id)
	{
		Name = c_name;
		ID = c_id;
	}
}

public class WinAppMenu : MonoBehaviour
{
	public List<Player> AllPlayers = new List<Player> ();
	public static WinAppMenu Instance;
	public GameObject homePanel, LogoPanel, WaitHitPanel, TrheeTwoOnePanel, GamePanel, ResoultPanel;

	public enum State
	{
		Home,
		Logo,
		WaitHitAgain,
		ThreeTwoOne,
		Game,
		Resoult
	}

	public InputField fieldTimer;
	public int TimerVal;
	public int CounterOne, CounterTwo;
	public Text timerInGame, Name1txt, Name2txt, Player1Init, Player2Init;
	public string Name1, Name2;
	public int Score1, Score2;
	public Text Coun321, Score1Text, Score2Text;
	public int count;
	public GameObject CountDown;

	public void AddPlayer (string Name, string DevID)
	{
		if (AllPlayers.Count == 2)
			return;

		Player tempPlayer = new Player (Name, DevID);

		print (Name);

		if (AllPlayers.Count == 0) {
			Name1 = Name;
			Player1Init.text = Name;
		} else {
			Name2 = Name;
			Player2Init.text = Name;
		}

		AllPlayers.Add (tempPlayer);

		/*if (AllPlayers.Count == 0) {
			AllPlayers.Add (tempPlayer);
			Name1 = Name;
		} else {
			int t = 0;
			for (int i = 0; i < AllPlayers.Count; i++) {
				if (tempPlayer.Name == AllPlayers [i].Name) {
					t++;
				}
			}
			if (t == 0) {
				AllPlayers.Add (tempPlayer);
				Name2 = Name;
			}
		}*/
	}

	public void AddScore (string Name, string DevID)
	{
		Player tempPlayer = new Player (Name, DevID);
		//for (int i = 0; i < AllPlayers.Count; i++) {
		if (Name == AllPlayers [0].Name) {
			Score1++;
			Score1Text.text = "" + Score1;
			PubNubComm.Instance.AddScoreCallback (Name, DevID, Score1.ToString ());
           
		}
		if (Name == AllPlayers [1].Name) {
			Score2++;
			Score2Text.text = "" + Score2;
			PubNubComm.Instance.AddScoreCallback (Name, DevID, Score2.ToString ());

		}
		//}
	}

	private State _appState;

	public State AppState {
		get { return _appState; }
		set {
			_appState = value;
			switch (value) {
			case State.Home:
				homePanel.SetActive (true);
				LogoPanel.SetActive (false);
                  
              
				GamePanel.SetActive (false);
				ResoultPanel.SetActive (false);
				break;
			case State.Logo:
				homePanel.SetActive (false);
				LogoPanel.SetActive (true);
              
                   // TrheeTwoOnePanel.SetActive(false);
				GamePanel.SetActive (false);
				ResoultPanel.SetActive (false);
				break;
			case State.WaitHitAgain:
				homePanel.SetActive (false);
				LogoPanel.SetActive (false);

				timerInGame.text = "" + TimerVal;
				GamePanel.SetActive (true);
				ResoultPanel.SetActive (false);

				Name1txt.text = Name1;
				Name2txt.text = Name2;
				break;
			case State.ThreeTwoOne:
				homePanel.SetActive (false);
				LogoPanel.SetActive (false);
				InvokeRepeating ("CountDowwn", 1, 1);
              
				GamePanel.SetActive (true);
				ResoultPanel.SetActive (false);
         
				break;
			case State.Game:
				homePanel.SetActive (false);
				LogoPanel.SetActive (false);
				InvokeRepeating ("TimerDown", 1, 1);
				GamePanel.SetActive (true);
				ResoultPanel.SetActive (false);
                   // timerInGame.text = "" + TimerVal;
				break;

			case State.Resoult:
				homePanel.SetActive (false);
				LogoPanel.SetActive (false);
              
				GamePanel.SetActive (false);
				ResoultPanel.SetActive (true);

				PubNubComm.Instance.SendResults (Name1, Score1.ToString ());
				break;
			}
		}
	}

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		AppState = State.Home;
	}

	public void onStartButton ()
	{
		TimerVal = 15;
		if (fieldTimer.text.Length > 0) {
			TimerVal = int.Parse (fieldTimer.text);
		}

		PubNubComm.Instance.UpdateTimer (TimerVal.ToString ());
		AppState = State.Logo;
	}
	// Update is called once per frame
	void Update ()
	{
		if (AppState == State.Home && Input.GetKeyDown (KeyCode.Return)) {
			onStartButton ();
		}
		if (AppState == State.Logo && Input.GetKeyDown (KeyCode.Return)) {
			AppState = State.WaitHitAgain;
		} else if (AppState == State.WaitHitAgain && Input.GetKeyDown (KeyCode.Return)) {
			CountDown.SetActive (true);
			AppState = State.ThreeTwoOne;
		}


	}

	public void TimerDown ()
	{
		TimerVal--;
		timerInGame.text = "" + TimerVal;
		PubNubComm.Instance.UpdateTimer (TimerVal.ToString ());
		if (TimerVal == 0) {
			CancelInvoke ("TimerDown");
			AppState = State.Resoult;
		}

	}

	public void CountDowwn ()
	{
		count--;
		Coun321.text = "" + count;
		PubNubComm.Instance.SendReady (count.ToString ());
		if (count == 0) {
			PubNubComm.Instance.SendGo ();			
			AppState = State.Game;
			CountDown.SetActive (false);
			CancelInvoke ("CountDowwn");
		}
	}
}
