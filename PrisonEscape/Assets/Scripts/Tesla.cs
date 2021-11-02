using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public GameObject[] roadPieces;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject road in roadPieces)
        {
            road.transform.Translate(0, 0, 15 * Time.deltaTime);
            if (road.transform.position.z > 70)
            {
                road.transform.Translate(0, 0, road.transform.position.z - 230);
            }
        }
    }
}
