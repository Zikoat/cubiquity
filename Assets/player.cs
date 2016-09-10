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
	}

	void FixedUpdate() {

        // declarations
        Transform camera = Camera.main.transform;
		Rigidbody rb = GetComponent<Rigidbody> ();

        

        // find forward
        Vector3 forward = (transform.position - camera.position);
        Debug.DrawRay(transform.position, forward, Color.yellow);
        Debug.DrawRay(transform.position, transform.localEulerAngles, Color.yellow);
        /*forward = transform.InverseTransformDirection(forward);
        forward.y = 0;
        forward = transform.TransformDirection(forward);
        // transform.forward = forward;

        // Quaternion rotation = new Quaternion();
        // rotation.SetLookRotation(forward, hit.normal);
        // transform.localRotation = rotation;*/


        // change velocity
        Vector3 vel = rb.velocity; // first we get the velocity
		vel = transform.InverseTransformDirection (vel); // velocity is in world space, transform to local space
		vel.x = Input.GetAxis ("Horizontal") * speed;
		vel.z = Input.GetAxis ("Vertical") * speed;
		vel = transform.TransformDirection (vel); // change vel back to world space
        rb.velocity = vel;
		Debug.DrawRay(transform.position, vel);

        // find up
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        Debug.DrawRay(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            Physics.gravity = -hit.normal.normalized * 10f;
        }
        transform.up = hit.normal; 

        Debug.DrawRay(transform.position, -transform.up, Color.blue, 0, false);
        Debug.DrawRay(transform.position, transform.forward, Color.blue, 0, false);



    }
}