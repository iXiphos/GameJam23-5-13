using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    public GameObject player;
    public float speed;
    GameObject[] lightSources;
    float timer = 0f;
    float avoidancePercent = 1f;
    public int difficultyIncreaseTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        lightSources = GameObject.FindGameObjectsWithTag("LightSource");
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        foreach (GameObject source in lightSources)
        {
            float radius = source.GetComponent<lightScript>().lightRadius;
            if(Vector2.Distance(transform.position, source.transform.position) < radius * avoidancePercent)
            {
               targetDirection = -(source.transform.position - transform.position).normalized;
            } 
        }
        
        transform.position += targetDirection * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if(timer > difficultyIncreaseTime)
        {
            avoidancePercent -= 0.2f;
            speed += 0.5f;
            timer = 0f;
        }
    }

   
}
