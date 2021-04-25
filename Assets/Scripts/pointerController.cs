using UnityEngine;

public class pointerController : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    public static pointerController current;

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
            Debug.LogError("Doofus you moofus you have two pointers.");
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = true;
        spriteRenderer.enabled = false;
    }

    public void ReplacePointer()
    {
        Cursor.visible = !Cursor.visible;
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos = Camera.main.ScreenToWorldPoint(mousepos);
        mousepos.z = 0;
        this.transform.position = mousepos;
    }

    public void setAnim(bool playing)
    {
        animator.SetBool("Play", playing);
    }
}
