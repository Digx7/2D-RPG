using UnityEngine;

[ExecuteInEditMode]
public class Parallax : MonoBehaviour
{
    public Channel RequestStartParallaxChannel;
    public Vector3Channel OnUpdateCameraLocationChannel;
    public bool parallaxOnXOnly = false;
    public bool isForground = false;
    [Range(0.0f, 2.0f)]
    public float distanceFromMiddleGround = 0;
    public float minXdelta = -200;
    public float maxXdelta = 200;
    public float minYdelta = -200;
    public float maxYdelta = 200;

    private Vector3 m_lastCameraPosition;
    private bool shouldParallax = false;
    private float minX, maxX, minY, maxY;

    private Vector3 m_startingPos;
    private bool isInEditorDebug = false;

    public void OnEnable()
    {
        RequestStartParallaxChannel.channelEvent.AddListener(Setup);
        OnUpdateCameraLocationChannel.channelEvent.AddListener(ParallaxElement);
    }

    public void OnDisable()
    {
        RequestStartParallaxChannel.channelEvent.RemoveListener(Setup);
        OnUpdateCameraLocationChannel.channelEvent.RemoveListener(ParallaxElement);

    }

    public void Setup()
    {
        // m_lastCameraPosition = Camera.main.transform.position;
        m_lastCameraPosition = transform.position;

        minX = transform.position.x - minXdelta;
        maxX = transform.position.x - maxXdelta;
        minY = transform.position.y - minYdelta;
        maxY = transform.position.y - maxYdelta;

        shouldParallax = true;
    }

    void ParallaxElement(Vector3 cameraPosition)
    {
        if (!shouldParallax) return;
        if (m_lastCameraPosition == cameraPosition) return;

        float cameraDeltaX = m_lastCameraPosition.x - cameraPosition.x;
        float cameraDeltaY = 0.0f;
        if (!parallaxOnXOnly) cameraDeltaY = m_lastCameraPosition.y - cameraPosition.y;

        Vector3 cameraDelta = new Vector3(cameraDeltaX, cameraDeltaY, 0);

        if (isForground)
        {
            cameraDelta *= -1;
            cameraDelta *= distanceFromMiddleGround;
        }
        else
        {
            cameraDelta *= distanceFromMiddleGround / 2f;
        }

        m_lastCameraPosition = cameraPosition;

        transform.position -= cameraDelta;
    }

#if UNITY_EDITOR

    [ContextMenu("Start Edtior Visualizing")]
    public void StartEditorVisualizing()
    {
        m_startingPos = transform.position;
        isInEditorDebug = true;

        Setup();
    }

    [ContextMenu("Stop Edtior Visualizing")]
    public void StopEditorVisualizing()
    {
        transform.position = m_startingPos;
        isInEditorDebug = false;

        shouldParallax = false;
    }

    public void Awake()
    {
        StopEditorVisualizing();
    }

    public void Update()
    {
        if (Application.isEditor && !Application.isPlaying && isInEditorDebug)
        {
            Vector3 fakeCameraPos = Camera.main.transform.position;

            ParallaxElement(fakeCameraPos);
        }
    }

#endif
}