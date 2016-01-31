using UnityEngine;

class CursorInputController : MonoBehaviour {
    public string xAxis = "Horizontal";
    public string yAxis = "Vertical";
    public string activateButton = "Action";

    public float xMoveSensitivity = 1.0f;
    public float yMoveSensitivity = 1.0f;
    public float maxSpeedUp = 3.0f;

    public float castLeeway = 0.2f;
    public Transform castPosition = null;

    private RectTransform parentRectTransform;

    void Start() {
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        /* Disable input when the game is paused */
        this.enabled = false;
        ServiceLocator.GetGameManager().OnLevelBegin.AddListener(() => (this.enabled = true));
        ServiceLocator.GetGameManager().OnLevelEnd.AddListener((x) => (this.enabled = false));
		ServiceLocator.GetGameManager().winOnTimeOut = true;
    }

    void Update() {
        float xAxisVal = Input.GetAxis(xAxis);
        float yAxisVal = Input.GetAxis(yAxis);
        bool activate = Input.GetButtonDown(activateButton);

        GameManager manager = ServiceLocator.GetGameManager();
        float speedFactor = manager.GetSpeedFactor(maxSpeedUp);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        pos.x += xAxisVal * xMoveSensitivity * speedFactor;
        pos.y += yAxisVal * yMoveSensitivity * speedFactor;

        Rect parentSize = parentRectTransform.rect;
        pos.x = Mathf.Clamp(pos.x, 0.0f, parentSize.width);
        pos.y = Mathf.Clamp(pos.y, 0.0f, parentSize.height);

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);

        if (activate) {
            RaycastHit hitInfo;
            Vector2 castFromPos = new Vector2(castPosition.position.x, castPosition.position.y);
            Camera mainCamera = Camera.main;
            Vector3 screenPoint = new Vector3(castFromPos.x, castFromPos.y, mainCamera.nearClipPlane);
            Ray rayFromCamera = mainCamera.ScreenPointToRay(screenPoint);
            Vector3 halfExtents = new Vector3(castLeeway, castLeeway, castLeeway);
            bool hit = Physics.BoxCast(rayFromCamera.origin, halfExtents, rayFromCamera.direction, out hitInfo);

            if (hit) {
                HitReceiver hitReceiver = hitInfo.collider.GetComponent<HitReceiver>();
                if (hitReceiver != null) {
                    hitReceiver.ReceiveHit();
                }
            }
        }
    }
}
