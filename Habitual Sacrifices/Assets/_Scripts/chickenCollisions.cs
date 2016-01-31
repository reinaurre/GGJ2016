using UnityEngine;
using System.Collections;

public class chickenCollisions : MonoBehaviour {
    public string fail;
    public string pass;

    public string audioToPlay = null;

    void OnTriggerEnter (Collider other)
    {
        bool playSound = false;
        if (other.gameObject.tag.Equals(fail))
        {
            ServiceLocator.GetGameManager().LoseLife();
            playSound = true;
        }
        if(other.gameObject.tag.Equals(pass))
        {
            ServiceLocator.GetGameManager().IncrementScore(20);
            playSound = true;
        }

        if (playSound && audioToPlay != null) {
            ServiceLocator.GetSoundSystem().PlaySound(audioToPlay);
            ServiceLocator.GetSoundSystem().PlayBackgroundMusic("virgins");
        }
    }
}
