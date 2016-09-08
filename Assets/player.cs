using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
    [Range(1, 10)]
    public float speed = 4;
    [Range(1, 10)]
    public float jumpHeight = 7;

    // Use this for initialization
    void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnDrawGizmos()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.magenta);
            
            Vector3 nearestCube = new Vector3();
            nearestCube.x = Mathf.RoundToInt(hit.point.x);
            nearestCube.y = Mathf.RoundToInt(hit.point.y);
            nearestCube.z = Mathf.RoundToInt(hit.point.z);
            Gizmos.DrawSphere(nearestCube, 0.3f);
            
        }
    }

    // Update is called once per frame
    void Update () {
		
		Debug.DrawRay (transform.position, transform.forward, Color.blue, 0, false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            Debug.Log("jump");
        }
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