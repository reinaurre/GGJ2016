using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
class HitDestroyer : MonoBehaviour {
    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(() => Destroy(this.gameObject));
    }
}
