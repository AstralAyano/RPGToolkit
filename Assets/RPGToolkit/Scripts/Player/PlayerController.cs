using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

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

        [Header("Player Settings")]
        public bool hasInventory;
        public bool hasLevel;

        public bool hasHealthPoint;
        public bool hasManaPoint;
        public bool hasStaminaPoint;

        public bool hasDash;
        public List<LayerMask> phaseThroughLayers = new List<LayerMask>();
        public List<LayerMask> dodgeableLayers = new List<LayerMask>();
        public bool hasWallJump;
        public LayerMask wallLayer;

        public bool hasAttack;
        public bool hasBlock;

        [Header("Player Stats / Values")]
        public int currentLevel;
        public float currentExperience = 0, maxExperience = 0;
        public float currentHealthPoint = 0, maxHealthPoint = 0;
        public float currentManaPoint = 0, maxManaPoint = 0;
        public float currentStaminaPoint = 0, maxStaminaPoint = 0;

        public float baseWalkSpeed = 0;
        public float currentWalkSpeed = 0;
        public float baseJumpHeight = 0;

        public float dashPower;
        public float dashDuration;
        public float dashCooldown;

        public float wallSlidingSpeed;
        public float wallJumpDuration;
        public Vector2 wallJumpForce;

        [HideInInspector] public bool isHealthMax = true;
        [HideInInspector] public bool isManaMax = true;

        private float dirH = 0.0f;
        [SerializeField] private bool dashUsable = true;
        [SerializeField] private bool dashCooldownStart = false;
        [SerializeField] private bool isTouchingWall = false;
        [SerializeField] private bool isWallJumping = false;
        [SerializeField] private bool isWallSliding = false;

        [SerializeField] private bool attackToResolve = false;
        [SerializeField] private bool enemyInfront = false;

        [Header("References")]
        [SerializeField] private PhysicsMaterial2D playerPhysics;
        [SerializeField] private ParticleSystem dustPS;
        [SerializeField] private ParticleSystem bloodPS;
        [SerializeField] private ParticleSystem healPS;
        [SerializeField] private ParticleSystem manaPS;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Animator ar;
        private TrailRenderer tr;

        [Header("Player SFXs")]
        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private AudioClip[] SFXClips;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            SFXClips = Resources.LoadAll<AudioClip>("Audio/PlayerSFXs");
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            ar = GetComponent<Animator>();
            tr = GetComponentInChildren<TrailRenderer>();

            audioSources = GetComponentsInChildren<AudioSource>();

            currentHealthPoint = hasHealthPoint ? maxHealthPoint : 0;
            currentManaPoint = hasManaPoint ? maxManaPoint : 0;
            currentStaminaPoint = hasStaminaPoint ? maxStaminaPoint : 0;

            currentWalkSpeed = baseWalkSpeed;
        }

        private void Update()
        {
            PlayerMovement();
            PlayerJump();

            CheckWallSliding();

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
            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            if (isWallJumping)
            {
                rb.velocity = new Vector2(-dirH * wallJumpForce.x, wallJumpForce.y);
            }
            else if (PlayerState != DASH)
            {
                rb.velocity = new Vector2(dirH * currentWalkSpeed, rb.velocity.y);
            }
        }

        private void LateUpdate()
        {
            UpdateAnimation();

            UpdatePlayerDirection();
        }

        private void PlayerMovement()
        {
            // Get Horizontal inputs from the player if it isn't in Attack, Block or Dead State.
            if (PlayerState != ATTACK && PlayerState != BLOCK && PlayerState != DEAD)
            {
                dirH = Input.GetAxis("Horizontal");
            }

            //Check if player is pressing left or right.
            if (Mathf.Abs(dirH) > 0.0f)
            {
                // Checks if player is also pressing Left Shift and if it isn't already in Dash State.
                if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerState != DASH && dashUsable)
                {
                    StartCoroutine(PlayerDash());
                }
            }
        }

        private void PlayerJump()
        {
            // Check if player is pressing Spacebar and if they are either in Land, Attack or Block State.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isWallSliding)
                {
                    isWallJumping = true;
                    Invoke("StopWallJump", wallJumpDuration);
                }
                else if (PlayerState == LAND || PlayerState == ATTACK || PlayerState == SKILL)
                {
                    rb.AddForce(Vector3.up * baseJumpHeight, ForceMode2D.Impulse);
                }
            }
        }

        private IEnumerator PlayerDash()
        {
            //skillIcon[2].fillAmount = 0;

            dashUsable = false;
            PlayerState = DASH;

            // Ignore collisions for phase through layers
            foreach (LayerMask layerMask in phaseThroughLayers)
            {
                int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
                if (layer >= 0 && layer <= 31)
                {
                    Physics2D.IgnoreLayerCollision(gameObject.layer, layer, true);
                }
                else
                {
                    Debug.LogWarning($"Layer {layer} is out of range. Skipping this layer.");
                }
            }

            // Ignore collisions for dodgeable layers
            foreach (LayerMask layerMask in dodgeableLayers)
            {
                int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
                if (layer >= 0 && layer <= 31)
                {
                    Physics2D.IgnoreLayerCollision(gameObject.layer, layer, true);
                }
                else
                {
                    Debug.LogWarning($"Layer {layer} is out of range. Skipping this layer.");
                }
            }

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            tr.emitting = true;
            rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
            //playSFX(4);

            yield return new WaitForSeconds(dashDuration);

            // Re-enable collisions for dodgeable layers
            foreach (LayerMask layerMask in dodgeableLayers)
            {
                int layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
                if (layer >= 0 && layer <= 31)
                {
                    Physics2D.IgnoreLayerCollision(gameObject.layer, layer, false);
                }
                else
                {
                    Debug.LogWarning($"Layer {layer} is out of range. Skipping this layer.");
                }
            }

            rb.gravityScale = originalGravity;
            PlayerState = LAND;
            tr.emitting = false;
            dashCooldownStart = true;

            yield return new WaitForSeconds(dashCooldown);

            dashUsable = true;
            dashCooldownStart = false;
        }

        private void StopWallJump()
        {
            isWallJumping = false;
        }

        private void UpdatePlayerDirection()
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

        private void UpdateAnimation()
        {
            // Check if Dash cooldown can start, then += to the skill icon's fillAmount.
            // if (!canDash && dashCDStart)
            // {
            //     if (skillIcon[2].fillAmount < 1.0f)
            //     {
            //         skillIcon[2].fillAmount += Time.deltaTime / dashCooldown;
            //     }
            // }

            // Check if Skill 1 cooldown can start, then += to the skill icon's fillAmount.
            // if (!canUseSkill1 && skillCDStart)
            // {
            //     if (skillIcon[3].fillAmount < 1.0f)
            //     {
            //         skillIcon[3].fillAmount += Time.deltaTime / skill1Cooldown;
            //     }
            // }

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
                //DustParticles();
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
                    //skillIcon[0].fillAmount = 1.0f;
                    attackToResolve = false;
                    PlayerState = LAND;
                    currentWalkSpeed = baseWalkSpeed;
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

        private void CheckWallSliding()
        {
            if (isTouchingWall && PlayerState != LAND && dirH != 0)
            {
                isWallSliding = true;
            }
            else
            {
                isWallSliding = false;
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

        public void OnGround(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                if (PlayerState == FALL)
                {
                    PlayerState = LAND;
                    //DustParticles();
                    //playSFX(2);
                }
            }
        }

        public void WallStay(Collider2D other)
        {
            if (other.gameObject.layer == wallLayer)
            {
                isTouchingWall = true;
            }
        }

        public void WallExit(Collider2D other)
        {
            if (other.gameObject.layer == wallLayer)
            {
                isTouchingWall = false;
            }
        }

        public void MeleeRangeDetected(MeleeRange meleeRange, Collider2D other)
        {
            if (!attackToResolve)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
                    other.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
                {
                    if (PlayerState == ATTACK)
                    {
                        Debug.Log("Attacked Enemy");
                        attackToResolve = true;

                        other.gameObject.SendMessage("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

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

        public void RegainStats(string stat, float amount)
        {
            switch (stat)
            {
                case "HP":
                    if ((currentHealthPoint + amount) <= maxHealthPoint)
                    {
                        PlaySFX("Health");
                        //HealParticles(health);
                        currentHealthPoint += amount;
                    }
                    break;
                case "MP":
                    if ((currentManaPoint + amount) <= maxManaPoint)
                    {
                        PlaySFX("Mana");
                        //HealParticles(health);
                        currentManaPoint += amount;
                    }
                    break;
                case "SP":
                    if ((currentStaminaPoint + amount) <= maxStaminaPoint)
                    {
                        PlaySFX("Stamina");
                        //HealParticles(health);
                        currentStaminaPoint += amount;
                    }
                    break;
            }
        }

        public void PlaySFX(string audioName)
        {
            AudioClip clipToPlay = null;

            foreach (AudioClip clip in SFXClips)
            {
                if (clip.name == audioName)
                {
                    clipToPlay = clip;
                    break;
                }
            }

            for (int i = 0; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].clip = clipToPlay;
                    audioSources[i].Play();
                    break;
                }
            }
        }
    }
}
