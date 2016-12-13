using System;
using UnityEngine;
using PubNubMessaging.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class PubNubComm : MonoBehaviour
{
	public bool Server;
	public static PubNubComm Instance;

	private Pubnub mPubnubInitialize;
	private Pubnub mPubnubFromClient;
	private Pubnub mPubnubToClient;

	private string mShipRaceInitializeChannel = "shipRace_initialize_test";
	private string mShipRaceFromClientChannel = "shipRace_fromClient_test";
	private string mShipRaceToClientChannel = "shipRace_toClient_test";

	private string mPubId = "pub-c-9aeaf337-cd42-4b9b-a444-453c7dbc5f52";
	private string mSubId = "sub-c-9ca7a4ca-8d5e-11e6-9125-02ee2ddab7fe";

	private string mDeviceId;
	private string mPlayerId;

	public string Name1, Name2;

	private float NeededDistance;
	public Transform ARObjectPosition;
	private float OffsetY;
	private Vector3 StartPosRed;
	Vector3 StartPosPewter, SpartPosWhite;
	public int Connected;

	//  private PubNubModel mPubNubModel;
	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		mDeviceId = Guid.NewGuid ().ToString (); // Get the device ID.
		mPlayerId = mDeviceId;

		StartPubnub ();
		//S  NeededDistance = Vector3.Distance(StartPosition.position, FinishPosition.position);
		// Invoke("GetStartPosiitons",0.1f);
	}

	private float amountToMove = .5f;

	void Update ()
	{

	}

	private void StartPubnub ()
	{
		mPubnubInitialize = new Pubnub (mPubId, mSubId);
		mPubnubInitialize.Subscribe<string> (
			mShipRaceInitializeChannel + "," + mShipRaceFromClientChannel,
			PubnubFromClientSubscribeReturnMessage,
			StartSubscribeConnectStatusMessage,
			StartErrorMessage);
		return;


		mPubnubFromClient = new Pubnub (mPubId, mSubId);
		mPubnubFromClient.Subscribe<string> (
			mShipRaceFromClientChannel,
			PubnubFromClientSubscribeReturnMessage,
			PubnubFromClientSubscribeConnectStatusMessage,
			PubnubFromClientErrorMessage);

		mPubnubToClient = new Pubnub (mPubId, mSubId);
		mPubnubToClient.Subscribe<string> (
			mShipRaceToClientChannel,
			PubnubToClientSubscribeReturnMessage,
			PubnubToClientSubscribeConnectStatusMessage,
			PubnubToClientErrorMessage);
	}

	private void StopPubnub ()
	{
		return;
		mPubnubInitialize = new Pubnub (mPubId, mSubId);
		mPubnubInitialize.Unsubscribe<string> (
			mShipRaceInitializeChannel,
			StartReturnMessage,
			StartSubscribeReturnMessage,
			StartSubscribeConnectStatusMessage,
			StartErrorMessage);

		mPubnubFromClient = new Pubnub (mPubId, mSubId);
		mPubnubFromClient.Unsubscribe<string> (
			mShipRaceFromClientChannel,
			PubnubFromClientReturnMessage,
			PubnubFromClientSubscribeReturnMessage,
			PubnubFromClientSubscribeConnectStatusMessage,
			PubnubFromClientErrorMessage);

		mPubnubToClient = new Pubnub (mPubId, mSubId);
		mPubnubToClient.Unsubscribe<string> (
			mShipRaceToClientChannel,
			PubnubToClientReturnMessage,
			PubnubToClientSubscribeReturnMessage,
			PubnubToClientSubscribeConnectStatusMessage,
			PubnubToClientErrorMessage);
	}

	void OnDisable ()
	{
		StopPubnub ();
	}

	public void SendMsgToInitializeChannel (object message)
	{
		mPubnubInitialize.Publish<string> (
			mShipRaceInitializeChannel,
			message,
			StartReturnMessage,
			StartErrorMessage);
	}

	private void StartSubscribeConnectStatusMessage (string connectMessage)
	{
		var connectModel = new PubNubConnectModel ();
		connectModel.event_type = "device_connect";
		connectModel.device_id = mDeviceId; //?
		string message = mPubnubInitialize.JsonPluggableLibrary.SerializeToJsonString (connectModel);
		SendMsgToInitializeChannel (connectModel);
		//	Debug.Log("device_connect test");
	}

	private void StartSubscribeReturnMessage (string result)
	{
		UnityEngine.Debug.Log (result);
		if (!string.IsNullOrEmpty (result) && !string.IsNullOrEmpty (result.Trim ())) {
			List<object> deserializedMessage = mPubnubInitialize.JsonPluggableLibrary.DeserializeToListOfObject (result);
			if (deserializedMessage != null && deserializedMessage.Count > 0) {
				object subscribedObject = (object)deserializedMessage [0];
				if (subscribedObject != null) {
					string resultActualMessage = mPubnubInitialize.JsonPluggableLibrary.SerializeToJsonString (subscribedObject);

					var connectModel = new PubNubConnectModel ();

					JsonUtility.FromJsonOverwrite (subscribedObject.ToString (), connectModel);
					if (!connectModel.event_type.Equals ("device_connect"))
						return;
					//	Debug.Log(connectModel.device_id);
				}
			}
		}
	}

	private void StartErrorMessage (PubnubClientError pubnubError)
	{
	}

	private void StartReturnMessage (string result)
	{
		//UnityEngine.Debug.Log (result);
	}

	public  void RegisterFanSet (string team_id)
	{
		var fanSetModel = new PubNubFanSetModel ();
		fanSetModel.event_type = "fanset";
		fanSetModel.device_id = mDeviceId; //?
		fanSetModel.team_id = team_id;
		SendMsgTo_FromClientChannel (fanSetModel);
	}

	public  void AddScoreCallback (string name, string devID, string score)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "add_score_callback";
		powerModel.device_id = devID; //?
		powerModel.player_id = score;
		powerModel.team_id = "";
		powerModel.name = name;
		SendMsgTo_FromClientChannel (powerModel);
	}

	public  void SendResults (string name, string score)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "results";
		powerModel.device_id = ""; //?
		powerModel.player_id = score;
		powerModel.team_id = "";
		powerModel.name = name;
		SendMsgTo_FromClientChannel (powerModel);
	}

	public void SendName (string thisname)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "name_set";
		powerModel.device_id = mDeviceId; //?
		powerModel.player_id = mPlayerId;
		powerModel.team_id = "shipRace_team_1";
		powerModel.name = thisname;
		SendMsgTo_FromClientChannel (powerModel);
	}

	public void UpdateTimer (string time)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "update_timer";
		powerModel.device_id = mDeviceId; //?
		powerModel.player_id = mPlayerId;
		powerModel.team_id = "shipRace_team_1";
		powerModel.name = time;
		SendMsgTo_FromClientChannel (powerModel);
	}

	public void SendGoal (string thisname)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "goal";
		powerModel.device_id = mDeviceId; //?
		powerModel.player_id = mPlayerId;
		powerModel.team_id = "shipRace_team_1";
		powerModel.name = thisname;
		SendMsgTo_FromClientChannel (powerModel);

	}

	public void SendReady (string count)
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "ready";
		powerModel.device_id = mDeviceId; //?
		powerModel.player_id = mPlayerId;
		powerModel.team_id = "shipRace_team_1";
		powerModel.name = count;
		SendMsgTo_FromClientChannel (powerModel);

	}

	public void SendGo ()
	{
		var powerModel = new PubNubPowerModel ();
		powerModel.event_type = "go";
		powerModel.device_id = mDeviceId; 
		SendMsgTo_FromClientChannel (powerModel);

	}

	public void SendMsgTo_FromClientChannel (object message)
	{
		mPubnubInitialize.Publish<string> (
			mShipRaceFromClientChannel,
			message,
			PubnubFromClientReturnMessage,
			PubnubFromClientErrorMessage);
	}

	private void PubnubFromClientSubscribeConnectStatusMessage (string connectMessage)
	{
		UnityEngine.Debug.Log ("BROWN SUBSCRIBE CONNECT CALLBACK");
	}

	private void PubnubFromClientSubscribeReturnMessage (string result)
	{
		Debug.Log ("PubnubFromClientSubscribeReturnMessage: " + result);


		if (!string.IsNullOrEmpty (result) && !string.IsNullOrEmpty (result.Trim ())) {
			List<object> deserializedMessage = mPubnubInitialize.JsonPluggableLibrary.DeserializeToListOfObject (result);

			if (deserializedMessage != null && deserializedMessage.Count > 0) {
				object subscribedObject = (object)deserializedMessage [0];
				if (subscribedObject != null) {
					//IF CUSTOM OBJECT IS EXCEPTED, YOU CAN CAST THIS OBJECT TO YOUR CUSTOM CLASS TYPE
					string resultActualMessage = mPubnubInitialize.JsonPluggableLibrary.SerializeToJsonString (subscribedObject);
					Debug.Log (resultActualMessage);
					PubNubPowerModel powerModel = new PubNubPowerModel ();

					JsonUtility.FromJsonOverwrite (resultActualMessage, powerModel);
					// Debug.Log(powerModel.team_id);
					if (powerModel.event_type.Equals ("name_set") && Server) {
						WinAppMenu.Instance.AddPlayer (powerModel.name, powerModel.device_id);
					} else if (powerModel.event_type.Equals ("goal") && Server) {
						WinAppMenu.Instance.AddScore (powerModel.name, powerModel.device_id);
					} else if (powerModel.event_type.Equals ("update_timer") && !Server) {
						MenuStateController.Instance.SetTimerVal (int.Parse (powerModel.name));
					} else if (powerModel.event_type.Equals ("ready") && !Server) {
						MenuStateController.Instance.SetReady (powerModel.name);
					} else if (powerModel.event_type.Equals ("go") && !Server) {
						MenuStateController.Instance.SetGo ();
					} else if (powerModel.event_type.Equals ("add_score_callback") && !Server) {
						MenuStateController.Instance.SetScore (powerModel.name, powerModel.player_id);
					} else if (powerModel.event_type.Equals ("results") && !Server) {
						MenuStateController.Instance.SetResults (powerModel.name, powerModel.player_id);
					}

				}
			}
		}
	}

	private void PubnubFromClientErrorMessage (PubnubClientError pubnubError)
	{
	}

	private void PubnubFromClientReturnMessage (string result)
	{
		//   Debug.Log(result);
	}

	public void SendMsgToClientChannel (string message)
	{
		mPubnubToClient.Publish<string> (
			mShipRaceToClientChannel,
			message,
			PubnubToClientReturnMessage,
			PubnubToClientErrorMessage);
		Debug.Log ("ToClient");
	}

	private void PubnubToClientSubscribeConnectStatusMessage (string connectMessage)
	{
		mPubnubToClient.Publish<string> (
			mShipRaceToClientChannel,
			connectMessage,
			PubnubToClientReturnMessage,
			PubnubToClientErrorMessage);
	}

	private void PubnubToClientSubscribeReturnMessage (string result)
	{
		UnityEngine.Debug.Log (result);
		if (!string.IsNullOrEmpty (result) && !string.IsNullOrEmpty (result.Trim ())) {
			List<object> deserializedMessage = mPubnubToClient.JsonPluggableLibrary.DeserializeToListOfObject (result);
			if (deserializedMessage != null && deserializedMessage.Count > 0) {
				object subscribedObject = (object)deserializedMessage [0];
				if (subscribedObject != null) {
					string resultActualMessage = mPubnubInitialize.JsonPluggableLibrary.SerializeToJsonString (subscribedObject);
					var moveModel = new PubNubMoveModel ();
					var powerModel = new PubNubPowerModel ();
					JsonUtility.FromJsonOverwrite (resultActualMessage, powerModel);
					JsonUtility.FromJsonOverwrite (resultActualMessage, moveModel);
					UnityEngine.Debug.Log (moveModel.event_type + "work!!!");

					if (!powerModel.event_type.Equals ("name_set"))
						return;


					if (powerModel.team_id.Equals ("shipRace_team_1")) {

						Debug.Log (powerModel.name);
					}

					if (moveModel.team_id.Equals ("shipRace_team_2")) {


					}

					if (moveModel.team_id.Equals ("shipRace_team_3")) {


					}
				}
			}
		}
	}

	private void PubnubToClientErrorMessage (PubnubClientError pubnubError)
	{
		UnityEngine.Debug.Log ("BROWN DisplayErrorMessage:" + pubnubError.StatusCode);
	}

	private void PubnubToClientReturnMessage (string result)
	{
		//UnityEngine.Debug.Log ("BrownDisplayReturnMessage STATUS CALLBACK");
		// UnityEngine.Debug.Log (result);
	}
}
