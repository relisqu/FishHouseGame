using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTranslate : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] float Speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        transform.Translate(transform.TransformDirection(Vector3.forward * Speed * Time.deltaTime));
        //transform.position += Vector3.forward * Speed * Time.deltaTime;
    }
}
