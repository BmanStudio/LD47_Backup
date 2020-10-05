using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float jumpPower = 8;
    public bool canTakeActions = true;

   PlayerWeapon _weapon = null;

    public PlayerWeapon Weapon
    {
        get => _weapon;
        set => _weapon = value;
    }

    Camera cam;
    private Vector3 camForward= new Vector3();
    Rigidbody rigidBody;

    // Added by Bman:
    private float startedSpeed;

    void Awake()
    {
        startedSpeed = moveSpeed;
    }

    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        cam =GetComponentInChildren<Camera>();
        rigidBody = GetComponentInChildren<Rigidbody>();
        
    }


    void Update()
    {
        if (!canTakeActions) return;
        camForward.x = cam.transform.forward.x;
        camForward.z = cam.transform.forward.z;
        transform.Translate(camForward.normalized * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime,Space.World);
        transform.Translate(cam.transform.right * -Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, Space.World);

        
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down,1) ) {
            rigidBody.velocity += jumpPower * Vector3.up;
        }
        if (_weapon!=null && Input.GetKey(KeyCode.Mouse0)) {
            _weapon.Fire(cam.transform.forward,gameObject);
        }
    }
    
    // Added by Bman:
    public void UpdateSpeedPassiveBonus(float value)
    {
        moveSpeed = startedSpeed + value;
        Debug.Log("Updated the move speed cuz of a passive bonus, now the speed is " + moveSpeed);
    }
}
