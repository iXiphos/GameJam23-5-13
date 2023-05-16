using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerScript : MonoBehaviour
{
    public GameObject firePrefab, torchPrefab;
    public CarryableObject carriedObj;

    [Header("Modify these for balance stuff")]
    public float moveSpeed;
    bool isCarryingObj = false;
    float interactionTime;
    public float stickToTorchTime = 2.0f, logToPileTime = 2.0f;
    public KeyCode pickupKey = KeyCode.E, interactKey = KeyCode.F;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        if(isCarryingObj)
            itemInteractions();
    }

    void playerMovement()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            movement.y += 1;
        if (Input.GetKey(KeyCode.S))
            movement.y -= 1;
        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;
        if (Input.GetKey(KeyCode.D))
            movement.x += 1;

        if(isCarryingObj)
            transform.position += (movement * moveSpeed * Time.deltaTime * carriedObj.carriedSpeedMult);
        else
        transform.position += (movement * moveSpeed * Time.deltaTime);
    }

    void itemInteractions()
    {
        //checks first 3 characters of obj name
        //prefabs always have (clone) appended to them
        switch(carriedObj.name.Substring(0,3))
        {
            case "twi":
                {
                    if (Input.GetKey(interactKey))
                    {
                        //nothing special, destroy it i guess
                        notCarryingAnymore();
                    }
                    break;
                }
            case "sti":
                {
                    if (Input.GetKey(interactKey))
                    {
                        interactionTime += Time.deltaTime;
                    }
                    else
                        interactionTime = 0;

                    if(interactionTime >= stickToTorchTime)
                    {
                        GameObject torch = Instantiate(torchPrefab, transform.position + new Vector3(0.6f,0,0), Quaternion.identity);
                        torch.transform.parent = transform;
                        //add torch duration

                        notCarryingAnymore();
                    }
                    break;
                }
            case "log":
                {
                    if (Input.GetKey(interactKey))
                    {
                        interactionTime += Time.deltaTime;
                    }
                    else
                        interactionTime = 0;

                    if (interactionTime >= logToPileTime)
                    {
                        GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
                        // set fire's light duration

                        notCarryingAnymore();
                    }
                    break;
                }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CarryableObject>() != null)
        {
            if (Input.GetKey(pickupKey) && !isCarryingObj)
            {
                carriedObj = collision.gameObject.GetComponent<CarryableObject>();
                isCarryingObj = true;
                collision.gameObject.transform.parent = gameObject.transform;
                collision.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (collision.gameObject.name.Contains("campfire") && isCarryingObj && Input.GetKeyDown(pickupKey))
        {
            collision.GetComponent<lightScript>().addThingToFire(carriedObj, this);
        }
    }

    public void notCarryingAnymore()
    {
        Destroy(carriedObj.gameObject);
        carriedObj = null;
        isCarryingObj = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("Monster"))
        {
            EndGame();
        }
    }

    void EndGame()
    {

    }
}
