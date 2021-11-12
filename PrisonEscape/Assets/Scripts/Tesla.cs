using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public GameObject[] roadPieces;
    private float spacing = 191f;
    private float smallest_z;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButton("Shoot"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        float dt = Time.deltaTime;
        foreach (GameObject road in roadPieces)
        {
            // Scroll speed
            road.transform.Translate(0, 0, 25 * dt);
            if (road.transform.position.z >= 0)
            {
                // Paste road after the last roadpiece (scaleable for any length roadPieces)
                smallest_z = 0;
                foreach (GameObject road2 in roadPieces)
                {
                    if (road2.transform.position.z < smallest_z)
                    {
                        smallest_z = road2.transform.position.z;
                    }
                }
                road.transform.position = new Vector3(0, 0, smallest_z - spacing);
            }
        }
    }
}