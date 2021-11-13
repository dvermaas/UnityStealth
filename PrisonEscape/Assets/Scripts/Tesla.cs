using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public GameObject[] roadPieces;
    private float spacing = 192f;
    private float smallest_z;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButton("Shoot")) Cursor.lockState = CursorLockMode.None;
                
        float dt = Time.deltaTime;
        smallest_z = 0;
        // Move all roadpieces and save the last one
        foreach (GameObject road in roadPieces)
        {
            road.transform.Translate(0, 0, 25 * dt);
            if (road.transform.position.z < smallest_z) smallest_z = road.transform.position.z;
        }
        // If first piece is behind car teleport to front
        foreach (GameObject road in roadPieces) if (road.transform.position.z >= 0) road.transform.position = new Vector3(0, 0, smallest_z - spacing);
    }
}