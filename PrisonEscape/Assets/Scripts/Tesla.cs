using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public GameObject[] roadPieces;
    private float spacing = 191.9f;
    private float teleport_z = 70f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //teleport_z = 70f;
        //teleport_z = spacing / 2;
        teleport_z = spacing * 1.1f;
    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject road in roadPieces)
        {
            road.transform.Translate(0, 0, 10 * Time.deltaTime);
            if (road.transform.position.z > teleport_z)
            {
                road.transform.Translate(0, 0, road.transform.position.z - (roadPieces.Length*(spacing) + teleport_z));
            }
        }
    }
}
