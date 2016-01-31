using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour {

  bool played = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    if (!played) {
      ServiceLocator.GetSoundSystem().PlaySound("laugh1");
      ServiceLocator.GetSoundSystem().PlaySound("laugh2");
      ServiceLocator.GetSoundSystem().PlayBackgroundMusic("title");
      played = true;
    }
	
	}
}
