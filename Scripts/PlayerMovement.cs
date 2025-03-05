using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool wasJustClicked = true;
    private bool canMove;
    private Vector2 playerSize;

    private Rigidbody2D rb;
    public Transform BoundaryHolder;

    private Boundary playerboundary;

    struct Boundary
    {
        public float Up, Down, Left, Right;
        public Boundary(float up, float down, float left, float right)
        {
            Up = up; Down = down; Left = left; Right = right;
        }
    }

    // Use this for initialization
    void Start()
{
    playerSize = GetComponent<SpriteRenderer>().bounds.extents;
    rb = GetComponent<Rigidbody2D>();

    // Try to find BoundaryHolder in the scene if not assigned
    if (BoundaryHolder == null)
    {
        BoundaryHolder = GameObject.Find("BoundaryHolder")?.transform;
        if (BoundaryHolder == null)
        {
            Debug.LogError("BoundaryHolder is missing! Assign it in the Inspector.");
            return;
        }
    }

    playerboundary = new Boundary(
    Mathf.Max(BoundaryHolder.GetChild(0).position.y, BoundaryHolder.GetChild(1).position.y), // Up (Max Y)
    Mathf.Min(BoundaryHolder.GetChild(0).position.y, BoundaryHolder.GetChild(1).position.y), // Down (Min Y)
    Mathf.Min(BoundaryHolder.GetChild(2).position.x, BoundaryHolder.GetChild(3).position.x), // Left (Min X)
    Mathf.Max(BoundaryHolder.GetChild(2).position.x, BoundaryHolder.GetChild(3).position.x)  // Right (Max X)
);

}


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (wasJustClicked)
            {
                wasJustClicked = false;

                if ((mousePos.x >= transform.position.x && mousePos.x < transform.position.x + playerSize.x ||
                     mousePos.x <= transform.position.x && mousePos.x > transform.position.x - playerSize.x) &&
                    (mousePos.y >= transform.position.y && mousePos.y < transform.position.y + playerSize.y ||
                     mousePos.y <= transform.position.y && mousePos.y > transform.position.y - playerSize.y))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                // Properly clamp mouse position within boundaries
               Vector2 clampedMousePos = new Vector2(
                    Mathf.Clamp(mousePos.x, playerboundary.Left, playerboundary.Right),
                    Mathf.Clamp(mousePos.y, playerboundary.Down, playerboundary.Up)
);


                rb.MovePosition(clampedMousePos);
            }
        }
        else
        {
            wasJustClicked = true;
        }
    }
}
