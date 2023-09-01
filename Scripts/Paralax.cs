using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    Vector2 startPosition;
    float startZ;


    Vector2 travel => (Vector2)cam.transform.position - startPosition; //=> means read-only

    float distanceFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));


    float paralaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;
    private void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }
    private void Update()
    {
        Vector2 newPos = startPosition + travel * paralaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
