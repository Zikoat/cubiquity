using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	[Range(1,10)]
	public float speed = 4;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			Debug.DrawRay (hit.point, hit.normal * 2, Color.magenta);
			if (Input.GetMouseButton (0)) {
				Debug.Log (hit.normal);
			}
		}
		Debug.DrawRay (transform.position, transform.forward, Color.blue, 0, false);
	}

	void FixedUpdate() {
		
		Rigidbody rb = GetComponent<Rigidbody> ();

		//rb.AddRelativeForce(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"), ForceMode.VelocityChange);

		Vector3 vel = rb.velocity; // first we get the velocity // transform.InverseTransformDirection (rb.velocity);

		vel = transform.InverseTransformDirection (vel); // velocity is in world space, transform to local space
		// change the velocity
		vel.x = Input.GetAxis ("Horizontal") * speed;
		vel.z = Input.GetAxis ("Vertical") * speed; // Mathf.Clamp(vel.y, -5, 5)

		vel = transform.TransformDirection (vel); // change vel back to world space

		rb.velocity = vel;
		Debug.DrawRay(transform.position, vel);
		//transform.LookAt (Camera.main.transform);

	}
}