using UnityEngine;

class ServiceLocator {
    private static SoundSystem soundSystem = null;
    private static GameManager gameManager = null;

    static private void GetServiceWithTag<T>(string tag, ref T service) {
        /* The second Equals check is true when the service is destroyed */
        if (service == null || service.Equals(null)) {
            GameObject go = GameObject.FindWithTag(tag);
            if (go != null) {
                service = go.GetComponent<T>();
                if (service == null) {
                    Util.LogWarning("Tried to get the service with tag {0}, but it doesn't have the right type ({1})", tag, typeof(T).Name);
                }
            } else {
                Util.LogWarning("Tried to get the service with tag {0}, but it doesn't exist!", tag);
            }
        }
    }

    public static SoundSystem GetSoundSystem() {
        GetServiceWithTag<SoundSystem>("SoundSystem", ref soundSystem);
        return soundSystem;
    }

    public static GameManager GetGameManager() {
        GetServiceWithTag<GameManager>("Manager", ref gameManager);
        return gameManager;
    }
}
