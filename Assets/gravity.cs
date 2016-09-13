using UnityEngine;
using System.Collections;
using Cubiquity;

public class gravity : MonoBehaviour {

    public int gravityRadius = 4;
    public int gravityCheckRadius = 4;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
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
            Debug.DrawRay(transform.position, up, Color.blue, 0, false);
            Physics.gravity = -up * 10f;
        }
    }
}
