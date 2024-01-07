using UnityEngine;
using Random = UnityEngine.Random;

public class SolarSystem : MonoBehaviour
{
    public GameObject planetPrefab;
    public int Number = 8;
    public float minOrbitRadius = 2;
    public float maxOrbitRadius = 10;
    public float minSize = .2f;
    public float maxSize = 2f;
    public float gM = 10f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Number; i++)
        {
            var planet = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity).GetComponent<Planet>();
            planet.radius = Random.Range(minSize, maxSize);
            planet.orbitRadius = Random.Range(minOrbitRadius, maxOrbitRadius);
            planet.centerOfRotation = Vector3.zero;
            planet.orbitAxis = Random.onUnitSphere;
            planet.rotationSpeed = Mathf.Sqrt(gM*180/Mathf.PI / Mathf.Pow(planet.orbitRadius, 3));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
