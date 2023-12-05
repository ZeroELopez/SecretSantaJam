using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseScript : MonoBehaviour
{
    [Tooltip("The interval the player will drop breadcrumbs in seconds")]
    [SerializeField] float breadcrumbDropInterval;
    [Tooltip("The speed the player will be chased")]
    [SerializeField] float speed;
    [Tooltip("The acceleration due to gravity")]
    [SerializeField] float gravityAcceleration;

    public GameObject breadcrumbPrefab;

    private PlayerMovement player;
    private float time;
    private List<GameObject> breadcrumbPath;
    private float adjustedSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        time = 0;
        breadcrumbPath = new List<GameObject>();
        adjustedSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= breadcrumbDropInterval) 
        {
            time = 0;
            GameObject breadcrumb = Instantiate(breadcrumbPrefab, player.transform.position, player.transform.rotation);
            breadcrumbPath.Add(breadcrumb);
        }
        MoveToNextBreadcrumb();
    }

    void MoveToNextBreadcrumb() 
    {
        if (breadcrumbPath.Count > 0)
        {
            GameObject breadCrumb = breadcrumbPath[0];
            Vector2 vector = (breadCrumb.transform.position - transform.position);
            if (vector.magnitude < 0.2)
            {
                breadcrumbPath.RemoveAt(0);
                Destroy(breadCrumb);
                MoveToNextBreadcrumb();
            }
            else
            {
                MoveTowardVector(vector);
            }
        }
        else 
        {
            Vector2 vector = (player.transform.position - transform.position);
            MoveTowardVector(vector);
        }
    }
    void MoveTowardVector(Vector2 vector) 
    {
        Vector2 direction = vector.normalized;
        if (direction.y >= 0)
        {
            adjustedSpeed = speed;
        }
        else
        {
            adjustedSpeed = adjustedSpeed + (-direction.y * Time.deltaTime * gravityAcceleration);
        }
        transform.Translate(direction * Time.deltaTime * adjustedSpeed);
    }
}
