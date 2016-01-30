using UnityEngine;

class FallingThing : MonoBehaviour {
    public float fallSpeed = 1.0f;
    public float rotationSpeed = 2.0f;

    private Quaternion originalRot;
    private Vector3 rotationAxis;
    private float rotation = 0.0f;

    void Awake() {
        originalRot = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        transform.rotation = originalRot;
        rotationAxis = Random.onUnitSphere;
    }

    void Update() {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - fallSpeed * Time.deltaTime, pos.z);

        rotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(rotation, rotationAxis) * originalRot;
    }
}
