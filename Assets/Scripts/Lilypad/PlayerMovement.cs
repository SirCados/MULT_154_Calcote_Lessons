using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private float Speed = 2f;
    public GameObject[] SpawnPoints;
    Rigidbody _playerRigidBody;

    GameObject _vegetableToPickup;

    [SerializeField] InventoryScript _playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
    }

    private void Update()
    {
        PickVegetable();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            MovePlayer();
        }         
    }

    void MovePlayer()
    {
        if (isLocalPlayer)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float forwardMovement = Input.GetAxis("Vertical");

            Vector3 movementVector = new Vector3(horizontalMovement, 0, forwardMovement);

            if (movementVector.magnitude > 0)
            {
                _playerRigidBody.AddForce(movementVector * Speed, ForceMode.Impulse);
            }
            else
            {
                _playerRigidBody.AddForce(Vector3.zero, ForceMode.Impulse);
            }

            if (transform.position.z > 40)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 40);
            }
            else if (transform.position.z < -40)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -40);
            }
        }              
    }

    void PickVegetable()
    {
        if (isLocalPlayer && _vegetableToPickup != null && Input.GetKeyDown(KeyCode.Space))
        {
            Collectable item = _vegetableToPickup.GetComponent<Collectable>();            
            _playerInventory.AddToInventory(item);
        }        
    }

    private void Respawn()
    {
        int index = 0;

        while (Physics.CheckBox(SpawnPoints[index].transform.position, new Vector3(1.5f, 1.5f, 1.5f)))
        {
            index++;
        }

        _playerRigidBody.MovePosition(SpawnPoints[index].transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer && other.CompareTag("Collectable"))
        {
            _vegetableToPickup = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLocalPlayer && other.CompareTag("Collectable"))
        {
            _vegetableToPickup = null;
        }

        if (isLocalPlayer && other.CompareTag("Hazard"))
        {
            Respawn();
        }        
    }

    void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            _playerRigidBody = GetComponent<Rigidbody>();
            _playerInventory = GetComponent<InventoryScript>();
            SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        }
    }
}
