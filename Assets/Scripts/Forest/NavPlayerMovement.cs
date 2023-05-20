using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
{
    public delegate void DropHive(Vector3 position);
    public static event DropHive OnHiveDrop;

    public float Speed = 40.0f;
    public float RotationSpeed = 30.0f;
    Rigidbody _rigidBody = null;
    float _translation = 0;
    float _rotation = 0;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        TranslateAndRotate();
        PlayerAction();
    }

    void TranslateAndRotate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        _translation += translation;
        _rotation += rotation;
    }

    private void FixedUpdate()
    {
        Vector3 localRotation = transform.rotation.eulerAngles;
        localRotation.y += _rotation * RotationSpeed * Time.deltaTime;
        _rigidBody.MoveRotation(Quaternion.Euler(localRotation));
        _rotation = 0;

        Vector3 movementVector = transform.forward * _translation;
        _rigidBody.velocity = movementVector * Speed * Time.deltaTime;
        _translation = 0;
    }

    void PlayerAction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnHiveDrop?.Invoke(transform.position + (transform.forward * -10));
        }
    }
}
