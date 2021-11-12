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
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Shoot"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        float dt = Time.deltaTime;
        foreach (GameObject road in roadPieces)
        {
            //road.transform.Translate(0, 0, 50 * dt);
            road.transform.Translate(0, 0, 0.6f);
            if (road.transform.position.z >= 0)
            {
                road.transform.Translate(0, 0, road.transform.position.z - (roadPieces.Length * spacing));
                //road.transform.position = new Vector3(0, 0, -191.9f*2);
            }
        }
    }
}
