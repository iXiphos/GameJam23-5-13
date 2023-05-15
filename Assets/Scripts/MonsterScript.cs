using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    public GameObject player;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        transform.position += targetDirection * speed * Time.deltaTime;
    }


}
