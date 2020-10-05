using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float jumpPower = 8;
    public bool canTakeActions = true;
    public float stepRate = 0.5f;
    public float stepCoolDown;
    public AudioClip[] footsteps;
    AudioSource source;
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
        source = GetComponent<AudioSource>();
        
    }


    void Update()
    {
        if (!canTakeActions) return;
        camForward.x = cam.transform.forward.x;
        camForward.z = cam.transform.forward.z;
        transform.Translate(camForward.normalized * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime,Space.World);
        transform.Translate(cam.transform.right * -Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, Space.World);

        bool canJump = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        Debug.DrawRay(transform.position, Vector3.down,Color.red);
        if (Input.GetKeyDown(KeyCode.Space) && canJump ) {
            rigidBody.velocity += jumpPower * Vector3.up;
        }
        if (_weapon!=null && Input.GetKey(KeyCode.Mouse0)) {
            _weapon.Fire(cam.transform.forward, gameObject);
        }

        stepCoolDown -= Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f && canJump)
        {
            source.pitch = 1f + Random.Range(-0.2f, 0.2f);
            source.PlayOneShot(footsteps[Random.Range(0,footsteps.Length)], 0.9f);
            stepCoolDown = stepRate;
        }
    }
    
    // Added by Bman:
    public void UpdateSpeedPassiveBonus(float value)
    {
        moveSpeed = startedSpeed + value;
        Debug.Log("Updated the move speed cuz of a passive bonus, now the speed is " + moveSpeed);
    }
}
