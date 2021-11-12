using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public GameObject[] roadPieces;
    //private float spacing = 191.9f;
    private float spacing = 191f;
    private float other_y;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        other_y = roadPieces[roadPieces.Length - 1].transform.position.z;
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
            road.transform.Translate(0, 0, 20 * dt);
            if (road.transform.position.z >= 0)
            {
                road.transform.position = new Vector3(0, 0, other_y - spacing);
            }
            other_y = road.transform.position.z;
        }
    }
}