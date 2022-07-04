using UnityEngine;

namespace Thirtwo.Character
{
    public abstract class CharController : MonoBehaviour
    {
        protected bool isCharActive = false;
        protected Animator animator;
        [HideInInspector] public int floorNo = 0;
        protected Floor activeFloor;
        protected virtual void OnDestroy()
        {
            GameManager.OnGameFinished -= GameManager_OnGameFinished;
            GameManager.OnGameStarted -= GameManager_OnGameStarted;
        }
        protected abstract void Move();

        protected virtual void Init()
        {
            animator = transform.GetChild(1).GetComponent<Animator>();
            GetActiveFloor();
            GameManager.OnGameStarted += GameManager_OnGameStarted;
            GameManager.OnGameFinished += GameManager_OnGameFinished;
        }
        protected abstract void GameManager_OnGameFinished(bool isWin);

        protected virtual void GameManager_OnGameStarted()
        {
            isCharActive = true;
        }

        public virtual void GetActiveFloor()
        {
            activeFloor = FloorManager.Instance.GetFloor(floorNo);
        }
        
        
    }

}
