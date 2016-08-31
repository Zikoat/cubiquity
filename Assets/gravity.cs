using UnityEngine;
using System.Collections;

public class gravity : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		if (Physics.Raycast (ray, out hit)) {
			Physics.gravity = -hit.normal.normalized * 10f;
		}
		transform.up = hit.normal;
		Debug.DrawRay (transform.position, transform.up, Color.blue, 0, false);
	}
}
