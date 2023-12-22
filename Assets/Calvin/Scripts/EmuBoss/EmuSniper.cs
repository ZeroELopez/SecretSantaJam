using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmuSniper : MonoBehaviour
{
    enum SniperState
    {
        SniperPatrol,
        SniperAiming,
        SniperShooting
    }

    /// <summary>
    /// The current state of the Sniper.
    /// </summary>
    private SniperState currentState;

    /// <summary>
    /// The radius in which the reticle will circle around.
    /// </summary>
    [SerializeField]
    private float patrolRadius;

    /// <summary>
    /// Is the Object moving Clockwise?
    /// </summary>
    [SerializeField]
    private bool clockwiseRotation;

    /// <summary>
    /// How long it should take for the Sniper to make one full circle.
    /// </summary>
    [SerializeField]
    private float patrolSpeed;

    [SerializeField]
    private float aimTime;

    /// <summary>
    /// How long the shooting hitbox should be active
    /// </summary>
    [SerializeField]
    private float shotTime;

    /// <summary>
    /// Reference to the reticle object
    /// </summary>
    [SerializeField]
    private EmuSniperReticle reticle;

    /// <summary>
    /// Reference to the reticle object
    /// </summary>
    [SerializeField]
    private EmuSniperShot shotHitbox;

    /// <summary>
    /// How far away from the actual placement of this object should the reticle be?
    /// </summary>
    private Vector3 reticleOffset;

    /// <summary>
    /// Angle in which to calculate the position of the reticle.
    /// </summary>
    private float angle;

    public AudioClip[] gunshotSounds;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentState = SniperState.SniperPatrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case SniperState.SniperPatrol:
                {
                    if(reticle != null)
                    {
                        //Increase the angle by how fast the reticle is patrolling
                        angle += (clockwiseRotation ? patrolSpeed : -patrolSpeed) * Time.deltaTime;

                        //Clamp the angle between 0 and 360.
                        if(angle < 0)
                        {
                            angle += 360;
                        }
                        else if(angle >= 360)
                        {
                            angle -= 360;
                        }

                        //Calculate offset
                        reticleOffset.x = Mathf.Sin(angle) * patrolRadius;
                        reticleOffset.y = Mathf.Cos(angle) * patrolRadius;

                        //Assign position.
                        reticle.transform.position = transform.position + reticleOffset;
                    }
                    break;
                }
            default:
                break;
        }
    }

    /// <summary>
    /// Public facing function for the Sniper to start aiming
    /// </summary>
    public void TakeAim()
    {
        Debug.Log("aiming");

        //Change State
        currentState = SniperState.SniperAiming;

        StartCoroutine(AimRoutine());
    }

    /// <summary>
    /// Coroutine for the aiming
    /// </summary>
    private IEnumerator AimRoutine()
    {
        reticle.StartCoroutine(reticle.Blink(aimTime));
        yield return new WaitForSecondsRealtime(aimTime);

        //kick off firing Routine
        StartCoroutine(FireShot());
    }

    /// <summary>
    /// Coroutine to Fire the shot.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireShot()
    {
        Debug.Log("firing");

        //Play sound effect
        int randomIndex = Random.Range(0, gunshotSounds.Length);
        audioSource.clip = gunshotSounds[randomIndex];
        audioSource.Play();

        // Set State
        currentState = SniperState.SniperShooting;

        //Enable the Sniper Shot Hitbox
        shotHitbox.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(shotTime);

        //Disable ShotHibox
        shotHitbox.gameObject.SetActive(false);

        // Return to Patrol
        currentState = SniperState.SniperPatrol;
        Debug.Log("patrolling");
    }
}
