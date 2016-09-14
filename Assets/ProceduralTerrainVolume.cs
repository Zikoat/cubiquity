using UnityEngine;
using System.Collections;
using Cubiquity;

/**
 * This class serves as an example of how to generate a TerrainVolume from code. The exact operation
 * of the noise function(s) is not particularly important here as you will want to implement your own
 * approach for your game, but you should focus on understanding how data is written into the volume.
 * Please note, most of the 'magic numbers' in this code are simply found by trial and error as there
 * is a lot of experimentation required to generate procedural terrains. Feel free to change them and
 * see what happens!
 */
[ExecuteInEditMode]
public class ProceduralTerrainVolume : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // The size of the volume we will generate
        int width = 64;
		int height = 64;
		int depth = 64;
		
		//Create an empty TerrainVolumeData with dimensions width * height * depth
		TerrainVolumeData data = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, width-1, height-1, depth-1));
		
		TerrainVolume volume = GetComponent<TerrainVolume>();
		TerrainVolumeRenderer volumeRenderer = GetComponent<TerrainVolumeRenderer>();
		
		volume.data = data;
		
		// This example looks better if we adjust the scaling factors on the textures.
		volumeRenderer.material.SetTextureScale("_Tex0", new Vector2(0.062f, 0.062f));
		volumeRenderer.material.SetTextureScale("_Tex1", new Vector2(0.125f, 0.125f));	
		volumeRenderer.material.SetTextureScale("_Tex2", new Vector2(0.125f, 0.125f));
		
		// At this point our volume is set up and ready to use. The remaining code is responsible
		// for iterating over all the voxels and filling them according to our noise functions.
		
		// This scale factor comtrols the size of the rocks which are generated.
		float rockScale = 32.0f;		
		float invRockScale = 1.0f / rockScale;
		
		// Let's keep the allocation outside of the loop.
		MaterialSet materialSet = new MaterialSet();
		
		// Iterate over every voxel of our volume
		for(int z = 0; z < depth; z++)
		{
			for(int y = height-1; y > 0; y--)
			{
				for(int x = 0; x < width; x++)
				{
                    // Make sure we don't have anything left in here from the previous voxel 
                    materialSet.weights[0] = 0;
                    materialSet.weights[1] = 0;
                    materialSet.weights[2] = 0;

                    float sampleX = (float)x * invRockScale;
                    float sampleY = (float)y * invRockScale;
                    float sampleZ = (float)z * invRockScale;

                    float simplexNoiseValue = SimplexNoise.Noise.Generate(sampleX, sampleY, sampleZ);
                    simplexNoiseValue *= 5.0f;
                    simplexNoiseValue = Mathf.Clamp(simplexNoiseValue, -0.5f, 0.5f);
                    simplexNoiseValue += 0.5f;
                    simplexNoiseValue *= 255;

                    materialSet.weights[2] = (byte)simplexNoiseValue;
					
					data.SetVoxel(x, y, z, materialSet);
				}
			}
		}
        // this saves the map to the previously specified path
        // data.CommitChanges();
        volume.OnMeshSyncComplete += unFreezePlayer;
        stopwatch.Stop();
        Debug.Log(stopwatch.ElapsedMilliseconds + " ms");
    }

    void Update ()
    {
        TerrainVolume volume = GetComponent<TerrainVolume>();
        MaterialSet air = new MaterialSet();
        air.weights[0] = 0;
        air.weights[1] = 0;
        air.weights[2] = 0;

        Vector3 nearestCube = new Vector3();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.magenta);
            nearestCube.x = Mathf.RoundToInt(hit.point.x);
            nearestCube.y = Mathf.RoundToInt(hit.point.y);
            nearestCube.z = Mathf.RoundToInt(hit.point.z);
            
        }
        if(Input.GetMouseButtonDown(0))
            volume.data.SetVoxel(
                (int)nearestCube.x, 
                (int)nearestCube.y, 
                (int)nearestCube.z, 
                air);

    }

    void unFreezePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        playerRB.constraints = RigidbodyConstraints.None;  
    }
}
