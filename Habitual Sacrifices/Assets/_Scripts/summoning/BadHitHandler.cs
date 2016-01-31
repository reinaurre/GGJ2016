using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HitReceiver))]
class BadHitHandler : MonoBehaviour {
    public string soundOnHit = "badSound";

    public class InCauldronEvent : UnityEvent<Vector3> {};
    public InCauldronEvent OnInCauldron = new InCauldronEvent();

    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(HandleHit);
    }

    public void HandleHit() {
        /* Do some indicator thing to show something bad happened */
        ServiceLocator.GetSoundSystem().PlaySound(soundOnHit);
        ServiceLocator.GetGameManager().LoseLife();
    }

    void OnTriggerEnter(Collider collider) {
        Destroy(this.gameObject);
        OnInCauldron.Invoke(this.transform.position);
    }
}
