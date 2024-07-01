using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Vector3 startPosition = new Vector3(0, -4, 0);
    void Start()
    {
        // starting position
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * input * _speed * Time.deltaTime);

        if(transform.position.x > 9) {
            transform.position = new Vector3(9, transform.position.y, 0);
        } else if (transform.position.x < -9) {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
    }
}
