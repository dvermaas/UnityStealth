using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Controls : MonoBehaviour
{
    // HP
    public int HP = 100;
    // Leg vars
    private Rigidbody _rigidbody;
    private float MovementSpeedBase = 3;
    private float MovementSpeed;
    private float JumpForce = 3;
    // Cam vars
    private GameObject _camera;
    private GameObject _gun;
    private float XRotation = 0f;
    public float MouseSensitivity = 300f;
    // crouch vars
    public bool IsCrouching = false;
    private BoxCollider _collisionFeet;
    public bool IsSprinting = false;
    // Doors
    RaycastHit hitInfo;
    public float FireRange = 50f;
    // PauseMenu
    public GameObject Menu;
    // Item gameobjets
    public GameObject[] ItemArray;
    // Inventory
    [SerializeField] private UI_Inventory uiInventory;
    public GameObject InventoryMenu;
    private Inventory inventory;

    private GameObject _fps;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        MovementSpeed = MovementSpeedBase;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _camera = GameObject.Find("Main Camera");
        _gun = GameObject.Find("Glock");
        _fps = GameObject.Find("FPS_Counter");

        _rigidbody = GetComponent<Rigidbody>();
        _collisionFeet = GetComponent<BoxCollider>();

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }

    // Update is called once per frame
    void Update()
    {
        // Menu
        PauseGame();
        // Inv
        Inventory();
        // Move (wasd)
        Move();
        // Looking (mouse)
        Look();
        // Jump (space)
        Jump();
        // Run (shift)
        Run();
        // Crouching (c)
        Crouch();
        // Interact (f)
        Interact(); 
        // This is retarded to constantly update bruh
        HandUpdate();
        // FPS
        //_fps.GetComponent<Text>().text = ((int)(1f / Time.unscaledDeltaTime)).ToString() + " FPS";
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fps.GetComponent<Text>().text = fps + "FPS";
            _timer = Time.unscaledTime + .1f;
        }
    }
    private void Inventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            uiInventory.CheckInventory();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    // Pauze menu
    public void PauseGame()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("Paused the game");
            Menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    // Resume menu
    public void ResumeGame()
    {
        Debug.Log("Resumed the game");
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            // Look for doors
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, FireRange))
            {
                Doors doors = hitInfo.transform.GetComponent<Doors>();
                if (doors != null)
                {
                    doors.ToggleDoor(transform);
                }
            }
        }
    }

    private void Equip(string itemName)
    {
        Debug.Log("I should equip" + itemName);
    }

    private void Move()
    {
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        float VectorLength = dir.x * dir.x + dir.z * dir.z;
        // Correct for diagonal andy's
        if (VectorLength > 1)
        {
            dir = dir.normalized;
        }
        transform.Translate(dir * MovementSpeed * Time.deltaTime);
    }

    private void Look()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        XRotation -= MouseY;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        transform.Rotate(Vector3.up * MouseX);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode.Impulse);
        }
    }

    private void Run()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (!IsCrouching)
            {
                MovementSpeed = 2 * MovementSpeedBase;
            }
        }
        if (Input.GetButtonUp("Sprint"))
        {
            if (!IsCrouching)
            {
                MovementSpeed = MovementSpeedBase;
            }
        }
    }

    // Change held item
    private void HandUpdate()
    {
        // Swap equiped item
        for (int i=0; i < ItemArray.Length; i++)
        {
            if (i == inventory.GetSelected().gameObject)
            {
                continue;
            }
            ItemArray[i].SetActive(false);
        }

        ItemArray[inventory.GetSelected().gameObject].SetActive(true);
    }

    // Crouch
    private void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (IsCrouching)
            {
                if (IsSprinting)
                {
                    MovementSpeed = MovementSpeedBase;
                }
                StartCoroutine(CrouchUp(1));
                MovementSpeed /= .6f;
            }
            else
            {
                StartCoroutine(CrouchUp(-1));
                MovementSpeed *= .6f;
            }
            IsCrouching = !IsCrouching;
        } 
    }

    // Main Menu
    public void GotoMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Boot");
    }

    // Incoming damage script
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            SceneManager.LoadScene("Boot");
        }
    }

    // Crouch animation, to crouch down set reverse to -1
    IEnumerator CrouchUp(int reverse)
    {
        // full rotation 1 sec
        float time = .4f;
        float speed = 1f;

        while (time > 0)
        {
            //_camera.transform.Translate(reverse * Vector3.up * Time.deltaTime * speed, Space.World);
            _collisionFeet.center += new Vector3(0, -2 * reverse * speed * Time.deltaTime, 0);
            time -= Time.deltaTime;
            yield return null;
        }
    }
}