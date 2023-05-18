using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private float Speed = 2f;
    public GameObject[] SpawnPoints;
    Rigidbody _playerRigidBody;
    
    Dictionary<Collectable.VegetableType, int> _itemInventory = new Dictionary<Collectable.VegetableType, int>();

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            _playerRigidBody = GetComponent<Rigidbody>();

            foreach (Collectable.VegetableType item in System.Enum.GetValues(typeof(Collectable.VegetableType)))
            {
                _itemInventory.Add(item, 0);
            }

            SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        }       

    }

    void AddToInventory(Collectable item)
    {
        _itemInventory[item.typeOfVegetable]++;
    }

    void PrintInventory()
    {
        string output = "";

        foreach(KeyValuePair<Collectable.VegetableType, int> keyValue in _itemInventory)
        {
            output += string.Format("{0}: {1} ", keyValue.Key, keyValue.Value);
        }
        print(output);
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
        float horizontalMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontalMovement, 0, forwardMovement);

        if(movementVector != Vector3.zero)
        {
            print(movementVector); 
            print("isLocal " + isLocalPlayer);
        }            

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

    private void Respawn()
    {
        int index = 0;

        while (Physics.CheckBox(SpawnPoints[index].transform.position, new Vector3(1.5f, 1.5f, 1.5f)))
        {
            index++;
        }

        _playerRigidBody.MovePosition(SpawnPoints[index].transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLocalPlayer && other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer && other.CompareTag("Collectable"))
        {
            Collectable item = other.gameObject.GetComponent<Collectable>();
            AddToInventory(item);
            PrintInventory();
        }
    }
}
