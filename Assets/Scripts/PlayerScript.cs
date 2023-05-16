using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public GameObject firePrefab, torchPrefab;
    public CarryableObject carriedObj;
    public Canvas canvas;
    public TMP_Text fooText;

    bool gameOver = false;
    [Header("Modify these for balance stuff")]
    public float moveSpeed;
    bool isCarryingObj = false;
    float interactionTime;
    public float stickToTorchTime = 2.0f, logToPileTime = 2.0f;
    public KeyCode pickupKey = KeyCode.E, interactKey = KeyCode.F;

    public AudioSource pickupSound, footsteps;

    // Start is called before the first frame update
    void Start()
    {
        canvas.GetComponent<ScoreScript>().StartScore();
        fooText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == false)
        {
            playerMovement();
            if (isCarryingObj)
                itemInteractions();
        }
    }

    void playerMovement()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        Vector3 still = new Vector3(0, 0, 0);
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

        if (movement != still)
        {
            footsteps.volume = 1;
        }
        else
        {
            footsteps.volume = 0;
        }
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
                pickupSound.clip = collision.GetComponent<CarryableObject>().pickup;
                pickupSound.Play();
            }
        }
        else if (collision.gameObject.name.Contains("campfire") && isCarryingObj && Input.GetKeyDown(pickupKey))
        {
            collision.GetComponent<lightScript>().addThingToFire(carriedObj, this);
            if (Random.Range(1,5) == 1)
            {
                GameObject.Find("GameManager").GetComponent<GMScript>().spawnCollectables(3);
            }
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
        gameOver = true;
        canvas.GetComponent<ScoreScript>().StopScore();
        fooText.gameObject.SetActive(true);
    }
}
