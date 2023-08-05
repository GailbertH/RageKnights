
using System.Collections;
using UnityEngine;

public class PlayerAdventureMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody characterController;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float groundDist;
    [SerializeField]
    private LayerMask floor;

    private PlayerAdventureActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerAdventureActions();
    }

    private void Start()
    {
        if (RecordKeeperManager.Instance == null)
        {
            StartCoroutine(tempWaiter());
        }
        else
        {
            Initialize();
        }
    }
    private IEnumerator tempWaiter()
    {
        yield return new WaitUntil(() => RecordKeeperManager.Instance != null);
        Initialize();
        yield return new WaitForEndOfFrame();
    }

    private void Initialize()
    {
        inputActions.Adventure_Map.Enable();
        if (RecordKeeperManager.Instance.playerPosition != Vector3.zero)
        {
            transform.position = RecordKeeperManager.Instance.playerPosition;
            RecordKeeperManager.Instance.playerPosition = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        inputActions.Adventure_Map.Disable();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        Vector3 direction = -transform.up;
        direction.y += -1.5f;
        //Debug.DrawRay(castPos, direction, Color.yellow);
        if (Physics.Raycast(castPos, direction, out hit, Mathf.Infinity, floor))
        {
            if (hit.collider != null)
            {
                Debug.Log("Did Hit");
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
        Vector2 moveInput = new Vector2();
        Vector3 movement = new Vector3();
#if UNITY_ANDROID
        if (AdventureUIManager.Instance != null)
        {
            moveInput = AdventureUIManager.Instance.GetMovement;
        }
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        moveInput = inputActions.Adventure_Map.Movement.ReadValue<Vector2>();
        #endif

        movement = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAdventureController enemyAdvControllers = null;
        other.TryGetComponent<EnemyAdventureController>(out enemyAdvControllers);
        if (enemyAdvControllers != null)
        { 
            RecordKeeperManager.Instance.collideEnemyId = enemyAdvControllers.adventureId;
            RecordKeeperManager.Instance.playerPosition = transform.position;
            SceneTransitionManager.Instance.StartTransition(TransitionKey.ADVENTURE_TO_COMBAT);
        }
    }
}
