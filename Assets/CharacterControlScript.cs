using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
    [SerializeField] private float rangeToNode = 0.1f;
    [SerializeField] private Node currentNode;
    [SerializeField] private float speed = 0.15f;

    public static CharacterControlScript current;

    private Interactable targetInteractable;
    List<Node> walkingRoute;
    bool isDoingSomething;

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
        walkingRoute = Node.findPath(currentNode, targetInteractable.getClosestNode());

        // LOGIC
        this.transform.position = targetInteractable.getClosestNode().transform.position;

        // call the interact
        targetInteractable.DoInteract();
        currentNode = targetInteractable.getClosestNode();
        isDoingSomething = false;
    }

}
