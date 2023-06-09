using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightScript : MonoBehaviour
{
    [Tooltip("How strong the light is")]
    public int lightStrength;
    [Tooltip("How long the current light level will last")]
    public float lightDuration;
    [Tooltip("When the light level decreases, how long will it last at that level")]
    public float lightDurationPerLevel;
    [Tooltip("Max strength of the light")]
    public int maxLightStrength;
    [Tooltip("The radius of the light")]
    public float lightRadius = 0f;
    [Tooltip("The prefab that is spawned to show light")]
    public GameObject lightPrefab;
    [HideInInspector]
    [Tooltip("The object that is spawned and resized")]
    public GameObject lightObj;

    public bool destroyWhenDone = true;

    [Tooltip("For controlling the sounds")]
    public AudioSource fireAdd, burnSound;

    // Start is called before the first frame update
    void Start()
    {
        lightDuration = lightDurationPerLevel;
        lightRadius = lightStrength * 2f;
        lightObj = Instantiate(lightPrefab, gameObject.transform.position + new Vector3(0,0,-1), Quaternion.identity);
        lightObj.transform.parent = gameObject.transform;
        GameObject.Find("Monster").GetComponent<MonsterScript>().lightSources.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(lightStrength > 0)
        {
            if (lightDuration > 0)
                lightDuration -= Time.deltaTime;
            else
            {
                lightStrength--;
                if (lightStrength == 0)
                {
                    lightDuration = 0;
                    if (destroyWhenDone)
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                    lightDuration = lightDurationPerLevel;
            }
        }
        lightStrength = Mathf.Clamp(lightStrength, 0, maxLightStrength);

        lightRadius = lightStrength * 3f;
        lightObj.transform.localScale = new Vector3(lightRadius, lightRadius, 0);
        lightObj.GetComponent<Light>().range = lightRadius*.5f;
    }

    public void addThingToFire(CarryableObject obj, PlayerScript player)
    {
        lightDuration += obj.bonusLightDuration;
        lightStrength += 2;
        fireAdd.clip = obj.GetComponent<CarryableObject>().sound;
        fireAdd.Play();
        burnSound.Play();

        player.notCarryingAnymore();
    }

    private void OnDestroy()
    {
        
    }

}
