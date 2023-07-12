using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Character
{
    [Header("Player Settings")]
    [SerializeField] float jumpForce;

    [Header("Attack")]
    [SerializeField] float projectileDamage;
    [SerializeField] float projectileSpeed;
    [SerializeField] Transform firePoint;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] GameObject muzzlePrefab;
    float isFiringCooldown = .3f;
    float  isFiringTimer;


    protected override void Update()
    {
#if UNITY_EDITOR
        MouseInput();
#endif
#if UNITY_ANDROID
        TouchInput();
#endif

        Movement();
        FiringReset();
    }

    // Moves the player on the horizontal axis and control his animations
    protected override void Movement()
	{
        // Stop and return if is firing
        if (isFiringTimer > 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        base.Movement();

        // Animations
        animator.SetBool("IsMoving", horizontalDirection != 0);
        animator.SetBool("OnGround", OnGround());
    }

    // Make the player jump
    void Jump()
	{
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

    // Call the function that makes the player fall off the platform
    void FallThrough()
	{
        StartCoroutine(FallThroughRoutine(.25f));
    }

    // Makes the player fall off the platform
    IEnumerator FallThroughRoutine(float fallTime)
	{
        groundCollider.enabled = false;
        yield return new WaitForSeconds(fallTime);
        groundCollider.enabled = true;
	}

    IEnumerator Fire()
    {
        if (OnGround() && isFiringTimer <= 0)
        {
            isFiringTimer = isFiringCooldown;
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(.1f);

            Vector2 direction = horizontalDirection < 0 ? Vector2.left : Vector2.right;

            PlayerProjectile projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.Setup(direction, projectileSpeed, projectileDamage);
            Destroy(Instantiate(muzzlePrefab, firePoint.position, Quaternion.identity), .2f);
        }
    }

    void FiringReset()
	{
        isFiringTimer -= Time.deltaTime;
	}

    #region Inputs
    float swipeTolerance = .99f;
	Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    // Controls touch interactions
    void TouchInput()
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
                if (currentSwipe.y > 0 && currentSwipe.x > -swipeTolerance && currentSwipe.x < swipeTolerance)
                {
                    Jump();
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -swipeTolerance && currentSwipe.x < swipeTolerance)
                {
                    FallThrough();
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -swipeTolerance && currentSwipe.y < swipeTolerance)
                {
                    SwitchDirection(Direction.Left);
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -swipeTolerance && currentSwipe.y < swipeTolerance)
                {
                    SwitchDirection(Direction.Right);
                }
                // tap
                else
                {
                    StartCoroutine(Fire());
                }
            }
        }
    }

    // Controls mouse interactions
    void MouseInput()
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
            if (currentSwipe.y > 0 && currentSwipe.x > -swipeTolerance && currentSwipe.x < swipeTolerance)
            {
                Jump();
            }
            //swipe down
            else if (currentSwipe.y < 0 && currentSwipe.x > -swipeTolerance && currentSwipe.x < swipeTolerance)
            {
                FallThrough();
            }
            //swipe left
            else if (currentSwipe.x < 0 && currentSwipe.y > -swipeTolerance && currentSwipe.y < swipeTolerance)
            {
                SwitchDirection(Direction.Left);
            }
            //swipe right
            else if (currentSwipe.x > 0 && currentSwipe.y > -swipeTolerance && currentSwipe.y < swipeTolerance)
            {
                SwitchDirection(Direction.Right);
            }
            // tap
			else
			{
                StartCoroutine(Fire());
            }
        }
    }
	#endregion
}

public enum Direction
{
    Up, Down, Right, Left
}