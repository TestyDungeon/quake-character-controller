using UnityEngine;

public class QuakeCharacterController : MonoBehaviour
{
    private CharacterController cc;
    [Header("Stats")]
    [SerializeField] private float MAX_SPEED = 0.5f;
    [SerializeField] private float speed = 0.35f;
    [SerializeField] private float accel = 11f;
    [SerializeField] private float airMaxSpeed = 0.1f;
    [SerializeField] private float airAccel = 11f;
    [SerializeField] private float friction = 5f;
    [SerializeField] private float stopSpeed = 0.1f;
    [SerializeField] private float slopeSticking = 0.4f;
    [SerializeField] private float jumpStrength = 0.35f;
    [SerializeField] private float gravity = 30f;
    [SerializeField] private LayerMask layerMask = ~0;
    [Header("Debug")]
    [SerializeField] private bool debugGUI = false;

    private bool jumped = false;
    private Vector3 playerVelocity = Vector3.zero;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        AirMove();
        JumpButton();
    }


    private void GroundMove()
    {
        cc.Move(playerVelocity * Time.deltaTime);

        float traceDistance = cc.height/2 + slopeSticking;
    
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, traceDistance, layerMask))
        {
            if (hit.normal.y >= 0.7f)
            {
                float dropDistance = hit.distance;
                cc.Move(Vector3.down * dropDistance);
            }
        }

    }

    private void AirMove()
    {
        Vector3 wishdir;
        Vector3 wishvel = new Vector3();
        float wishspeed;

        Vector3 forward;
        Vector3 right;

        float fmove, smove;

        forward = transform.forward;
        right = transform.right;

        fmove = Input.GetAxisRaw("Horizontal");
        smove = Input.GetAxisRaw("Vertical");

        Vector3.Normalize(forward);
        Vector3.Normalize(right);

        for (int i = 0; i < 3; i++)
            wishvel[i] = forward[i] * smove + right[i] * fmove;

        wishdir = wishvel;
        wishspeed = wishdir.magnitude * speed;
        Vector3.Normalize(wishdir);

        if (wishspeed > MAX_SPEED)
        {
            VectorScale(wishvel, MAX_SPEED / wishspeed, wishvel);
            wishspeed = MAX_SPEED;
        }
        
        if (GroundCheck() && !jumped)
        {
            Friction();
            Accelerate(wishdir, wishspeed);
            GroundMove();
        }
        else
        {
            
            AirAccelerate(wishdir, wishspeed);
            playerVelocity[1] -= gravity * Time.deltaTime;
            cc.Move(playerVelocity * Time.deltaTime);
        }
    }

    private void Accelerate(Vector3 wishDir, float wishSpeed)
    {
        float currentSpeed, addSpeed, accelSpeed;

        currentSpeed = Vector3.Dot(playerVelocity, wishDir);
        addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return;

        accelSpeed = accel * Time.deltaTime * wishSpeed;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        for (int i = 0; i < 3; i++)
            playerVelocity[i] += wishDir[i] * accelSpeed;
    }

    private void AirAccelerate(Vector3 wishDir, float wishSpeed)
    {
        float wishSpd = wishSpeed;

        if (wishSpd > airMaxSpeed)
            wishSpd = airMaxSpeed;

        float currentSpeed = Vector3.Dot(playerVelocity, wishDir);
        float addSpeed = wishSpd - currentSpeed;

        if (addSpeed <= 0)
            return;

        float accelSpeed = airAccel * Time.deltaTime * wishSpeed;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        for (int i = 0; i < 3; i++)
            playerVelocity[i] += wishDir[i] * accelSpeed;
    }

    private void Friction()
    {
        float control, drop, newspeed;

        float speed = playerVelocity.magnitude;

        if (speed < 0.01)
        {
            playerVelocity = Vector3.zero;
            return;
        }

        drop = 0;

        if (GroundCheck())
        {
            control = speed < stopSpeed ? stopSpeed : speed;
            drop += control * friction * Time.deltaTime;
        }

        newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        newspeed /= speed;

        playerVelocity[0] *= newspeed;
        playerVelocity[1] *= newspeed;
        playerVelocity[2] *= newspeed;
    }

    private void JumpButton()
    {
        if (!GroundCheck())
        {
            return;
        }
        else
        {
            jumped = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
            playerVelocity -= Vector3.Project(playerVelocity, transform.up);
            playerVelocity += transform.up * jumpStrength;
        }
    }

    public bool GroundCheck()
    {
        if (Physics.SphereCast(transform.position, cc.radius, -transform.up, out RaycastHit hit, cc.height*0.3f, layerMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        GUI.color = Color.green;
        var ups = playerVelocity;
        if (debugGUI)
        {
            GUI.Label(new Rect(0, 15, 400, 100),
            "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups\n" +
            "Velocity: " + ups + "\n" +
            "Grounded: " + GroundCheck());    
        }
        
    }

    public static void VectorScale(Vector3 in_, float scale, Vector3 out_)
    {
        out_[0] = in_[0] * scale;
        out_[1] = in_[1] * scale;
        out_[2] = in_[2] * scale;
    }

}
