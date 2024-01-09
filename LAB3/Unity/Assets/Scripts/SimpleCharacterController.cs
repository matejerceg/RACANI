using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    public float movementSpeed = 6.0f;
    public float rotationSpeed = 2.0f;
    public float detectionRadius = 3f;
    public float detectionRadiusVerticalOffset = 1f;

    public Vector2 turn;
    public float sensitivity = .5f;
    public Vector3 deltaMove;
    public float speed = 1;

    private CharacterController characterController;
    public Transform playerCamera;
    public TreeLogManager treeLogManager;

    private Dictionary<string, bool> buildObjects = new Dictionary<string, bool>();

    bool canBuild = false;
    public GameObject campfire;
    public GameObject bench;
    public GameObject table;
    public GameObject carriage;
    public GameObject cabin;
    private int activeObject = 0;

    private SFX sfx;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        buildObjects["campfire"] = false;
        buildObjects["bench"] = false;
        buildObjects["table"] = false;
        buildObjects["carriage"] = false;
        buildObjects["cabin"] = false;

    }

    private void Update()
    {
        RotateCharacter();
        MoveCharacter();
        CheckTreeCollision();
        Build();
        Debug.Log(canBuild);
        GetTreeLogs();

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        movement = transform.TransformDirection(movement);
        movement *= movementSpeed;

        characterController.Move(movement * Time.deltaTime);
    }

    private void RotateCharacter()
    {
        if (Input.GetMouseButton(1))
        {
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;
            transform.localRotation = Quaternion.Euler(0, turn.x, 0);

            turn.y = Mathf.Clamp(turn.y, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(-turn.y, 0, 0);
        }
    }

    public void CheckTreeCollision()
    {
        if (Physics.CheckSphere(transform.position, detectionRadius))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, detectionRadiusVerticalOffset, 0), detectionRadius);

            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Tree"))
                {
                    DealDamage2(collider);
                }
            }
        }
    }

    public void DealDamage2(Collider collider)
    {
        Debug.Log("Near Tree: " + collider.gameObject.name);
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, detectionRadiusVerticalOffset, 0), detectionRadius);
    }*/

    public void Build()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(canBuild)
            {
                GameObject SFX = GameObject.Find("ObjectSFX");
                sfx = SFX.GetComponent<SFX>();

                if (activeObject == 1)
                {
                    if (treeLogManager.logCount >= 10)
                    {
                        campfire.SetActive(true);
                        Transform parentTransform = campfire.transform.parent;
                        Transform secondChild = parentTransform.GetChild(1);
                        secondChild.gameObject.SetActive(false);

                        treeLogManager.logCount -= 10;
                        treeLogManager.showBuildInfo(false, "", 0);
                        canBuild = false;
                        buildObjects["campfire"] = true;

                        sfx.PlayBuildSound();
                    }
                }
                else if (activeObject == 2)
                {
                    if (treeLogManager.logCount >= 20)
                    {
                        bench.SetActive(true);
                        Transform parentTransform = bench.transform.parent;
                        Transform secondChild = parentTransform.GetChild(1);
                        secondChild.gameObject.SetActive(false);

                        treeLogManager.logCount -= 20;
                        treeLogManager.showBuildInfo(false, "", 0);
                        canBuild = false;
                        buildObjects["bench"] = true;

                        sfx.PlayBuildSound();
                    }
                }
                else if (activeObject == 3)
                {
                    if (treeLogManager.logCount >= 50)
                    {
                        table.SetActive(true);
                        Transform parentTransform = table.transform.parent;
                        Transform secondChild = parentTransform.GetChild(1);
                        secondChild.gameObject.SetActive(false);

                        treeLogManager.logCount -= 50;
                        treeLogManager.showBuildInfo(false, "", 0);
                        canBuild = false;
                        buildObjects["table"] = true;

                        sfx.PlayBuildSound();
                    }
                }
                else if (activeObject == 4)
                {
                    if (treeLogManager.logCount >= 100)
                    {
                        carriage.SetActive(true);
                        Transform parentTransform = carriage.transform.parent;
                        Transform secondChild = parentTransform.GetChild(1);
                        secondChild.gameObject.SetActive(false);

                        treeLogManager.logCount -= 100;
                        treeLogManager.showBuildInfo(false, "", 0);
                        canBuild = false;
                        buildObjects["carriage"] = true;

                        sfx.PlayBuildSound();
                    }
                }
                else if (activeObject == 5)
                {
                    if (treeLogManager.logCount >= 200)
                    {
                        cabin.SetActive(true);
                        Transform parentTransform = cabin.transform.parent;
                        Transform secondChild = parentTransform.GetChild(1);
                        secondChild.gameObject.SetActive(false);

                        treeLogManager.logCount -= 200;
                        treeLogManager.showBuildInfo(false, "", 0);
                        canBuild = false;
                        buildObjects["cabin"] = true;

                        sfx.PlayBuildSound();
                    }
                }

            }
        }
    }

    public void GetTreeLogs()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            treeLogManager.logCount += 100;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TreeLog"))
        {
            Debug.Log("collected");
            treeLogManager.logCount++;
            Destroy(other.gameObject);
        }

        
        if (other.CompareTag("BuildSignCampfire"))
        {
            if(!buildObjects["campfire"])
            {
                canBuild = true;
                treeLogManager.showBuildInfo(true, "campfire", 10);
                activeObject = 1;
            }
        }
        if (other.CompareTag("BuildSignBench"))
        {
            if (!buildObjects["bench"])
            {
                canBuild = true;
                treeLogManager.showBuildInfo(true, "bench", 20);
                activeObject = 2;
            }
        }
        if (other.CompareTag("BuildSignTable"))
        {
            if (!buildObjects["table"])
            {
                canBuild = true;
                treeLogManager.showBuildInfo(true, "table", 50);
                activeObject = 3;
            }
        }
        if (other.CompareTag("BuildSignCarriage"))
        {
            if (!buildObjects["carriage"])
            {
                canBuild = true;
                treeLogManager.showBuildInfo(true, "carriage", 100);
                activeObject = 4;
            }
        }
        if (other.CompareTag("BuildSignCabin"))
        {
            if (!buildObjects["cabin"])
            {
                canBuild = true;
                treeLogManager.showBuildInfo(true, "cabin", 200);
                activeObject = 5;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuildSignCampfire") || other.CompareTag("BuildSignBench") || other.CompareTag("BuildSignTable") || other.CompareTag("BuildSignCarriage") || other.CompareTag("BuildSignCabin"))
        {
            canBuild = false;
            treeLogManager.showBuildInfo(false, "", 0);
        }
    }
}
