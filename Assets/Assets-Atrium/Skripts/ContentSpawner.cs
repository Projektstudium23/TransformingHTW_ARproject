using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject grass;
    GameObject[,] initiatedGrass;

    public float xOffset;
    public float zOffset;
    public int xRange, zRange;


    void Start() {
       // initiatedGrass = new GameObject[xRange, zRange];
      //  spawnGrass();
    }


    void spawnGrass() {
        initiatedGrass = new GameObject[xRange, zRange];
        for (int x = 0; x < xRange; x++) {
            for (int z = 0; z < zRange; z++) {
                Vector3 pos = new Vector3(x + xOffset, 0, z + zOffset);
                Quaternion rot = Quaternion.Euler(new(0,0,0));
                initiatedGrass[x, z] = Instantiate(grass, pos, rot);
                initiatedGrass[x, z].transform.parent = this.transform;
            }

        }

    }




}
