using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    public GameObject player;
    public float startingSpeed, speedScaling;
    float speed;
    public List<lightScript> lightSources = new List<lightScript>();
    float timer = 0f;
    float avoidancePercent = 1f;
    public int difficultyIncreaseTime;

    [SerializeField] AudioSource scream;

    // Start is called before the first frame update
    void Start()
    {
        speed = startingSpeed;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        foreach (lightScript source in lightSources)
        {
            if (source != null)
            {
                float radius = source.lightRadius;
                radius /= 2;
                if (Vector2.Distance(transform.position, source.transform.position) < radius * avoidancePercent)
                {
                    targetDirection = -(source.transform.position - transform.position).normalized;
                }
            }
        }
        
        transform.position += targetDirection * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if(timer > difficultyIncreaseTime)
        {
            avoidancePercent -= 0.2f;
            speed += speedScaling;
            timer = 0f;

            scream.Play();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -2);
    }

   
}
