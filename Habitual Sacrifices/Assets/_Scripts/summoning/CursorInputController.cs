using UnityEngine;

class CursorInputController : MonoBehaviour {
    public string xAxis = "Horizontal";
    public string yAxis = "Vertical";
    public string activateButton = "Action";

    public float xMoveSensitivity = 1.0f;
    public float yMoveSensitivity = 1.0f;

    public float castLeeway = 0.5f;

    private RectTransform parentRectTransform;

    void Start() {
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    void Update() {
        float xAxisVal = Input.GetAxis(xAxis);
        float yAxisVal = Input.GetAxis(yAxis);
        bool activate = Input.GetButtonDown(activateButton);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        pos.x += xAxisVal * xMoveSensitivity;
        pos.y += yAxisVal * yMoveSensitivity;

        Rect parentSize = parentRectTransform.rect;
        pos.x = Mathf.Clamp(pos.x, 0.0f, parentSize.width);
        pos.y = Mathf.Clamp(pos.y, 0.0f, parentSize.height);

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        if (activate) {
            RaycastHit hitInfo;
            Camera mainCamera = Camera.main;
            Vector3 screenPoint = new Vector3(pos.x, pos.y, mainCamera.nearClipPlane);
            Ray rayFromCamera = mainCamera.ScreenPointToRay(screenPoint);
            Vector3 halfExtents = new Vector3(castLeeway, castLeeway, castLeeway);
            bool hit = Physics.BoxCast(rayFromCamera.origin, halfExtents, rayFromCamera.direction, out hitInfo);

            if (hit) {
                HitReceiver hitReceiver = hitInfo.collider.GetComponent<HitReceiver>();
                hitReceiver.ReceiveHit();
            }
        }
    }
}
