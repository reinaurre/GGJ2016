using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HitReceiver))]
class GoodHitHandler : MonoBehaviour {
    public string soundOnHit = "whoosh";

    public class InCauldronEvent : UnityEvent<Vector3> {};
    public InCauldronEvent OnInCauldron = new InCauldronEvent();

    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(HandleHit);
    }

    public void HandleHit() {
        Destroy(this.gameObject);

        ServiceLocator.GetSoundSystem().PlaySound(soundOnHit);
        ServiceLocator.GetGameManager().IncrementScore(100);
    }

    void OnTriggerEnter(Collider collider) {
        ServiceLocator.GetGameManager().LoseLife();
        OnInCauldron.Invoke(this.transform.position);
    }
}
