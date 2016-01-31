using UnityEngine;
using System.Collections;

public class chickenCollisions : MonoBehaviour {
    public string fail;
    public string pass;

    public string audioToPlay = null;

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag.Equals(fail))
        {
            ServiceLocator.GetGameManager().LoseLife();
        }
        if(other.gameObject.tag.Equals(pass))
        {
            ServiceLocator.GetGameManager().IncrementScore(20);
        }

        if (audioToPlay != null) {
            ServiceLocator.GetSoundSystem().PlaySound(audioToPlay);
        }
    }
}
