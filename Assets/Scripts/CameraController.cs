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
        if (!target)
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
}