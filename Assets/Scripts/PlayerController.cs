using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    float horizontalInput;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        MouseSwipe();
#endif
#if UNITY_ANDROID
        TouchSwipe();
#endif

        Movement();
    }

    void Movement()
	{
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (rb.velocity.x < 0) spriteRenderer.flipX = true;
        else if (rb.velocity.x > 0) spriteRenderer.flipX = false;

        animator.SetBool("IsMoving", horizontalInput != 0);
        animator.SetBool("OnAir", OnAir());
    }

    void Jump()
	{
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

    void FallThrough()
	{

    }

    bool OnAir()
	{
        return !Physics2D.Raycast(transform.position, -transform.up, .1f, LayerMask.GetMask("Ground"));
	}

    void SwitchDirection(Direction direction)
	{
        horizontalInput = direction == Direction.Right ? 1 : -1;

    }

    //inside class
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    public void TouchSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Jump();
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    FallThrough();
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    SwitchDirection(Direction.Left);
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    SwitchDirection(Direction.Right);
                }
            }
        }
    }

    public void MouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Jump();
            }
            //swipe down
            else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                FallThrough();
            }
            //swipe left
            else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                SwitchDirection(Direction.Left);
            }
            //swipe right
            else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                SwitchDirection(Direction.Right);
            }
        }
    }
}

public enum Direction
{
    Up, Down, Right, Left
}