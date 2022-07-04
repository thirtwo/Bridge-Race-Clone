using UnityEngine;
using UnityEngine.AI;
namespace Thirtwo.Character
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AIBrickController))]
    public class EnemyController : CharController
    {
        private NavMeshAgent agent;
        private AIBrickController brickController;
        protected Ladder activeLadder = null;
        [SerializeField] private Transform activeFollowingObject = null;

        [Header("AI Settings")]
        [SerializeField] private int[] desiredBrickCount = new int[2] { 5, 12 };
        private int neededBrickCount = 0;
        private void Awake()
        {
            Init();
        }
        private void Update()
        {
            if (!isCharActive) return;
            Move();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            brickController.OnCollect -= BrickController_OnCollect;
            brickController.OnDrop -= BrickController_OnDrop;
        }
        protected override void Init()
        {
            base.Init();
            agent = GetComponent<NavMeshAgent>();
            brickController = GetComponent<AIBrickController>();
            neededBrickCount = Random.Range(desiredBrickCount[0], desiredBrickCount[1]);
            brickController.OnCollect += BrickController_OnCollect;
            brickController.OnDrop += BrickController_OnDrop;
        }

        private void BrickController_OnCollect()
        {
            neededBrickCount--;
            activeFollowingObject = null;
            agent.velocity = Vector3.zero;
        }
        private void BrickController_OnDrop()
        {
            activeFollowingObject = null;
        }


        protected override void Move()
        {
            if (activeFollowingObject == null)
            {
                GetFollowingObject();
                return;// secure 1 frame wait
            }
            if (Vector3.Distance(agent.destination, transform.position) < 0.2f)
            {
                activeFollowingObject = null;
                animator.SetBool("IsRunning", false);
            }
            else
            {
                agent.SetDestination(activeFollowingObject.position);
                animator.SetBool("IsRunning", true);
            }
        }
        private void GetFollowingObject()
        {
            if (neededBrickCount > 0)
            {
                FindClosesetBrick();
                return;
            }
            if (activeLadder == null)
            {
                activeLadder = activeFloor.GetLadder();
                activeFollowingObject = activeLadder.colliderPoints[(int)brickController.color].transform;
                return;
            }
            if (brickController.bricks.Count > 0)
            {
                activeFollowingObject = activeLadder.colliderPoints[(int)brickController.color].transform;
                return;
            }
            neededBrickCount = Random.Range(desiredBrickCount[0], desiredBrickCount[1]);
            FindClosesetBrick();
        }
        private void FindClosesetBrick()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer != 6) continue;
                if (colliders[i].TryGetComponent<Brick>(out var brick))
                {
                    if (brick.color == brickController.color && !brick.IsCollected)
                    {
                        activeFollowingObject = brick.transform;
                        return;
                    }
                }
            }
            var bricks = activeFloor.bricks;
            for (int i = 0; i < bricks.Count; i++)
            {
                if (bricks[i].color == brickController.color && !bricks[i].IsCollected)
                {
                    activeFollowingObject = bricks[i].transform;
                    return;
                }
            }
        }
        public override void GetActiveFloor()
        {
            base.GetActiveFloor();
            if (activeFloor == null)
            {
                GameManager.FinishGame(false,transform);
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("Win");
                return;
            }
            neededBrickCount = Random.Range(desiredBrickCount[0], desiredBrickCount[1]);
            activeLadder = null;
            activeFollowingObject = null;
        }
        protected override void GameManager_OnGameFinished(bool isWin)
        {
            isCharActive = false;
            agent.isStopped = true;
            agent.enabled = false;
            animator.SetBool("IsRunning", false);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
