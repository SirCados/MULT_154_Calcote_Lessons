using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadMovement : MonoBehaviour
{
    public float Speed = 5f;

    public enum DriftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }

    public DriftDirection drift = DriftDirection.LEFT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveLilypad();
        DestroyLilypad();
    }

    void MoveLilypad()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime * (float)drift);
    }

    void DestroyLilypad()
    {
        if(transform.position.x < -80)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject child = collision.gameObject;

            child.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject child = collision.gameObject;

            child.transform.SetParent(null);
        }
    }
}
