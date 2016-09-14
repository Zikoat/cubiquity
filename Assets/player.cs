using UnityEngine;
using System.Collections;
using Cubiquity;


public class player : MonoBehaviour {
    [Range(1, 10)]
    public float speed = 4;
    [Range(1, 10)]
    public float jumpHeight = 7;

    public bool raytrace = true;
    public int gravityRadius = 9;
    public int gravityCheckRadius = 10;
    [Range(0, 20)]
    public float gravityStrength = 9.89f;



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
        /*forward = transform.InverseTransformDirection(forward); */
        //forward. = 0; 
        //forward = transform.TransformDirection(forward); 
        // transform.forward = forward; 
 /*
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

        }

        // transform.localEulerAngles = new Vector3(10, yRotation * 90, 10);
        transform.Rotate(Vector3.right, Time.deltaTime);
        Debug.DrawRay(transform.position, transform.forward, Color.blue, 0, false);
        Debug.DrawRay(transform.position, -transform.up, Color.blue, 0, false);


        // all of localGravity sets transform.up and Physics.gravity
        // i could move the transform.up allocation to update instead of FixedUpdate,
        // to avoid getting jagged movement, although this may interfere with physics,
        // or have no impact on physics.
        bool localGravity = !gameObject.GetComponent<player>().raytrace;
        if (localGravity)
        {
            int positionx = Mathf.RoundToInt(transform.position.x);
            int positiony = Mathf.RoundToInt(transform.position.y);
            int positionz = Mathf.RoundToInt(transform.position.z);
            Vector3 average = new Vector3();
            int count = 0;

            TerrainVolume volume = GameObject.FindGameObjectWithTag("terrain").GetComponent<TerrainVolume>();

            for (int x = positionx - gravityCheckRadius; x < positionx + gravityCheckRadius; x++)
                for (int y = positiony - gravityCheckRadius; y < positiony + gravityCheckRadius; y++)
                    for (int z = positionz - gravityCheckRadius; z < positionz + gravityCheckRadius; z++)
                    {
                        int value = volume.data.GetVoxel(x, y, z).weights[2];
                        Vector3 pos = new Vector3(x, y, z);
                        if (Vector3.SqrMagnitude(transform.position - pos) < gravityRadius && value > 127)
                        {
                            Debug.DrawLine(transform.position, pos, new Color(1f, 0, 0, 0.3f));
                            average += pos - transform.position;
                            count++;
                        }
                    }
            average /= count;
            Debug.DrawRay(transform.position, average, Color.green);
            Vector3 up = -average.normalized;
            //--------------------------------------------------------------------------------
            transform.up = up;
            Debug.DrawRay(transform.position, up, Color.cyan, 0, false);
            Physics.gravity = -up * gravityStrength;
        }

    }
}