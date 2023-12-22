using Assets.Scripts.Base.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmuContra : MonoBehaviour,
    ISubscribable<onGameWon>,
    ISubscribable<onGameLost>
{
    [SerializeField]
    public float cooldownTimer;

    [SerializeField]
    public int bulletsPerSalvo;

    [SerializeField]
    public float shotDelayTimer;

    private Vector2 firingDirection;

    /// <summary>
    /// Cached reference to the player that the Emus aim at.
    /// </summary>
    private PlayerMovement playerReference;

    /// <summary>
    /// Flag for if player is in shooting range
    /// </summary>
    private bool playerInRange;

    [SerializeField]
    public GameObject StunBullet;

    [SerializeField]
    public Transform SpawnPoint;

    /// <summary>
    /// Initialize
    /// </summary>
    void Start()
    {
        playerReference = GameObject.FindFirstObjectByType<PlayerMovement>();
        Subscribe();
    }

    /// <summary>
    /// Coroutine to shoot at Player
    /// </summary>
    /// <returns></returns>
    /// 
    public UnityEvent<Vector2> onShooting;

    public IEnumerator ShootAtPlayer()
    {
        while (playerInRange)
        {
            //Wait until cooldown elapses to begin firing
            yield return new WaitForSecondsRealtime(cooldownTimer);

            //Aim at player
            //Note: this will not adjust per shot. We take the position just once and then fire a salvo.
            //      Re-Aim once we prepare a new slavo of bullets.
            Vector2 dir = (playerReference.transform.position - transform.position).normalized;
            onShooting?.Invoke(dir);

            //Spawn Bullets to travel towards player.
            for (int i = 0; i < bulletsPerSalvo; i++)
            {
                //Spawn
                if (StunBullet != null)
                {
                    GameObject bulletInstance = Instantiate(StunBullet);
                    bulletInstance.transform.position = SpawnPoint.position;
                    bulletInstance.GetComponent<StunBullet>().direction = dir;
                }

                yield return new WaitForSecondsRealtime(shotDelayTimer);
            }
        }
    }

    /// <summary>
    /// Handle colliding with the trigger hitbox. 
    /// This should be a circle hitbox that represents the range at which the emus can fire.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Process if collision is Player
        //If so, kick start the coroutine.
        if (collision.GetComponent<PlayerMovement>() == playerReference)
        {
            playerInRange = true;
            StartCoroutine(ShootAtPlayer());
        }
    }

    /// <summary>
    /// When the player exits the collider hibox for the emu,
    /// stop the fire at Coroutine.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() == playerReference)
        {
            playerInRange = false;
        }
    }

    /// <summary>
    /// Subscribe to certain events.
    /// </summary>
    public void Subscribe()
    {
        EventHub.Instance.Unsubscribe<onGameWon>(this);
        EventHub.Instance.Unsubscribe<onGameLost>(this);
    }

    /// <summary>
    /// Unsubscribe.
    /// </summary>
    public void Unsubscribe()
    {
        EventHub.Instance.Subscribe<onGameWon>(this);
        EventHub.Instance.Subscribe<onGameLost>(this);
    }

    /// <summary>
    /// Stop shooting once the game is over.
    /// </summary>
    /// <param name="evt"></param>
    public void HandleEvent(onGameLost evt)
    {
        StopCoroutine(ShootAtPlayer());
        Unsubscribe();
    }

    /// <summary>
    /// Stop shooting when the game is over.
    /// </summary>
    /// <param name="evt"></param>
    public void HandleEvent(onGameWon evt)
    {
        StopCoroutine(ShootAtPlayer());
        Unsubscribe();
    }

    void OnDestroy()
    {
        if (EventHub.Instance != null)
        {
            Unsubscribe();
        }
    }
}
