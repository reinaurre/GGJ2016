using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ingredient {
    public string name;
    public Transform[] variations;
}

public class simon_Controller : MonoBehaviour {
    public float maxScore = 1000.0f;

	public GameObject objectCam;
	public GameObject objectCell;
    public Transform select;

    public Transform[] spawnPoints;

    public Ingredient[] ingredients;

    Dictionary<string, Transform[]> ingredientMap = new Dictionary<string, Transform[]>();

	Animator objectAnim;
	Animator objectCamAnim;
    Transform objectCellTransform;
    Transform newObjectObj;
    float axisOld = 0;
    float axisNew;
    int selectIndex = 0;
    int selection = 4;
    int newObjectIndex;

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

        int indexToSpawn = (int)Random.Range(0.0f, ingredients.Length - 1);
        Ingredient ingredientToSpawn = ingredients[indexToSpawn];

        Spawn(ingredientToSpawn.name);

        objectAnim.SetTrigger("blur_rotation");
        objectCamAnim.SetTrigger("blur_rotation");
    }

    void Spawn(string name)
    {
        Transform[] variations = ingredientMap[name];
        int indexToObject = (int)Random.Range(0.0f, variations.Length - 1);
        newObjectIndex = indexToObject;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform newObject = Instantiate(variations[i], spawnPoints[i].position, spawnPoints[i].rotation) as Transform;
            newObject.parent = spawnPoints[i].transform;
            if (i == indexToObject) {
                newObjectObj = Instantiate(variations[i], objectCellTransform.position, objectCellTransform.rotation) as Transform;
                newObjectObj.parent = objectCellTransform.transform;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        GameManager manager = ServiceLocator.GetGameManager();
        
        if (Input.GetButtonDown("Action"))
        {
            if(selectIndex == newObjectIndex) {
                manager.IncrementScore(maxScore * manager.GetTimeFractionLeft());
                manager.WinLevelEarly();
            } else {
                ServiceLocator.GetSoundSystem().PlaySound("badSound");
                manager.LoseLife();
            }
        }

        //Util.Log("{0}", Input.GetAxis("Horizontal"));

        if (Input.GetAxis("Horizontal") != axisOld)
        {
            axisNew = Input.GetAxis("Horizontal");
            if(axisOld == 0)
            {
                if (axisNew > axisOld)
                {
                    selectIndex += 1;
                    if (selectIndex == 4)
                    {
                        selectIndex = 0;
                    }
                    select.parent = spawnPoints[selectIndex].transform;
                    select.position = spawnPoints[selectIndex].position;
                }
                else if (axisNew < axisOld)
                {
                    selectIndex -= 1;
                    if (selectIndex == -1)
                    {
                        selectIndex = 3;
                    }
                    select.parent = spawnPoints[selectIndex].transform;
                    select.position = spawnPoints[selectIndex].position;
                }

                Transform[] childrenTransforms = spawnPoints[selectIndex].GetComponentsInChildren<Transform>();

                int childCount = childrenTransforms.Length;

                if (childrenTransforms != null)
                {
                    for (int i = childCount - 1; i >= 0; i--)
                    {
                        if (childrenTransforms[i].gameObject.tag == "ingredient")
                        {
                            selection = i;
                        }
                    }
                }
            }
            
            axisOld = axisNew;
        }

	}
}
