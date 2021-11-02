using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform Camera;
    public Transform firePoint;
    [SerializeField] private LayerMask layerMask;

    private float FireRate = (1 / (200 / 60f));
    public float FireRange = 50f;
    private float ReloadTime = 2f;
    private float NextFire;
    private int MagSize = 18;
    public int MagFill;
    public int damage = 30;
    public float max_spread = 5f;
    private float spread;
    public LineRenderer lineRenderer;

    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    //new
    RaycastHit hitInfo;
    void Start()
    {
        // Fill mag with 30 rounds
        MagFill = MagSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Fires gun when asked and manages ammo
        if (Input.GetButton("Shoot"))
        {
            if (Time.time > NextFire)
            {
                if (MagFill > 0)
                {
                    // remove bullet and wait firerate
                    MagFill -= 1;
                    NextFire = Time.time + FireRate;
                    StartCoroutine(Fire());
                }
                else
                {
                    // play reload sound and wait on reloadtime and fill gun
                    NextFire += ReloadTime;
                    audioSource.PlayOneShot(audioClipArray[1]);
                    MagFill = MagSize;
                }
            }
            if (Input.GetButtonDown("Reload") && MagFill != MagSize)
            {
                NextFire += ReloadTime;
                audioSource.PlayOneShot(audioClipArray[1]);
                MagFill = MagSize;
            }
        }
        
    }

    IEnumerator Fire()
    {
        // play sound
        audioSource.PlayOneShot(audioClipArray[0]);
        // raycast and return object it hit
        if (Physics.Raycast(Camera.position, Camera.forward, out hitInfo, FireRange))
        {
            Debug.Log("(Weapon.cs) I hit something");
            NPC enemy = hitInfo.transform.GetComponent<NPC>();
            if (enemy != null)
            {
                Debug.Log("(Weapon.cs) It was an enemy pog");
                enemy.TakeDamage(damage);
            }
            else
            {
                Debug.Log("(Weapon.cs) Not an enemy sadge");
            }
            // tracer if hit
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        // tracer if no hit
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, Camera.transform.position + Camera.transform.forward * FireRange);
        }
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.0005f);
        lineRenderer.enabled = false;
    }
}
