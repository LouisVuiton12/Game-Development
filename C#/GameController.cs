using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Array of pickup objects
    public GameObject[] pickups;

    // Game state variables
    private int numPickups;
    private int count;

    // Player reference
    private PlayerController player;

    // Text fields
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI closestPickupText;

    // Line renderer for debug
    private LineRenderer lineRenderer;

    // Initialize references and game state variables
    public float moveSpeed = 10.0f; // the speed at which the ball moves
    public float startForce = 1.0f; // the initial force applied to the ball to start it rolling

    private Rigidbody rb;

    void FixedUpdate()
    {
        // get user input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // apply force to the ball based on user input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * moveSpeed);

        // apply a small force to start the ball rolling if it's not already moving
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.AddForce(transform.forward * startForce, ForceMode.Impulse);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        numPickups = pickups.Length;
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        // Add line renderer for debug
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update the player's position and velocity text
    public void UpdatePositionText(string text)
    {
        Vector3 velocity = player.GetComponent<Rigidbody>().velocity;
        float speed = velocity.magnitude;
        positionText.text = text + "\nSpeed: " + speed.ToString("0.00") + " units/sec";
    }

    // Update the number of pickups collected and check for win condition
    public void UpdateCount()
    {
        count++;
        SetCountText();

        if (count >= numPickups)
        {
            winTextObject.SetActive(true);
        }
    }

    // Update the count text field
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    // Update the speed and distance to closest pickup text fields
    void UpdateUI()
    {
        // Update speed text
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;
        speedText.text = "Speed: " + speed.ToString("F1") + " m/s";

        // Find the closest pickup
        float minDist = Mathf.Infinity;
        GameObject closestPickup = null;
        foreach (GameObject pickup in pickups)
        {
            if (pickup.activeSelf)
            {
                float dist = Vector3.Distance(player.transform.position, pickup.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPickup = pickup;
                }
            }
        }

        // Update closest pickup text
        if (closestPickup != null)
        {
            closestPickupText.text = "Distance to closest pickup: " + minDist.ToString("F1") + " m";
        }
        else
        {
            closestPickupText.text = "";
        }

        // Render a line to the closest pickup
        if (closestPickup != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, closestPickup.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    // Update the UI every frame
    void Update()
    {
        UpdateUI();
    }
}
