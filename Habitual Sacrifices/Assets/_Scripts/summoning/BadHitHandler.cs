using UnityEngine;

[RequireComponent(typeof(HitReceiver))]
class BadHitHandler : MonoBehaviour {
    public string soundOnHit = "badSound";

    public void Awake() {
        HitReceiver receiver = GetComponent<HitReceiver>();
        receiver.OnHitEvent.AddListener(HandleHit);
    }

    public void HandleHit() {
        /* Do some indicator thing to show something bad happened */
        ServiceLocator.GetSoundSystem().PlaySound(soundOnHit);
        //ServiceLocator.GetGameManager().LoseLife();
    }
}
