using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerControllerA : MonoBehaviour
{
    public static GameObject instance;

    [Header("PlayerState")]
    const string IDLE = "Idle";
    const string WALK = "Walk";
    const string HIT = "Hit";
    const string SKILL = "Skill";

    const string LAND = "Land";
    const string JUMP = "Jump";
    const string FALL = "Fall";
    const string DASH = "Dash";
    const string ATTACK = "Attack";
    const string BLOCK = "Block";
    const string DEAD = "Dead";

    public string PlayerState = LAND;

    [Header("References")]
    [SerializeField] private PhysicsMaterial2D playerPhysics;
    [SerializeField] private ParticleSystem dustPS;
    [SerializeField] private ParticleSystem bloodPS;
    [SerializeField] private ParticleSystem healPS;
    [SerializeField] private ParticleSystem manaPS;
    [SerializeField] private Image[] skillIcon;

    [Header("Prefabs")]
    [SerializeField] private GameObject lightningCloud;

    [Header("Variables")]
    [SerializeField] private bool attackToResolve = false;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxMP;
    [SerializeField] private float currHP;
    [SerializeField] private float currMP;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float skill1Cooldown;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider mpSlider;

    [Header("Audio")]
    public AudioSource[] audSrc;
    public List<AudioClip> clips;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator ar;
    private TrailRenderer tr;

    private float dirH = 0.0f;
    private bool isWallTouch;
    private bool isSliding;
    private bool wallJumping;
    private bool canDash = true;
    private bool dashCDStart = false;
    private bool canUseSkill1 = true;
    private bool skillCDStart = false;
    private bool enemyInfront = false;
    [HideInInspector] public bool isHealthMax = true, isManaMax = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ar = GetComponent<Animator>();
        tr = GetComponentInChildren<TrailRenderer>();

        audSrc = GetComponentsInChildren<AudioSource>();

        baseMoveSpeed = moveSpeed;

        hpSlider.maxValue = maxHP;
        mpSlider.maxValue = maxMP;

        currHP = maxHP;
        currMP = maxMP;

        hpSlider.value = currHP;
        mpSlider.value = currMP;

        for (int i = 0; i < skillIcon.Length; i++)
        {
            skillIcon[i].fillAmount = 1.0f;
        }
    }

    void Update()
    {
        Movement();

        Jump();

        Action();

        if (isWallTouch && PlayerState != LAND && dirH != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        // Sets player's 2D physics material.
        if (PlayerState == JUMP || PlayerState == FALL)
        {
            playerPhysics.friction = 0.0f;
            rb.sharedMaterial = playerPhysics;
        }
        else if (PlayerState == LAND)
        {
            playerPhysics.friction = 0.4f;
            rb.sharedMaterial = playerPhysics;
        }
    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (wallJumping)
        {
            rb.velocity = new Vector2(-dirH * wallJumpForce.x, wallJumpForce.y);
        }
        else if (PlayerState != DASH)
        {
            rb.velocity = new Vector2(dirH * moveSpeed, rb.velocity.y);
        }
    }

    private void LateUpdate()
    {
        UpdateAnimation();

        UpdatePlayerDirection();

        UpdateHPMPBar();

        CheckEnemyExist();

        CheatKeybinds();
    }

    void Movement()
    {
        // Get Horizontal inputs from the player if it isn't in Attack, Block or Dead State.
        if (PlayerState != ATTACK && PlayerState != BLOCK && PlayerState != DEAD)
        {
            dirH = Input.GetAxis("Horizontal");
        }

        // Check if player is pressing left or right.
        if (Mathf.Abs(dirH) > 0.0f)
        {
            // Checks if player is also pressing Left Shift and if it isn't already in Dash State.
            if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerState != DASH && canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    void Jump()
    {
        // Check if player is pressing Spacebar and if they are either in Land, Attack or Block State.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSliding)
            {
                wallJumping = true;
                Invoke("StopWallJump", wallJumpDuration);
            }
            else if (PlayerState == LAND || PlayerState == ATTACK || PlayerState == SKILL)
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
            }
        }
    }

    void StopWallJump()
    {
        playSFX(3);
        wallJumping = false;
    }

    void Action()
    {
        // Checks if player is in Land State for Attack and Block.
        if (PlayerState == LAND)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                moveSpeed = 0;
                PlayerState = ATTACK;
                skillIcon[0].fillAmount = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                moveSpeed = 0;
                PlayerState = BLOCK;

                int playerLayer = LayerMask.NameToLayer("Player");
                int enemyLayer = LayerMask.NameToLayer("Enemy");
                Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

                skillIcon[1].fillAmount = 0.0f;
            }
        }

        // If the player stops holding Mouse1, stops blocking and reset everything to Land State.
        if (Input.GetKeyUp(KeyCode.Mouse1) && PlayerState == BLOCK)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

            PlayerState = LAND;
            moveSpeed = baseMoveSpeed;

            skillIcon[1].fillAmount = 1.0f;
        }

        // Check if player presses Q to activate skill, whether player has enough mana and that skill isn't already in use.
        if (Input.GetKeyDown(KeyCode.Q) && currMP >= 10 && canUseSkill1)
        {
            StartCoroutine(Skill());
        }
    }

    public float CheckGround()
    {
        float dist1 = Physics2D.Raycast(transform.Find("CheckGroundRaycastL").position, -Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")).distance;
        float dist2 = Physics2D.Raycast(transform.Find("CheckGroundRaycastR").position, -Vector2.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")).distance;
        
        if (dist1 < dist2)
        {
            return dist1;
        }
        else
        {
            return dist2;
        }
    }

    IEnumerator Dash()
    {
        skillIcon[2].fillAmount = 0;

        canDash = false;
        PlayerState = DASH;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyShield"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyArrow"), true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        tr.emitting = true;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        playSFX(4);

        yield return new WaitForSeconds(dashDuration);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyArrow"), false);
        rb.gravityScale = originalGravity;
        PlayerState = LAND;
        tr.emitting = false;
        dashCDStart = true;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        dashCDStart = false;
    }

    IEnumerator Skill()
    {
        GameObject closestEnemy = GetComponentInChildren<SkillRange>().ClosestEnemy();

        if (closestEnemy != null)
        {
            skillIcon[3].fillAmount = 0.0f;

            currMP -= 10;
            canUseSkill1 = false;
            PlayerState = SKILL;

            yield return new WaitForSeconds(0.25f);

            Vector2 aboveEnemy = closestEnemy.transform.position + new Vector3(0, 5, 0);
            
            Instantiate(lightningCloud, aboveEnemy, Quaternion.identity);
            playSFX(7);

            RaycastHit2D hit = Physics2D.Raycast(aboveEnemy, -Vector2.up, Mathf.Infinity, (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("EnemyShield")));
            
            if (hit)
            {
                Debug.Log(hit.collider.name);
                hit.collider.gameObject.SendMessage("TakeDamage", 20, SendMessageOptions.DontRequireReceiver);
            }

            yield return new WaitForSeconds(0.25f);

            PlayerState = LAND;
            skillCDStart = true;

            yield return new WaitForSeconds(skill1Cooldown);

            canUseSkill1 = true;
            skillCDStart = false;
        }
        else
        {
            Debug.Log("Skill 1 : No enemy in range.");
        }
    }

    // public void MeleeRangeDetected(MeleeRange meleeRange, Collider2D other)
    // {
    //     if (!attackToResolve)
    //     {
    //         if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
    //             other.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
    //         {
    //             if (PlayerState == ATTACK)
    //             {
    //                 Debug.Log("Attacked Enemy");
    //                 attackToResolve = true;

    //                 other.gameObject.SendMessage("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
    //             }
    //         }
    //     }
    // }

    public void BlockRangeDetected(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyInfront = true;
        }
        else
        {
            enemyInfront = false;
        }
    }

    public void OnGround(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (PlayerState == FALL)
            {
                PlayerState = LAND;
                DustParticles();
                playSFX(2);
            }
        }
    }

    public void WallStay(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWallTouch = true;
        }
    }

    public void WallExit(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWallTouch = false;
        }
    }

    void UpdateAnimation()
    {
        // Check if Dash cooldown can start, then += to the skill icon's fillAmount.
        if (!canDash && dashCDStart)
        {
            if (skillIcon[2].fillAmount < 1.0f)
            {
                skillIcon[2].fillAmount += Time.deltaTime / dashCooldown;
            }
        }

        // Check if Skill 1 cooldown can start, then += to the skill icon's fillAmount.
        if (!canUseSkill1 && skillCDStart)
        {
            if (skillIcon[3].fillAmount < 1.0f)
            {
                skillIcon[3].fillAmount += Time.deltaTime / skill1Cooldown;
            }
        }

        if (PlayerState == LAND && Mathf.Abs(rb.velocity.x) <= 0.001f && Mathf.Abs(rb.velocity.y) == 0f)
        {
            ar.Play(IDLE);
        }

        if ((PlayerState == LAND || PlayerState == SKILL) && Mathf.Abs(rb.velocity.x) > 0.001f && (Mathf.Abs(rb.velocity.y) > -1.15f && Mathf.Abs(rb.velocity.y) < 1.15f))
        {
            if (PlayerState == SKILL)
            {
                PlayerState = LAND;
            }

            ar.Play(WALK);
        }

        if ((PlayerState == LAND || PlayerState == ATTACK || PlayerState == SKILL) && rb.velocity.y >= 1.15f)
        {
            if (PlayerState == SKILL)
            {
                PlayerState = LAND;
            }

            PlayerState = JUMP;
            DustParticles();
            ar.Play(JUMP);
        }

        if ((PlayerState == JUMP && rb.velocity.y <= -1.15f) || (PlayerState == LAND && CheckGround() > 3.36f))
        {
            PlayerState = FALL;
            ar.Play(FALL);
        }

        if (PlayerState == ATTACK)
        {
            ar.Play(ATTACK);

            if (ar.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !ar.IsInTransition(0))
            {
                skillIcon[0].fillAmount = 1.0f;
                attackToResolve = false;
                PlayerState = LAND;
                moveSpeed = baseMoveSpeed;
            }
        }

        if (PlayerState == BLOCK)
        {
            ar.Play(BLOCK);
        }

        if (PlayerState == SKILL)
        {
            ar.Play(SKILL);
        }

        if (PlayerState == DEAD)
        {
            ar.Play(DEAD);
        }
    }

    void UpdatePlayerDirection()
    {
        if (dirH > 0f)
        {
            transform.localScale = new Vector2(5, 5);
        }
        else if (dirH < 0f)
        {
            transform.localScale = new Vector2(-5, 5);
        }
    }

    void DustParticles()
    {
        dustPS.Play();
    }

    void BloodParticles(float damage)
    {
        var emission = bloodPS.emission;
        emission.rateOverTime = 10 * damage;
        bloodPS.Play();
    }

    void HealParticles(float healAmt)
    {
        var emission = healPS.emission;
        emission.rateOverTime = 1.5f * healAmt;
        healPS.Play();
    }

    void ManaParticles(float regenAmt)
    {
        var emission = healPS.emission;
        emission.rateOverTime = 2 * regenAmt;
        manaPS.Play();
    }

    IEnumerator TakeHit()
    {
        sr.color = new Color32(255, 128, 128, 255);
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    public void RegainHP(float health)
    {
        if ((currHP + health) <= maxHP)
        {
            playSFX(9);
            HealParticles(health);
            currHP += health;
        }
    }

    public void RegainMP(float mana)
    {
        if ((currMP + mana) <= maxMP)
        {
            playSFX(10);
            ManaParticles(mana);
            currMP += mana;
        }
    }

    public void TakeDamage(float damage)
    {
        if (PlayerState != DASH && !enemyInfront && PlayerState != BLOCK && skillIcon[2].fillAmount != 0)
        {
            if ((currHP - damage) <= 0)
            {
                currHP = 0;
                rb.velocity = new Vector2(0, 0);
                PlayerState = DEAD;

                GameObject.Find("GameOver").GetComponent<GameOver>().ActivateScene(false);
            }
            else
            {
                playSFX(8);
                currHP -= damage;
                BloodParticles(damage);
                StartCoroutine(TakeHit());
            }
        }
    }

    void UpdateHPMPBar()
    {
        hpSlider.value = currHP;
        mpSlider.value = currMP;

        if (currHP != maxHP)
        {
            isHealthMax = false;
        }

        if (currMP != maxHP)
        {
            isManaMax = false;
        }
    }

    void CheckEnemyExist()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyArray.Length <= 0)
        {
            if (SceneManager.GetActiveScene().name == "GameLevel1")
            {
                GameObject.Find("Portal").GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("Portal").GetComponent<CapsuleCollider2D>().enabled = true;
            }
            else if (SceneManager.GetActiveScene().name == "GameLevel2")
            {
                GameObject.Find("GameOver").GetComponent<GameOver>().ActivateScene(true);
            }
        }
    }

    void CheatKeybinds()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            RegainHP(10);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            currMP -= 10;
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            RegainMP(10);
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            maxHP = 100000;
            currHP = maxHP;

            maxMP = 100000;
            currMP = maxMP;
        }
    }

    public void playSFX(int clip)
    {
        for (int i = 0; i < audSrc.Length; i++)
        {
            if (!audSrc[i].isPlaying)
            {
                audSrc[i].clip = clips[clip];
                audSrc[i].Play();
                break;
            }
        }
    }
}