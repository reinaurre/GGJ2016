using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ingredient {
    public string name;
    public Transform[] variations;
}

public class simon_Controller : MonoBehaviour {

	public GameObject objectCam;
	public GameObject objectCell;

    public Transform[] spawnPoints;

    public Ingredient[] ingredients;

    Dictionary<string, Transform[]> ingredientMap = new Dictionary<string, Transform[]>();

	Animator objectAnim;
	Animator objectCamAnim;
    Transform objectCellTransform;
    Transform newObjectObj;
    

	// Use this for initialization
	void Start () {

        	
		objectAnim = objectCell.GetComponent <Animator> ();
		objectCamAnim = objectCam.GetComponent <Animator> ();
        objectCellTransform = objectCell.transform;

        for (int i = 0; i < ingredients.Length; i++)
        {
            Ingredient ingredient = ingredients[i];
            ingredientMap[ingredient.name] = ingredient.variations;
        }

        int indexToSpawn = (int)Random.Range(0.0f, ingredients.Length);
        Ingredient ingredientToSpawn = ingredients[indexToSpawn];

        Spawn(ingredientToSpawn.name);
	}

    void Spawn(string name)
    {
        Transform[] variations = ingredientMap[name];
        int indexToObject = (int)Random.Range(0.0f, variations.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform newObject = Instantiate(variations[i], spawnPoints[i].position, spawnPoints[i].rotation) as Transform;
            newObject.parent = spawnPoints[i].transform;
            if (i == indexToObject) {
                newObjectObj = Instantiate(variations[i], objectCellTransform.position, objectCellTransform.rotation) as Transform;
                newObjectObj.parent = spawnPoints[i].transform;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
        // to be removed testing purposes
		if (Input.GetButtonDown ("Start")) {
			objectAnim.SetTrigger ("blur_rotation");
			objectCamAnim.SetTrigger ("blur_rotation");
		}

	}
}
