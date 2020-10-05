using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Talk to Bman about this code:
namespace Assets.Scripts.Actors.Enemy
{
    [RequireComponent(typeof(FieldOfView), typeof(HealthSystem),
        typeof(EnemyMovement))]
    public class EnemyAIBrain : MonoBehaviour
    {
        [Tooltip("This value can be equal or smaller than the FOV Radius!")]
        [SerializeField] float _attackRange = 8f;
        [SerializeField] float _delayBetweenStateMachineUpdate = 0.2f;
        [SerializeField] public bool isBoss = false;
        [SerializeField] public float HardSearchTime = 5;


        public float DetectStateDelayTime = 1;
        
        private bool _isActive = true;
        
        private StateMachine _stateMachine;
        private EnemyAttacker _enemyAttacker;

        void Awake()
        {
            _stateMachine = new StateMachine(this, GetComponent<FieldOfView>(),
                GetComponent<HealthSystem>(), GetComponent<EnemyMovement>());
            _enemyAttacker = GetComponent<EnemyAttacker>();
        }

        void OnEnable()
        {
            GetComponent<HealthSystem>().OnTookDamage += OnTookDamage;
        }

        void OnDisable()
        {
            GetComponent<HealthSystem>().OnTookDamage -= OnTookDamage;
        }

        void Start()
        {
            StartCoroutine("PersistentStateUpdate", _delayBetweenStateMachineUpdate);
        }
        
        float _attackTimer = 0;

