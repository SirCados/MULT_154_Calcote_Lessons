using System.Collections;
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
    bool _isDead = false;
    Transform _lookTarget;

    Animator _animator;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _lookTarget = GameObject.Find("HeadAimTarget").transform;
        InvokeRepeating("TwitchEar", 2.0f, 2.0f);
    }
    void Update()
    {
        if (!_isDead)
        {
            TranslateAndRotate();
            PlayerAction();
        }
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

        _animator.SetFloat("speed", translation);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDead && collision.collider.CompareTag("Hazard"))
        {
            _animator.SetBool("isDead", true);
            _isDead = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isDead && other.CompareTag("Hazard"))
        {
            StartCoroutine(LookAndLookAway(_lookTarget.position, other.transform.position));
        }
    }

    void TwitchEar()
    {
        _animator.SetTrigger("TwitchLeftEar");
    }

    private IEnumerator LookAndLookAway(Vector3 targetPosition, Vector3 hazardPosition)
    {
        const int INTERVALS = 20;
        const float INTERVAL_TIME = .5f / INTERVALS;

        Vector3 targetVector = targetPosition - transform.position;
        Vector3 hazardVector = hazardPosition - transform.position;

        float angle = Vector2.SignedAngle(new Vector2(targetVector.x, targetVector.z), new Vector2(hazardVector.x, hazardVector.z));

        float intervalAngle = angle / INTERVALS;

        for(int counter = 0; counter < INTERVALS; counter++)
        {
            _lookTarget.RotateAround(transform.position, Vector3.up, -intervalAngle);
            yield return new WaitForSeconds(INTERVAL_TIME);
        }

        for (int counter = 0; counter < INTERVALS; counter++)
        {
            _lookTarget.RotateAround(transform.position, Vector3.up, intervalAngle);
            yield return new WaitForSeconds(INTERVAL_TIME);
        }
    }
}
