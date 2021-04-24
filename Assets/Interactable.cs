using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Current State where interacting with this object will  cause the state to get pushed forwards.
    [SerializeField] private GameStateMachine.StateOptions stateToPushTheState;
    [SerializeField] private string defaultResponse = "REPLACE ME DOOFUS";

    private Animator animatorionar;

    [SerializeField] private Node closestNode;

    [SerializeField] private UnityEvent preInteractionFunctions;
    [SerializeField] private UnityEvent postInteractionFunctions;


    private GameStateMachine.StateOptions currentState;

    bool isInteractable = true;
    [SerializeField]
    private bool hasAction = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameStateMachine.current.onStatePushed += onStateUp;
        currentState = GameStateMachine.current.GetState();
        animatorionar = GetComponent<Animator>();
    }

    public Node getClosestNode()
    {
        return closestNode;
    }

    public void DoInteract()
    {
        if (!isInteractable)
            return;

        if (hasAction && currentState == stateToPushTheState)
        {
            preInteractionFunctions.Invoke();
            Debug.Log("DOING SOMETHING");
            // Do something special
            // Push state forwards
            StartAnimation();
            GameStateMachine.current.PushState();
        }
        else
        {
            // Default response
            Debug.Log("DR: " + defaultResponse);
        }
    }

    public void onStateUp(GameStateMachine.StateOptions newState)
    {
        currentState = newState;
    }

    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver()
    {
        // TODO show me the object shiny yeaaahhhhhh
        pointerController.current.setAnim(true);
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        // TODO don't show me the shiny oh yeah
        pointerController.current.setAnim(false);
    }

    /// OnMouseUpAsButton is only called when the mouse is released over
    /// the same GUIElement or Collider as it was pressed.
    /// </summary>
    void OnMouseUpAsButton()
    {
        CharacterControlScript.current.BeginInteraction(this);
    }

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, closestNode.transform.position);
    }

    public void EndOfInteraction()
    {
        postInteractionFunctions.Invoke();
    }

    public void DestoryMeDaddy()
    {
        Debug.LogWarning("GOODBYE " + this.name);
        transform.gameObject.SetActive(false);
        //GameObject.Destroy(this);
    }

    public void HelloThere()
    {
        Debug.LogWarning("HELLO " + this.name);
        transform.gameObject.SetActive(true);
        //GameObject.Destroy(this);
    }

    public void StartAnimation()
    {
        if (animatorionar != null)
        {
            animatorionar.SetBool("Go", true);
        }
    }
}
