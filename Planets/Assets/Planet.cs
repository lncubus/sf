using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
    public Vector3 centerOfRotation = Vector3.zero;
    public float rotationSpeed;
    public float orbitRadius;
    public Vector3 orbitAxis = Vector3.up;
    public float radius = 1;

    Vector3 RandomTangent(Vector3 vector)
    {
        return Quaternion.FromToRotation(Vector3.forward, vector) * (Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right);
    }

    // Start is called before the first frame update
    void Start()
    {
        orbitAxis.Normalize();
        transform.localScale = new Vector3(radius, radius, radius);
        // (y,−x,0), (z, 0,−x), or (0, z,−x) 
        transform.position = centerOfRotation + orbitRadius * RandomTangent(orbitAxis);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        transform.RotateAround(centerOfRotation, orbitAxis, rotationSpeed * Time.fixedDeltaTime);
    }
}
