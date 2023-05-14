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
    // Start is called before the first frame update
    void Start()
    {
        lightStrength = 2;
        lightDuration = 5.0f;
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
                    lightDuration = 0;
                else
                    lightDuration = lightDurationPerLevel;
            }
        }
        Mathf.Clamp(lightStrength, 0, maxLightStrength);
    }

    public void addThingToFire(CarryableObject obj, PlayerScript player)
    {
        lightStrength++;
        lightDuration += obj.bonusLightDuration;

        player.notCarryingAnymore();
    }
}
