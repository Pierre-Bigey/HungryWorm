using System;
using HungryWorm.Scripts.Food;
using UnityEngine;

namespace HungryWorm
{
    #region State Machine
    public enum Step
    {
        Enter,
        Process,
        Exit
    }

    public class AirplaneState
    {
        protected AirplaneState nextState;
        protected AirplaneController airplaneController;
        protected Step step;
        
        public AirplaneState(AirplaneController airplaneController)
        {
            this.airplaneController = airplaneController;
            this.step = Step.Enter;
        }
        
        public void SetNextState(AirplaneState nextState)
        {
            this.nextState = nextState;
        }

        protected virtual void Enter()
        { }

        protected virtual void Process()
        { }
        
        protected virtual void Exit()
        { }

        public virtual void Update()
        {
            if (step == Step.Enter)
            {
                Enter();
                step = Step.Process;
            }
            else if (step == Step.Process)
            {
                Process();
            } 
            else if (step == Step.Exit)
            {
                Exit();
                step = Step.Enter;
                airplaneController.ChangeState(nextState);
            }
        }
    }
    
    public class AirplaneMissileReloadState : AirplaneState
    {
        private float reloadTime;
        private float timeWhenReloaded;
        public AirplaneMissileReloadState(AirplaneController airplaneController, float reloadTime) : base(airplaneController)
        {
            this.reloadTime = reloadTime;
        }

        protected override void Enter()
        {
            airplaneController.m_speed = 1.2f * airplaneController.m_MaxSpeed;
            timeWhenReloaded = Time.time;
        }
        
        protected override void Process()
        {
            if (Time.time - timeWhenReloaded > reloadTime)
            {
                step = Step.Exit;
            }
        }
        
        protected override void Exit()
        {
            airplaneController.m_speed = airplaneController.m_MaxSpeed;
            airplaneController.ReloadMissile();
        }
    }
    
    public class AirplaneMissileTargetAquiringState : AirplaneState
    {
        private PlayerController m_playerController;
        private GameObject m_player;
        private float m_distanceToFireMissile = 10;
        private float m_timeToFireMissile = 2;
        private float m_timeSinceAquiringTarget;
        private bool m_isAquiringTarget;
        
        public AirplaneMissileTargetAquiringState(AirplaneController airplaneController, 
            PlayerController m_playerController, float mDistanceToFireMissile, float mTimeToFireMissile) : base(airplaneController)
        {
            this.m_playerController = m_playerController;
            this.m_player = m_playerController.gameObject;
            this.m_distanceToFireMissile = mDistanceToFireMissile;
            this.m_timeToFireMissile = mTimeToFireMissile;
        }
        
        protected override void Enter()
        {
            m_isAquiringTarget = false;
        }

        protected override void Process()
        {
            float distance = airplaneController.transform.position.x - m_player.transform.position.x;
            bool playerInFront = airplaneController.movingRight ? distance < 0 : distance > 0;
            if ( playerInFront && Math.Abs(distance) < m_distanceToFireMissile)
            {
                //IF player not in dirt, shoot a laser at its head to detect him
                if (!m_playerController.InDirt)
                {
                    DetecPlayer();
                }
                else
                {
                    // Else shoot a laser around the player to see if we can hit him 
                    PointLaserAround();
                }
            }
            // If the player is not in front of the airplane, stop the laser
            else
            {
                
                airplaneController.m_Laser.gameObject.SetActive(false);
                if(m_isAquiringTarget) AudioManager.Instance.StopMissileAcquiringSound();
                m_isAquiringTarget = false;
                
            }
        }

        /// <summary>
        /// Detect if the player is visible by the plane by sending a raycast to its head position
        /// </summary>
        private void DetecPlayer()
        {
            //Start Forward Raycast
            Vector3 direction = m_player.transform.position - airplaneController.m_LaserStartPoint.position;
            Ray ray = new Ray(airplaneController.m_LaserStartPoint.position, direction);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
            if (hit.collider != null)
            {
                airplaneController.m_Laser.SetLaser(airplaneController.m_LaserStartPoint.position, hit.point);

                airplaneController.m_Laser.gameObject.SetActive(true);

                //Check if it's part of the player layer
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    PointLaser(hit.collider.gameObject);
                }
            }
        }

        /// <summary>
        /// Called when the raycast hit the player, to show the laser and countdown before firing
        /// </summary>
        /// <param name="collider"></param>
        private void PointLaser(GameObject collider)
        {
            if (m_isAquiringTarget)
            {
                if (Time.time - m_timeSinceAquiringTarget > m_timeToFireMissile)
                {
                    // Anticipate where the player will be when the missile reaches him
                    Rigidbody2D playerRigidbody = m_player.GetComponent<Rigidbody2D>();
                    float timeToReachPlayer = Vector2.Distance(airplaneController.m_MissileSpawnPoint.position, collider.transform.position) / (5 * 2) - 0.2f;
                    Vector3 target = collider.transform.position; //+ new Vector3(playerRigidbody.velocity.x * timeToReachPlayer, playerRigidbody.velocity.y * timeToReachPlayer, 0);
                    //Debug.Log("Firing on : " + collider.name);
                    airplaneController.FireMissile(target);
                    step = Step.Exit;
                }
            }
            else
            {
                m_isAquiringTarget = true;
                m_timeSinceAquiringTarget = Time.time;
                AudioManager.Instance.PlayMissileAcquiringSound();
            }
        }

