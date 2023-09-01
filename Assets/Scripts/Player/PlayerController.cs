using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Transform playerBackpack;
    public Rigidbody rb;
    public int metalItems;
    public int plasticItems;
    public int metalStockItems;
    public int metalBodyItems;
    public int plasticScopeItems;
    public int plasticMagItems;
    public PlayerMovement playerMovement;




    private bool isStack = false;


    public GameObject bagMetal;
    public GameObject bagPlastic;
    public GameObject bagBody;
    public GameObject bagScope;
    public GameObject bagMag;
    public GameObject bagStock;
    public Transform backpackBait;


    public GameObject offStock;
    public GameObject offScope;
    public GameObject offMag;
    public GameObject offBody;


    public float speed = 0;

    public List<GameObject> metalList = new List<GameObject>();
    public List<GameObject> plasticList = new List<GameObject>();
    private List<GameObject> scopeList = new List<GameObject>();
    private List<GameObject> stockList = new List<GameObject>();
    private List<GameObject> magList = new List<GameObject>();
    private List<GameObject> bodyList = new List<GameObject>();




    private GameObject lastMetalObject;
    private GameObject lastPlasticObject;
    private GameObject lastScopeObject;
    private GameObject lastStockObject;
    private GameObject lastMagObject;
    private GameObject lastBodyObject;

    public MetalDestroyer metalDestroyer;
    public MetalDestroyerBody metalDestroyerBody;
    public PlasticDestroyer plasticDestroyer;
    public PlasticDestroyerMag plasticDestroyerMag;

    public GameObject plasticInScope;
    public GameObject plasticInMag;
    public GameObject metalInBody;
    public GameObject metalInStock;
    public GameObject jumpingMetal;
    public GameObject jumpingPlastic;
    public PlayerAnimation playerAnimation;
    public LootManager lootManager;
    
    public int maxHealth = 100;
    private int currentHealth;

    public float rotationSpeed;
    public GameManager gameManager;
    public Zombie zombie;
    public bool isAlive = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public bool IsMoving()
    {
        Vector3 joystickPosition = playerMovement.joyStick.transform.localPosition;

        Vector3 inputDir = new Vector3(joystickPosition.x, 0, joystickPosition.y);
        return inputDir.magnitude > 0;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("damage");
        if (currentHealth <= 0)
        {
            isAlive = false;
            playerMovement.enabled = false;
            lootManager.restartButton.interactable = true;
            playerMovement.animator.SetTrigger("Death");
            Debug.Log("Player is dead!");
            lootManager.ShowRestartButton();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Metal"))
        {
            if (!isStack)
            {
                playerAnimation.animator.SetTrigger("Idle");
                playerMovement.enabled = false;
                isStack = true;
                other.gameObject.SetActive(false);
                GameObject newMetal = Instantiate(other.gameObject, other.gameObject.transform.position, other.gameObject.transform.rotation);
                StackingMetal(newMetal);

                jumpingMetal.SetActive(true);
                jumpingMetal.transform.position = other.gameObject.transform.position;

                Vector3 targetPosition = backpackBait.position;

                jumpingMetal.transform.DOJump(targetPosition, 1.2f, 1, 1f)
                    .OnStart(() =>
                    {  
                        RotateMetalWhileJumping(jumpingMetal);
                    })
                    .OnComplete(() =>
                    {
                        jumpingMetal.SetActive(false);
                        playerMovement.enabled = true;

                    });

                bagMetal.SetActive(true);
            }
        }
        if (other.CompareTag("Plasticout"))
        {
            if (!isStack)
            {
                playerMovement.enabled = false;
                playerAnimation.animator.SetTrigger("Idle");
                isStack = true;
                other.gameObject.SetActive(false);
                GameObject newPlastic = Instantiate(other.gameObject, other.gameObject.transform.position, other.gameObject.transform.rotation);
                StackingPlastic(newPlastic);

                jumpingPlastic.SetActive(true);
                jumpingPlastic.transform.position = other.gameObject.transform.position;

                Vector3 targetPosition = backpackBait.position;

                jumpingPlastic.transform.DOJump(targetPosition, 1f, 1, 1f)
                    .OnStart(() =>
                    {
                        RotatePlasticWhileJumping(jumpingPlastic);
                    })
                    .OnComplete(() =>
                    {
                        jumpingPlastic.SetActive(false);
                        playerMovement.enabled = true;
                    });

                bagPlastic.SetActive(true);
            }
        }
    
        if (other.CompareTag("OutScope"))
        {

                StackingScope(other.gameObject);
                other.gameObject.SetActive(false);
                bagScope.SetActive(true);
        }
        if (other.CompareTag("outMag"))
        {
            StackingMag(other.gameObject);
            other.gameObject.SetActive(false);
            bagMag.SetActive(true);

        }
        if (other.CompareTag("OutStock"))
        {
                StackingStock(other.gameObject);
                other.gameObject.SetActive(false);
                bagStock.SetActive(true);

        }
        if (other.CompareTag("OutBody"))
        {
            StackingBody(other.gameObject);
            other.gameObject.SetActive(false);
            bagBody.SetActive(true);

        }


        if (other.gameObject.CompareTag("Metalin"))
        {
            metalInStock.SetActive(false);

            if (metalItems > 0)
            {
                metalItems--;
                GameObject metalToDecrease = metalList[metalItems];
                DecreaseStackingMetal(metalToDecrease);
                metalDestroyer.OpenOffMetal();
                if (metalItems == 0)
                    bagMetal.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("MetalinBody"))
        {
            metalInBody.SetActive(false);
            if (metalItems > 0)
            {
                metalItems--;
                GameObject metalToDecrease = metalList[metalItems];
                DecreaseStackingMetal(metalToDecrease);
                metalDestroyerBody.OpenOffMetalBody();
                if (metalItems == 0)
                    bagMetal.SetActive(false);
            }

        }
        if (other.gameObject.CompareTag("Plasticin"))
        {
            plasticInScope.SetActive(false);
            if (plasticItems > 0)
            {
                plasticItems--;
                GameObject plasticToDecrease = plasticList[plasticItems];
                DecreaseStackingPlastic(plasticToDecrease);
                plasticDestroyer.OpenOffPlastic();
                if (plasticItems == 0)
                    bagPlastic.SetActive(false);
            }
        }
        if (other.gameObject.CompareTag("PlasticinMag"))
        {
            plasticInMag.SetActive(false);
            if (plasticItems > 0)
            {
                plasticItems--;
                GameObject plasticToDecrease = plasticList[plasticItems];
                DecreaseStackingPlastic(plasticToDecrease);
                plasticDestroyerMag.OpenOffPlasticMag();
                if (plasticItems == 0)
                    bagPlastic.SetActive(false);
            }
        }

   
        if (other.gameObject.CompareTag("Table"))
        {
            DecreaseStackingStock(lastStockObject);
            offStock.SetActive(true);
            bagStock.SetActive(false);
        }    
        if (other.gameObject.CompareTag("Table2"))
        {
            DecreaseStackingScope(lastScopeObject);
            offScope.SetActive(true);
            bagScope.SetActive(false);

        }
        if (other.gameObject.CompareTag("Table3"))
        {
            DecreaseStackingMag(lastMagObject);
            offMag.SetActive(true);
            bagMag.SetActive(false);


        }
        if (other.gameObject.CompareTag("Table4"))
        {
            DecreaseStackingBody(lastBodyObject);
            offBody.SetActive(true);
            bagBody.SetActive(false);

        }

    }

    public void StackingMetal(GameObject _gameObject)
    {
        lastMetalObject = _gameObject;
        
        lastMetalObject.transform.position = new Vector3(lastMetalObject.transform.position.x, lastMetalObject.transform.position.y, lastMetalObject.transform.position.z);
        lastMetalObject.transform.SetParent(playerBackpack);
        metalList.Add(lastMetalObject);
        metalItems++;
        isStack = false;
        LootManager.instance.AddMetal();
    }
    public void StackingPlastic(GameObject _gameObject)
    {
        lastPlasticObject = _gameObject;

        lastPlasticObject.transform.position = new Vector3(lastPlasticObject.transform.position.x, lastPlasticObject.transform.position.y , lastPlasticObject.transform.position.z);
        lastPlasticObject.transform.SetParent(playerBackpack);
        plasticList.Add(lastPlasticObject);
        plasticItems++;
        isStack=false;
        LootManager.instance.AddPlastic();

    }
    public void StackingScope(GameObject _gameObject)
    {
        lastScopeObject = _gameObject;
        lastScopeObject.transform.position = new Vector3(lastScopeObject.transform.position.x, lastScopeObject.transform.position.y + 1f, lastScopeObject.transform.position.z);
        lastScopeObject.transform.SetParent(transform);
        scopeList.Add(lastScopeObject);
    }
    public void StackingStock(GameObject _gameObject)
    {
        lastStockObject = _gameObject;
        lastStockObject.transform.position = new Vector3(lastStockObject.transform.position.x, lastStockObject.transform.position.y + 1f, lastStockObject.transform.position.z);
        lastStockObject.transform.SetParent(transform);
        stockList.Add(lastStockObject);
    }
    public void StackingBody(GameObject _gameObject)
    {
        lastBodyObject = _gameObject;

        lastBodyObject.transform.position = new Vector3(lastBodyObject.transform.position.x, lastBodyObject.transform.position.y + 1f, lastBodyObject.transform.position.z);
        lastBodyObject.transform.SetParent(transform);
        bodyList.Add(lastBodyObject);
    }
    public void StackingMag(GameObject _gameObject)
    {
        lastMagObject = _gameObject;
        lastMagObject.transform.position = new Vector3(lastMagObject.transform.position.x, lastMagObject.transform.position.y + 1f, lastMagObject.transform.position.z);
        lastMagObject.transform.SetParent(transform);
        magList.Add(lastMagObject);
    }

    public void DecreaseStackingPlastic(GameObject _gameObject)
    {

        if (plasticList.Contains(_gameObject))
        {
            _gameObject.transform.parent = null;
            plasticList.Remove(_gameObject);
            Destroy(_gameObject);
            LootManager.instance.DecreasePlastic();
        }
    }
    public void DecreaseStackingMetal(GameObject _gameObject)
    {
        if (metalList.Contains(_gameObject))
        {
            _gameObject.transform.parent = null;
            metalList.Remove(_gameObject);
            Destroy(_gameObject);
            LootManager.instance.DecreaseMetal();
        }
    }
    public void DecreaseStackingScope(GameObject _gameObject)
    {
        lastScopeObject = _gameObject;
        lastScopeObject.transform.parent = null;
        scopeList.Remove(_gameObject);
        Destroy(lastScopeObject);
    }
    public void DecreaseStackingMag(GameObject _gameObject)
    {
        lastMagObject = _gameObject;
        lastMagObject.transform.parent = null;
        magList.Remove(_gameObject);
        Destroy(lastMagObject);
    }
    public void DecreaseStackingStock(GameObject _gameObject)
    {
        lastStockObject = _gameObject;
        lastStockObject.transform.parent = null;
        stockList.Remove(_gameObject);
        Destroy(lastStockObject);
    }
    public void DecreaseStackingBody(GameObject _gameObject)
    {
        lastBodyObject = _gameObject;
        lastBodyObject.transform.parent = null;
        bodyList.Remove(_gameObject);
        Destroy(lastBodyObject);
    }

    private void RotateMetalWhileJumping(GameObject metal)
    {
        metal.transform.DORotate(new Vector3(0f, 360f, 0f), 0.5f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental);
    }

    private void RotatePlasticWhileJumping(GameObject plastic)
    {
        plastic.transform.DORotate(new Vector3(0f, 360f, 0f), 0.5f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental);
    }
}