        void Update()
        {
            var myState = _stateMachine.CurrentState;
            if (myState == StateMachine.State.Attack)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _enemyAttacker.FireCooldown) {
                    _enemyAttacker.Attack();
                    _attackTimer = 0;
                }
            }
        }
        
        // Subscribed to the healthSystem onTookDamage event
        // so AI wont stand there and take bullets
        private void OnTookDamage()
        {
            _stateMachine.InvokeResponseForDamage();
        }
        

        /// <summary>
        /// Counting on the fact that there is only 1 player
        /// and that is the first and only target of FOV
        /// </summary>
        /// <param name="playerPosition"></param>
        /// <returns></returns>
        float GetDistanceToPlayer(Vector3 playerPosition)
        {
            return Vector3.Distance(transform.position, playerPosition);
        }

        public bool GetPlayerInAttackRange(Vector3 playerPosition)
        {
            return GetDistanceToPlayer(playerPosition) <= _attackRange;
        }
        
        IEnumerator PersistentStateUpdate(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                if (_isActive) { _stateMachine.UpdateState(); }
            }
            // ReSharper disable once IteratorNeverReturns
        }
        private class StateMachine
        {
            public State CurrentState { get; set; } = State.Idle;
            private readonly FieldOfView _fieldOfView;
            private readonly EnemyAIBrain _enemyAiBrain;
            private readonly HealthSystem _healthSystem;
            private readonly EnemyMovement _enemyMovement;
            
            private const float HealthRunThreshold = 20f;

            public StateMachine(EnemyAIBrain enemyAiBrain, FieldOfView fov,
                HealthSystem healthSystem, EnemyMovement enemyMovement)
            {
                _enemyAiBrain = enemyAiBrain;
                _fieldOfView = fov;
                _healthSystem = healthSystem;
                _enemyMovement = enemyMovement;
            }
            
            private float _detectDelayTimer;
            private float _hardSearchTimer;
            private const float HardSearchCooldown = 3;

            private Vector3 _playerLastSeenPos;

            public void UpdateState()
            {
                Debug.Log(CurrentState);
                // If the AI see the player
                if (_fieldOfView.visibleTargets.Count >= 1)
                {
                    // Safety throw:
                    if (_fieldOfView.visibleTargets.Count > 1)
                    {
                        Debug.LogError("Enemy " + this._enemyAiBrain.gameObject + "got 2 targets");
                    }
                    
                    var player = _fieldOfView.visibleTargets[0];
                    _playerLastSeenPos = player.position;
                    
                    // If health is below threshold we should run away
                    if (_healthSystem.GetCurrentHealth() <= HealthRunThreshold)
                    {
                        _enemyMovement.ToggleChaseSpeed(true);
                        CurrentState = State.RunAway;
                        return;
                    }

                    if (CurrentState == State.RunAway)
                    {
                        _enemyMovement.MoveToRandomPointWithMinDistFromPlayer(_playerLastSeenPos, 10);
                    }
                    
                    // Checking if the current state is attacking
                    // means we should keep attacking
                    else if (CurrentState == State.Attack)
                    {
                        // Only attacks moving targets, unless you're the boss :)
                        // Moving to HardSearch state instead (looking at the player position)
                        if (!_enemyAiBrain.isBoss && player.GetComponentInChildren<Rigidbody>().velocity.magnitude == 0)
                        {
                            _hardSearchTimer = _enemyAiBrain.HardSearchTime;
                            CurrentState = State.HardSearch;
                            _fieldOfView.SetupLight(Color.magenta);
                        }
                    }
                    
                    // If the AI is idle
                    // means we should start detecting the target for
                    // T time before chasing into range
                    else if (CurrentState == State.Idle)
                    {
                        _detectDelayTimer = _enemyAiBrain.DetectStateDelayTime;
                        CurrentState = State.Detect;
                        _fieldOfView.SetupLight(Color.cyan);
                    }
                    
                    // If detecting, should wait and play animation
                    else if (CurrentState == State.Detect)
                    {
                        _detectDelayTimer -= Time.deltaTime + _enemyAiBrain.DetectStateDelayTime;

                        // After the timer run out
                        if (_detectDelayTimer <= 0)
                        {
                            if (_enemyAiBrain.GetPlayerInAttackRange(player.position))
                            {
                                CurrentState = State.Attack;
                                _fieldOfView.SetupLight(Color.red);
                                _enemyMovement.StopMoving();
                            }
                            else
                            {
                                _enemyMovement.ToggleChaseSpeed(true);
                                CurrentState = State.Chase;
                                _fieldOfView.SetupLight(Color.magenta);
                            }
                        }
                    }
                    
                    // If chasing should keep chase until out of sight
                    else if (CurrentState == State.Chase)
                    {
                        if (_enemyAiBrain.GetPlayerInAttackRange(player.position))
                        {
                            CurrentState = State.Attack;
                            _fieldOfView.SetupLight(Color.red);
                            _enemyMovement.StopMoving();
                        }
                        else
                        {
                            _enemyMovement.GoToTarget(player);
                        }
                    }
                    
                    // State hard search happens when the player
                    // stopped moving after he was attacked, so the AI
                    // will look for him, wait for T time and go back to
                    // some Idle
                    else if (CurrentState == State.HardSearch)
                    {
                        if (player.GetComponentInChildren<Rigidbody>().velocity.magnitude != 0)
                        {
                            CurrentState = State.Attack;
                            _fieldOfView.SetupLight(Color.red);
                            return;
                        }

                        _hardSearchTimer -= Time.deltaTime + _enemyAiBrain._delayBetweenStateMachineUpdate;

                        if (_hardSearchTimer <= 0)
                        {
                            _enemyMovement.ToggleChaseSpeed(false);
                            CurrentState = State.IdleSearch;
                            _enemyMovement.MoveToRandomPointWithMinDistFromPlayer(_playerLastSeenPos, 5);    
                            _fieldOfView.SetupLight(Color.yellow);
                        }
                    }
                    else if (CurrentState == State.IdleSearch)
                    {
                        _hardSearchTimer -= Time.deltaTime + _enemyAiBrain._delayBetweenStateMachineUpdate;
                        if (_hardSearchTimer <= -HardSearchCooldown)
                        {
                            _detectDelayTimer = _enemyAiBrain.DetectStateDelayTime;
                            CurrentState = State.Detect;
                            _fieldOfView.SetupLight(Color.cyan);
                        }
                    }
                }
                
                // If the AI cant see the player
                else
                {
                    if (CurrentState == State.Attack)
                    {
                        _hardSearchTimer = _enemyAiBrain.HardSearchTime;
                        CurrentState = State.HardSearch;
                        _fieldOfView.SetupLight(Color.magenta);
                        _enemyMovement.MoveToRandomPointWithMaxDistFromPlayer(_playerLastSeenPos, 3);
                    }
                    else if (CurrentState == State.HardSearch)
                    {
                        // todo add looking animation
                        _hardSearchTimer -= Time.deltaTime + _enemyAiBrain._delayBetweenStateMachineUpdate;

                        if (_hardSearchTimer <= 0)
                        {
                            _enemyMovement.ToggleChaseSpeed(false);
                            CurrentState = State.IdleSearch;
                            _enemyMovement.MoveToRandomPointWithMaxDist(3);    
                            _fieldOfView.SetupLight(Color.yellow);
                        }
                    }
                    
                    // Should pick a random point to go to in 
                    else if (CurrentState == State.IdleSearch)
                    {
                        Debug.Log(_enemyMovement.GetIsNavMeshStopped());
                        // If the AI reached the destination and stopped
                        if (_enemyMovement.GetIsNavMeshStopped())
                        {
                            // Make random chance to just stand still
                            var n = Random.value;
                            // 70% to keep moving to new point
                            if (n > 0.3f)
                            {
                                _enemyMovement.MoveToRandomPointWithMinDist(8);
                            }
                            else
                            {
                                CurrentState = State.Idle;
                            }
                        }
                    }
                    else if (CurrentState == State.Idle)
                    {
                        // todo add idle animation
                        // Make random chance to just stand still
                        var n = Random.value;
                        // 50% to keep moving to new point
                        if (n > 0.5f)
                        {
                            _enemyMovement.MoveToRandomPointWithMinDist(8);
                        }
                    }
                    else if (CurrentState == State.Chase)
                    {
                        _hardSearchTimer = _enemyAiBrain.HardSearchTime;
                        CurrentState = State.HardSearch;
                        _fieldOfView.SetupLight(Color.magenta);
                        _enemyMovement.MoveToRandomPointWithMaxDistFromPlayer(_playerLastSeenPos, 3);
                    }
                    else if (CurrentState == State.Detect)
                    {
                        _enemyMovement.ToggleChaseSpeed(false);
                        CurrentState = State.IdleSearch;
                        _enemyMovement.MoveToRandomPointWithMaxDist(5);    
                        _fieldOfView.SetupLight(Color.yellow);
                    }
                    else if (CurrentState == State.RunAway)
                    {
                        if (_enemyMovement.GetIsNavMeshStopped())
                        {
                            _enemyMovement.ToggleChaseSpeed(false);
                            CurrentState = State.IdleSearch;
                            _enemyMovement.MoveToRandomPointWithMaxDist(5);    
                            _fieldOfView.SetupLight(Color.yellow);
                        }
                    }
                }
            }

            /// <summary>
            /// Returns null if the AI cant see the player
            /// </summary>
            /// <returns></returns>
            public GameObject GetPlayer()
            {
                if (_fieldOfView.visibleTargets.Count > 0)
                {
                    return _fieldOfView.visibleTargets[0].gameObject;
                }

                return null;
            }

            public enum State
            {
                Idle,
                IdleSearch,
                HardSearch,
                Detect,
                Attack,
                Chase,
                RunAway
            }

            public void InvokeResponseForDamage()
            {
                var player = PlayerInventory.Instance.transform;
                _enemyMovement.GoToTarget(player);
                _enemyMovement.ToggleChaseSpeed(true);
                _hardSearchTimer = _enemyAiBrain.HardSearchTime;
                CurrentState = State.HardSearch;
                _fieldOfView.SetupLight(Color.magenta);
            }
        }
    }
}
