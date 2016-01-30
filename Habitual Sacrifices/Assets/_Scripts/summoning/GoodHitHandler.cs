using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
class GoodHitHandler : MonoBehaviour {
    public string soundOnHit = "whoosh";

    private SoundSystem soundSystem = null;

    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(HandleHit);

        GameObject soundSystemGO = GameObject.FindWithTag("SoundSystem");
        if (soundSystemGO != null) {
            soundSystem = soundSystemGO.GetComponent<SoundSystem>();
        }
    }

    public void HandleHit() {
        Destroy(this.gameObject);

        if (soundSystem != null) {
            soundSystem.PlaySound(soundOnHit);
        }
    }
}