        private void PointLaserAround()
        {
            float playerY = m_player.transform.position.y;
            // Add 5 degrees to the direction in both directions
            Vector3 directionRight = m_player.transform.position + new Vector3(1,1-playerY,0)- airplaneController.m_LaserStartPoint.position;
            Vector3 directionLeft =m_player.transform.position + new Vector3(-1,1-playerY,0)- airplaneController.m_LaserStartPoint.position;
            
            Ray rayRight = new Ray(airplaneController.m_LaserStartPoint.position, directionRight);
            Ray rayLeft = new Ray(airplaneController.m_LaserStartPoint.position, directionLeft);
            
            RaycastHit2D hitRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, 1000);

            if (hitRight.collider != null)
            {
                if (hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    airplaneController.m_Laser.SetLaser(airplaneController.m_LaserStartPoint.position, hitRight.point);
                    airplaneController.m_Laser.gameObject.SetActive(true);
                    PointLaser(hitRight.collider.gameObject);
                }
            }
            else
            {
                RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, 1000);
                if (hitLeft.collider != null)
                {
                    if (hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        airplaneController.m_Laser.SetLaser(airplaneController.m_LaserStartPoint.position, hitLeft.point);
                        airplaneController.m_Laser.gameObject.SetActive(true);
                        PointLaser(hitLeft.collider.gameObject);
                    }
                }
            }

        }
        
        protected override void Exit()
        {
            AudioManager.Instance.StopMissileAcquiringSound();
           
            airplaneController.m_Laser.gameObject.SetActive(false);
        }
    }

    public class AirplaneBombReloadState : AirplaneState
    {
        private float reloadTime;
        private float timeWhenReloaded;

        public AirplaneBombReloadState(AirplaneController airplaneController, float reloadTime) : base(
            airplaneController)
        {
            this.reloadTime = reloadTime;
        }

        protected override void Enter()
        {
            timeWhenReloaded = Time.time;
        }

        protected override void Process()
        {
            if (Time.time - timeWhenReloaded > reloadTime)
            {
                step = Step.Exit;
            }
        }

        protected override void Exit()
        {
            airplaneController.ReloadBomb();
        }
    }

    public class AirplaneBombScanning : AirplaneState
    {
        // The angle from the horiontal axis to the ground where to check for player
        private float m_angle;
        
        public AirplaneBombScanning(AirplaneController airplaneController, float angle) : base(airplaneController)
        {
            // Angle conversion in radian
            m_angle = angle * Mathf.Deg2Rad;
        }

        protected override void Process()
        {
            
            // IF goes to left, Add pi/2 to the angle
            float angle = m_angle + (airplaneController.movingRight ? 0 : (float) Math.PI / 2 );
            
            // Compute direction based on the angle
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(-angle), 0);
            Ray ray = new Ray(airplaneController.m_BombSpawnPoint.position, direction);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    airplaneController.DropBomb();
                    step = Step.Exit;
                }
            }
            
            // Debug the ray
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        }
    }
    
    #endregion
    public class AirplaneController : Edible
    {
        [Header("Structure")]
        [SerializeField] private GameObject m_AirplaneModel;
        [SerializeField] private GameObject m_SmokeParticle;
        [SerializeField] private GameObject m_ThrustAnimation;
        private Rigidbody2D m_Rigidbody2D;
        
        [Header("Movement")]
        [SerializeField] private float atlitude = 5.35f;
        [SerializeField] private float m_turnAtDistance = 50f;
        [SerializeField] public float m_MaxSpeed = 10f;
        [HideInInspector] public bool movingRight = true;
        [HideInInspector] public float m_speed;
        private bool m_isMoving = true;
        
        [Header("Bomb")]
        [SerializeField] private float m_BombReloadTime = 15;
        [SerializeField] private GameObject m_BombPrefab;
        private GameObject m_Bomb;
        [SerializeField] public Transform m_BombSpawnPoint;
        [SerializeField] private float m_BombScanningAngle = 45;
        private float m_timeWhenBombDropped;
        
        [Header("Missile")]
        [SerializeField] private float m_distanceToFireMissile = 10;
        [SerializeField] private float m_MissileReloadTime = 5;
        [SerializeField] private float m_MissileAquiringTime = 2;
        [SerializeField] private GameObject m_MissilePrefab;
        private GameObject m_Missile;
        [SerializeField] public Transform m_MissileSpawnPoint;
        [SerializeField] public Laser m_Laser;
        [SerializeField] public Transform m_LaserStartPoint;
        [HideInInspector] GameObject m_player;
        private PlayerController m_playerController;
        
        // Finite State Machine
        private AirplaneState m_currentMissileState;
        private AirplaneState m_currentBombState;
        
        private void Start()
        {
            transform.position = new Vector3(transform.position.x, atlitude, transform.position.z);
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_playerController = PlayerController.Instance;
            m_player = m_playerController.gameObject;
            
            m_Missile = Instantiate(m_MissilePrefab, m_MissileSpawnPoint);
            
            AirplaneState missileReloadState = new AirplaneMissileReloadState(this, m_MissileReloadTime);
            AirplaneState missileTargetAquiringState = new AirplaneMissileTargetAquiringState(this, m_playerController, m_distanceToFireMissile, m_MissileAquiringTime);
            missileReloadState.SetNextState(missileTargetAquiringState);
            missileTargetAquiringState.SetNextState(missileReloadState);

            m_currentMissileState = missileTargetAquiringState;
            
            AirplaneState bombReloadState = new AirplaneBombReloadState(this, m_BombReloadTime);
            AirplaneState bombScanningState = new AirplaneBombScanning(this, m_BombScanningAngle);
            bombReloadState.SetNextState(bombScanningState);
            bombScanningState.SetNextState(bombReloadState);
            
            m_currentBombState = bombReloadState;
            
            m_speed = m_MaxSpeed;

        }

        private void OnEnable()
        {
            UIEvents.LeaderboardScreenShown += DestroyThis;
        }
        
        private void OnDisable()
        {
            UIEvents.LeaderboardScreenShown -= DestroyThis;
        }

        private void FixedUpdate()
        {
            if (m_player == null)
            {
                Debug.LogError("Player not found in AirplaneController");
            }

            if (!m_isMoving)
            {
                return;
            }
            
            // Move the airplane
            Move();
            
            // Get the signed distance between the airplane and the player
            float distance = transform.position.x - m_player.transform.position.x;
            
            // Check if the airplane should turn
            if (Math.Abs(distance) > m_turnAtDistance && ( (distance > 0 && movingRight) || (distance < 0 && !movingRight) ) )
            {
                Turn();
            }
            
            m_currentMissileState.Update();
            m_currentBombState.Update();
        }

        public void ReloadMissile()
        {
            m_Missile = Instantiate(m_MissilePrefab, m_MissileSpawnPoint);
        }
        
        public void ReloadBomb()
        {
            m_Bomb = Instantiate(m_BombPrefab, m_BombSpawnPoint);
        }

        private void Move()
        {
            // Move the airplane
            m_Rigidbody2D.velocity = new Vector2(movingRight ? m_speed : -m_speed, 0);
        }

        private void Turn()
        {
            movingRight = !movingRight;
            
            
            
            // Reverse the rotation of the airplane
            transform.rotation = Quaternion.Euler(0, movingRight ? 0 : 180, 0);
            
            // Reverse the position of the missile
            var localPosition = m_MissileSpawnPoint.localPosition;
            m_MissileSpawnPoint.localPosition = new Vector3(localPosition.x, localPosition.y, movingRight ? -1 : 1);
            
            // Reverse the position of the bomb
            var position = m_BombSpawnPoint.localPosition;
            m_BombSpawnPoint.localPosition = new Vector3(position.x, position.y, movingRight ? 1 : -1);
            
            // Reverse the position of the ThrustAnimation
            var thrustPosition = m_ThrustAnimation.transform.localPosition;
            m_ThrustAnimation.transform.localPosition = new Vector3(thrustPosition.x, thrustPosition.y, movingRight ? 1 : -1);
        }

        
        public void FireMissile(Vector3 target)
        {
            m_Missile.transform.SetParent(null);
            m_Missile.GetComponent<MissileController>().Fire(target + new Vector3(0,1,0));
            AudioManager.Instance.PlayMissileFiredSound();
        }

        public void DropBomb()
        {
            m_Bomb.transform.SetParent(null);
            Vector3 force = new Vector3(movingRight ? 1 : -1, 0, 0);
            m_Bomb.GetComponent<BombController>().Drop(force);
        }
        
        public void ChangeState(AirplaneState nextState)
        {
            m_currentMissileState = nextState;
        }

        public void Crash()
        {
            m_isMoving = false;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.None; 
            // TODO Add fire animation
            m_ThrustAnimation.SetActive(false);
        }
        
        public void Explose()
        {
            m_isMoving = false;
            GetComponent<EdgeCollider2D>().enabled = false;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll; 
            // TODO Add explosion animation
            
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }

        public override void Kill()
        {
            Crash();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            //CHeck if hit the dirt
            if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                Explose();
            }
        }   
        
    }
}