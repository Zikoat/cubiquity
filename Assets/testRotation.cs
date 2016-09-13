using UnityEngine;
using System.Collections;

public class testRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 relativePos = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);

        transform.rotation = rotation;

        Debug.DrawRay(transform.position, transform.forward, Color.blue, 0, false);
        Debug.DrawRay(transform.position, -transform.up, Color.blue, 0, false);


    }
}
