using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	[Range(1,10)]
	public float speed = 4;
	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
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

        // declarations
        Transform camera = Camera.main.transform;
		Rigidbody rb = GetComponent<Rigidbody> ();

        // find up
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            Physics.gravity = -hit.normal.normalized * 10f;
        }
        // transform.up = hit.normal; // done with lookuprotation
        Debug.DrawRay(transform.position, transform.up, Color.blue, 0, false);


        // find forward
        Vector3 forward = (camera.position - transform.position);
        forward = transform.InverseTransformDirection(forward);
        forward.y = 0;
        forward = transform.TransformDirection(forward);
        // transform.forward = forward;
        Debug.DrawRay(transform.position, forward, Color.yellow);

        Quaternion rotation = new Quaternion();
        rotation.SetLookRotation(forward, hit.normal);
        //transform.rotation.SetLookRotation(forward, hit.normal);
        transform.rotation = rotation;
        

        // change velocity
        Vector3 vel = rb.velocity; // first we get the velocity
		vel = transform.InverseTransformDirection (vel); // velocity is in world space, transform to local space
		vel.x = Input.GetAxis ("Horizontal") * speed;
		vel.z = Input.GetAxis ("Vertical") * speed;
		vel = transform.TransformDirection (vel); // change vel back to world space
        rb.velocity = vel;
		Debug.DrawRay(transform.position, vel);



    }
}