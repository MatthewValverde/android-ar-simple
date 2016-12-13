using UnityEngine;
using System.Collections;

public class LerpTarget : MonoBehaviour
{
    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	target.rotation = Quaternion.Lerp(target.rotation,transform.rotation,0.5f);
	}
}
