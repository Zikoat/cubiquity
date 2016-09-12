using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
    [Range(1, 10)]
    public float speed = 4;
    [Range(1, 10)]
    public float jumpHeight = 7;

    public bool raytrace = true;
    

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
 
+
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

        if (raytrace)
        {
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
}