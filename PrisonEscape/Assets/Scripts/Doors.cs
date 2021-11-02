using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private bool IsOpen = false;
    private bool IsBusy = false;
    public bool IsLocked = false;
    Quaternion defaultRotation;

    void Start()
    {
        defaultRotation = transform.parent.transform.rotation;
    }

    // Checks if door should open or close
    public void ToggleDoor(Transform actor)
    {
        if (!IsBusy)
        {
            IsBusy = true;
            if (IsOpen)
            {
                CloseDoor();
            }
            else if (!IsLocked)
            {
                OpenDoor(actor);
            }
            else
            {
                Debug.Log("(Doors.cs) The door is locked");
                IsBusy = false;
            }
        }
        else
        {
            Debug.Log("(Doors.cs) stop spamming doors dickhead");
        }
    }

    // Opens the door
    public void OpenDoor(Transform actor)
    {
        var heading = actor.position - transform.position;
        float dot = Vector3.Dot(heading, transform.right);
        // if greater then 0, open normally
        if (dot > 0)
        {
            Debug.Log("(Doors.cs) opening door (pos)");
            StartCoroutine(RotateObject(false, Vector3.up, .7f));
            IsOpen = true;
        }
        else
        {
            Debug.Log("(Doors.cs) opening door (neg)");
            StartCoroutine(RotateObject(true, Vector3.up, .7f));
            IsOpen = true;
        }
    }

    // Closes the door
    public void CloseDoor()
    {
        float rot = defaultRotation.y - transform.parent.transform.rotation.y;
        Debug.Log("(Doors.cs) closing door" + rot);
        if (rot > 0)
        {
            StartCoroutine(RotateObject(true, Vector3.down, .7f));
        }
        else
        {
            StartCoroutine(RotateObject(false, Vector3.down, .7f));
        }
        IsOpen = false;
    }
    
    // Animated rotation of the door swinging open/closed
    IEnumerator RotateObject(bool reverse, Vector3 axis, float inTime)
    {
        // calculate rotation speed
        float angle = 90;
        float rotationSpeed = angle / inTime;

        // save starting rotation position
        Quaternion startRotation = transform.parent.transform.rotation;
        float deltaAngle = 0;
        while (deltaAngle < angle)
        {
            deltaAngle += rotationSpeed * Time.deltaTime;
            deltaAngle = Mathf.Min(deltaAngle, angle);
            if (reverse)
            {
                transform.parent.transform.rotation = startRotation * Quaternion.AngleAxis(-1 * deltaAngle, axis);
            }
            else
            {
                transform.parent.transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);
            }
            yield return null;
        }
        // rotate until reaching angle
        IsBusy = false;
        
        yield return new WaitForSeconds(5f);
        // Closes the door if it is still open after 5 seconds
        if (IsOpen && !IsBusy)
        {
            IsBusy = true;
            CloseDoor();
        }
    }
}
