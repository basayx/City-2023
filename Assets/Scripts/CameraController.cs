using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform target;
    [SerializeField] private float smoothness = 0.05f;
    public Vector3 offset;
    public Vector3 rotation = new Vector3(45, 0, 0);
    public bool IsReady = true;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        DOTween.To(() => smoothness, x => smoothness = x, 20, 5);
    }

    public void Update()
    {
        if (!target || !IsReady)
        {
            return;
        }

        Vector3 targetPosition = target.position;
        transform.position = SmoothPosition(targetPosition + offset);
        transform.rotation = SmoothRotation(rotation);
    }

    private Vector3 SmoothPosition(Vector3 desiredPosition)
    {
        return Vector3.Lerp(transform.position, desiredPosition, smoothness * Time.deltaTime);
    }

    private Quaternion SmoothRotation(Vector3 desiredRotation)
    {
        return Quaternion.Lerp(transform.rotation, Quaternion.Euler(desiredRotation), smoothness * Time.deltaTime);
    }

    public void MoveToTargetPos(Transform targetPos, float duration = 2f)
    {
        IsReady = false;
        target = null;
        PlayerController.Instance.CanMove = false;

        transform.DORotate(targetPos.eulerAngles, duration);
        transform.DOMove(targetPos.position, duration).OnComplete(() =>
        {
            IsReady = true;
        });
    }

    public void BackToMainPos(float duration = 2f)
    {
        IsReady = false;
        transform.DORotate(rotation, duration);
        transform.DOMove(PlayerController.Instance.transform.position + offset, duration).OnComplete(() =>
        {
            target = PlayerController.Instance.transform;
            PlayerController.Instance.CanMove = true;

            IsReady = true;
        });
    }
}