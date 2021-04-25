using UnityEngine;
using System;

public class GameStateMachine : MonoBehaviour
{
    public enum StateOptions
    {
        START,
        VASE_BROKEN, // Interact with Vase
        BROOM_COLLECTED, // Interact with broom
        VASE_SWEEPED, // Interact with broken pieces
        EXIT_1_ATTEMPT, // Interact with the exit (painting falls, fire starts)
        FIRE_EXTINGUISHER_PICKUP, // Interact with the FE
        FIRE_OUT, // Interact with the fire
        TAPESTRY_COVERUP, // Interacting with the tapestry
        END  /// Interacting with the exit
    }

    private static StateOptions currentState = StateOptions.START;

    public static GameStateMachine current = null;

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
            Debug.LogError("Doofus you moofus you have two status machinusses.");
        }
    }

    public event Action<StateOptions> onStatePushed;

    public StateOptions GetState()
    {
        return currentState;
    }

    public void PushState()
    {
        currentState++;
        onStatePushed(currentState);
    }
}
