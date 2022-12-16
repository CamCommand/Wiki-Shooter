using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterController controller;

    [SerializeField] Camera playerCam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;
        playerCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
    }


    void Update()
    {
        if (inputManager.PlayerFired())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit))
        {
            FindObjectOfType<AudioManager>().PlaySound("Fire");
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
