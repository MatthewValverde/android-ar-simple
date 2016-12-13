using UnityEngine;
using System.Collections;

public class ARCameraRotation : MonoBehaviour
{
	public bool Gyro;
	public GUIStyle style;
	private Vector3 rot;
	// Use this for initialization
	void Start ()
	{
		Input.gyro.enabled = true;

		print ("localRotation: " + transform.localRotation);

		if (Gyro) {
			//   transform.rotation = Input.gyro.attitude;
			transform.localRotation = Input.gyro.attitude;

			/*rot.x = Input.gyro.gravity.z * -90;
	        rot.y += Input.gyro.rotationRate.y*-1;

            transform.localEulerAngles = rot;*/
		}
	}

	public void SetRotation ()
	{
		transform.localRotation = Input.gyro.attitude;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Gyro) {
			//   transform.rotation = Input.gyro.attitude;
	     
			///  if (Mathf.Abs(Input.gyro.rotationRateUnbiased.x) > 0.12f || Mathf.Abs(Input.gyro.rotationRateUnbiased.y) > 0.12f ||
			//   Mathf.Abs(Input.gyro.rotationRateUnbiased.z) > 0.12f)
			//  {
			transform.localRotation = Quaternion.LerpUnclamped (transform.localRotation, Input.gyro.attitude, 0.7f);

			//  }
	    
		} 
	        
		/*rot.x = Input.gyro.gravity.z * -90;
	        rot.y += Input.gyro.rotationRate.y*-1;

            transform.localEulerAngles = rot;*/
	    
		//transform.rotation = Input.gyro.attitude;
		//  transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
	}

	void OnGUI ()
	{
		return;
		GUILayout.Label ("Gyroscope attitude : " + Input.gyro.attitude, style);
		GUILayout.Label ("transform.localEulerAngles : " + transform.localEulerAngles, style);
		GUILayout.Label ("transform.localRotation : " + transform.localRotation, style);
		//GUILayout.Label ("Gyroscope rotationRate : " + Input.gyro.rotationRate, style);
		//GUILayout.Label ("Gyroscope rotationRateUnbiased : " + Input.gyro.rotationRateUnbiased, style);
		//GUILayout.Label ("Gyroscope updateInterval : " + Input.gyro.updateInterval, style);
		//GUILayout.Label ("Gyroscope userAcceleration : " + Input.gyro.userAcceleration, style);
	}
}
