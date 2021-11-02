using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiberWire : MonoBehaviour
{
    public Transform Camera;
    public Transform Player;
    public float FireRange = 5f;
    RaycastHit hitInfo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            // Raycast to find target
            if (Physics.Raycast(Camera.position, Camera.forward, out hitInfo, FireRange))
            {
                NPC enemy = hitInfo.transform.GetComponent<NPC>();
                if (enemy != null)
                {
                    // Only start fiberwire if behind the enemy
                    if (Mathf.Abs(enemy.transform.eulerAngles.y - Player.eulerAngles.y) < 45)
                    {
                        Debug.Log("(FiberWire.cs) sufficiently behind enemy");
                        StartCoroutine(Fiberwire(enemy)); 
                    }
                    else
                    {
                        Debug.Log("(FiberWire.cs) insufficient angle");
                    }
                }
            }
        }
    }

    IEnumerator Fiberwire(NPC enemy)
    {
        enemy.Grabme();
        yield return new WaitForSeconds(2f);
        enemy.TakeDamage(100);
    }
}
