using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    RaycastHit hit;
    public GameObject[] waypoints;
    public float FireRange = 50f;
    public LineRenderer lineRenderer;
    public Transform head;
    public float Heat = 0f;

    public int HP = 100;
    private Animator _animator;
    private NavMeshAgent NavMeshAgent;
    private GameObject Player;
    private GameObject HeatBar;
    
    private int WayPointCount = 0;

    private bool grabbed = false;

    public float StopSearch;
    public bool losing_sight = false;
    public float PatrolWaitTime;
    public bool PatrolWait = false;
    public float FireCooldown = 0f;

    // 0 = guest, 1 = staff, 2= guard, 3=target
    //public string ai_type = "guest";
    private bool illegal = true;
    private GameObject playerScript;

    public GameObject weapon;

    void Start()
    {
        _animator = GetComponent<Animator>();
        NavMeshAgent = transform.parent.GetComponent<NavMeshAgent>();
        playerScript = GameObject.Find("Player");
        Player = GameObject.Find("Main Camera");
        HeatBar = GameObject.Find("HeatBarFill");
    }


    
    void Update()
    {
        // If door in front of face open it
        DoorCheck();
        // Stop moving when grabbed.
        if (grabbed)
        {
            NavMeshAgent.isStopped = true;
        }
        // If spotted player, go into combat mode.
        else if ((SpotCheck()))
        {
            Combat();
        }
        // Otherwise patrol.
        else
        {
            Patrol();
        }
        
    }

    private void DoorCheck()
    {
        // Look for doors
        RaycastHit hitInfo;
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hitInfo, .5f))
        {
            Doors doors = hitInfo.transform.GetComponent<Doors>();
            if (doors != null)
            {
                doors.ToggleDoor(transform);
            }
        }
    }

    private void Combat()
    {
        // Do combat stuff here
        weapon.SetActive(true);
        _animator.SetBool("Combat", true);
        transform.parent.GetComponent<NavMeshAgent>().speed = 5f;

        // If lost sight (no viewcone) start countdown untill stopping search.
        if (!CanSeePlayer(360))
        {
            if (!losing_sight)
            {
                NavMeshAgent.destination = Player.transform.position;
                StopSearch = Time.time + 5f;
                losing_sight = true;
            }
            if (Time.time > StopSearch)
            {
                Heat = 99;
                HeatBar.GetComponent<Image>().color = new Color(1f, 0.475f, 0f, 1f);
                transform.parent.GetComponent<NavMeshAgent>().speed = 2f;
                weapon.SetActive(false);
                _animator.SetBool("Combat", false);
            }
        }
        else
        {
            losing_sight = false;
            NavMeshAgent.destination = Player.transform.position;
            // Engage in firefight
            if (Time.time > FireCooldown)
            {
                FireCooldown = Time.time + 1f;
                //
                int dmg = Random.Range(10, 30);
                Debug.Log("Enemy hit you for: " + dmg);
                playerScript.GetComponent<Player_Controls>().TakeDamage(dmg);
            }
        }
    }

    private void Patrol()
    {
        // Go to waypoint.
        NavMeshAgent.destination = waypoints[WayPointCount].transform.position;
        // If at waypoint go to the next one.
        if (Vector3.Distance(transform.position, waypoints[WayPointCount].transform.position) < 1)
        {
            // Allign with waypoint forward vector.
            transform.parent.transform.rotation = Quaternion.RotateTowards(transform.parent.transform.rotation, waypoints[WayPointCount].transform.rotation, 80f * Time.deltaTime);
            if (!PatrolWait)
            {
                PatrolWaitTime = Time.time + 5f;
                PatrolWait = true;
                _animator.SetFloat("Speed", 0f);
            }
            if (Time.time > PatrolWaitTime)
            {
                _animator.SetFloat("Speed", 5f);
                WayPointCount = (WayPointCount + 1) % waypoints.Length;
            }
            
        }
        else
        {
            PatrolWait = false;
            _animator.SetFloat("Speed", 5f);
        }
    }

    private bool SpotCheck()
    {
        if (Heat >= 100)
        {
            HeatBar.GetComponent<Image>().color = new Color(1f, 0, 0.235f, 1f);
            return true;
        }
        if (CanSeePlayer() && illegal)
        {
            Heat += 100 * Time.deltaTime;
            HeatBar.GetComponent<UI_Bar>().UpdateBar(Heat);
        }
        else
        {
            Heat -= 140 * Time.deltaTime;
            if (Heat < 0) Heat = 0;
            HeatBar.GetComponent<UI_Bar>().UpdateBar(Heat);
        }
        return false;
    }

    private bool CanSeePlayer(float viewAngle = 45f)
    {
        // Determine if within viewcone
        var rayDirection = (Player.transform.position - head.transform.position).normalized;
        if (Vector3.Angle(rayDirection, head.transform.forward) < viewAngle)
        {
            if (Physics.Raycast(head.transform.position, rayDirection, out hit))
            {
                // If enemy can see player
                if (hit.collider.gameObject == Player)
                {
                    lineRenderer.SetPosition(0, head.transform.position);
                    lineRenderer.SetPosition(1, hit.transform.position);
                    lineRenderer.enabled = true;
                    return true;
                }
            }
        }
        return false;
    }

    public void Grabme()
    {
        grabbed = true;
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        _animator.SetFloat("Health", HP);

        // If no HP stop navigation heat and script (Saving CPU)
        if (HP <= 0)
        {
            Heat = 0;
            HeatBar.GetComponent<UI_Bar>().UpdateBar(Heat);
            NavMeshAgent.isStopped = true;
            this.enabled = false;
        }
    }
}
