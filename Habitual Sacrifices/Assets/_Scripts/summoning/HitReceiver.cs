using UnityEngine;
using UnityEngine.Events;

class HitReceiver : MonoBehaviour {
    public class HitEvent : UnityEvent {};
    public HitEvent OnHitEvent = new HitEvent();

    public void ReceiveHit() {
        OnHitEvent.Invoke();
    }
}
