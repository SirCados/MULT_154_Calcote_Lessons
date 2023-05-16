using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float Speed = 2f;
    public GameObject SpawnPoint;

    Vector3 _movementVector;
    Rigidbody _playerRigidBody;
    Dictionary<Collectable.VegetableType, int> _itemInventory = new Dictionary<Collectable.VegetableType, int>();

    // Start is called before the first frame update
    void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody>();

        foreach (Collectable.VegetableType item in System.Enum.GetValues(typeof(Collectable.VegetableType)))
        {
            _itemInventory.Add(item, 0);
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
            output += string.Format("{0}: {1}", keyValue.Key, keyValue.Value);
        }
        print(output);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {       
        float horizontalMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");

        _movementVector = new Vector3(horizontalMovement, 0, forwardMovement);

        if(_movementVector.magnitude > 0)
        {
            _playerRigidBody.AddForce(_movementVector * Speed, ForceMode.Impulse);
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
        _playerRigidBody.MovePosition(SpawnPoint.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {

            Collectable item = other.gameObject.GetComponent<Collectable>();
            AddToInventory(item);
            PrintInventory();
        }
    }
}
