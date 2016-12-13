using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerBehavior : MonoBehaviour
{
    public Rigidbody Puck;
    public Vector3 puckDefaultPosition;
    public Quaternion puckDDefRotation;
    public Animator Anim;
    public GameObject TrackableObj;
    // Use this for initialization
    void Start()
    {
        puckDefaultPosition = Puck.transform.position;
        puckDDefRotation = Puck.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPointDown()
    {
        {
            Anim.SetTrigger("PDown");
            Anim.SetBool("PUp", false);
            Puck.velocity = Vector3.zero;
            Puck.transform.position = puckDefaultPosition;
            Puck.transform.rotation = puckDDefRotation;
        }
    }

    public void SetPointUp()
    {
      //if (TrackableObj.activeSelf)
      //  {
            Anim.SetBool("PUp", true);
            Anim.ResetTrigger("PDown");
            Anim.SetFloat("Swipe", 0);
        //}
    }

    public void Swipe(BaseEventData bed)
    {
     //   if (TrackableObj.activeSelf)
       // {
            PointerEventData ped = bed as PointerEventData;
            Anim.SetFloat("Swipe", Mathf.Clamp(ped.delta.y*0.005f, 0, 1));
       // }
        //  Debug.Log(ped.delta);
    }

    public void Shot()
    {
        // Debug.Log("Shot");
        //  if(gameObject.activeSelf)
       
        Puck.AddForce(transform.right * 200);
    }

}
