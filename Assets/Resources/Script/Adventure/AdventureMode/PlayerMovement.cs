using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody characterController;
    [SerializeField]
    private float speed;

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
        Vector2 moveInput = inputActions.Adventure_Map.Movement.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAdvController enemyAdvControllers = null;
        other.TryGetComponent<EnemyAdvController>(out enemyAdvControllers);
        if (enemyAdvControllers != null)
        { 
            RecordKeeperManager.Instance.collideEnemyId = enemyAdvControllers.adventureId;
            RecordKeeperManager.Instance.playerPosition = transform.position;
            SceneTransitionManager.Instance.StartTransition(TransitionKey.ADVENTURE_TO_COMBAT);
        }
    }
}
