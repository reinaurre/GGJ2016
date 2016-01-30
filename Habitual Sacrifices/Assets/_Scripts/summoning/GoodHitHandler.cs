using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
class GoodHitHandler : MonoBehaviour {
    public string soundOnHit = "whoosh";

    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(HandleHit);
    }

    public void HandleHit() {
        Destroy(this.gameObject);

        ServiceLocator.GetSoundSystem().PlaySound(soundOnHit);
        ServiceLocator.GetGameManager().IncrementScore(100);
    }
}
