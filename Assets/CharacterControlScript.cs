using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
    [SerializeField] private float rangeToNode = 0.1f;
    [SerializeField] private Node currentNode;
    [SerializeField] private float speed = 2f;

    public static CharacterControlScript current;

    [SerializeField]
    private Animator ani;

    private Interactable targetInteractable;
    List<Node> walkingRoute;
    bool isDoingSomething = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Debug.LogError("Doofus you moofus you have two players.");
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        this.transform.position = currentNode.transform.position;
    }

    public void BeginInteraction(Interactable toTouch)
    {
        if (isDoingSomething)
            return;

        targetInteractable = toTouch;
        isDoingSomething = true;

        // Walk to the route
        DoWalk(targetInteractable.getClosestNode());
    }

    public void DoWalk(Node target)
    {
        // Walk to the route
        walkingRoute = Node.findPath(currentNode, target);
        StartCoroutine("StartCreeping");
    }

    IEnumerator StartCreeping()
    {
        SoundController.current.PlayWalkSound();
        ani.SetBool("Walking", true);
        for (int i = 0; i < walkingRoute.Count; i++)
        {
            Node targetNode = walkingRoute[i];
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 myTarget = new Vector2(targetNode.transform.position.x, targetNode.transform.position.y);

            while (Vector2.Distance(myPos, myTarget) > rangeToNode)
            {
                float oldX = myPos.x;
                // creep up on it
                float step = speed * Time.deltaTime;

                // move sprite towards the target location
                transform.position = Vector2.MoveTowards(myPos, myTarget, step);
                float newX = transform.position.x;

                if (newX > oldX)
                {
                    faceRight();
                }
                else
                {
                    faceLeft();
                }

                myPos = new Vector2(transform.position.x, transform.position.y);
                yield return null;
            }
        }
        SoundController.current.StopWalkSound();
        ani.SetBool("Walking", false);

        currentNode = walkingRoute[walkingRoute.Count - 1];

        if (targetInteractable != null)
        {
            // make them face the object
            if (transform.position.x > targetInteractable.transform.position.x)
            {
                faceLeft();
            }
            else
            {
                faceRight();
            }

            // call the interact now we're there
            ani.SetBool("Interact", true);
            targetInteractable.DoInteract();
            ani.SetBool("Interact", false);

        }
        isDoingSomething = false;
    }

    public void faceRight()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void faceLeft()
    {
        this.transform.localScale = new Vector3(-1, 1, 1);
    }
}
