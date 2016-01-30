using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
class HitEchoer : MonoBehaviour {
    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(this.Echo);
    }

    public void Echo() {
        Util.Log("hit");
    }
}
