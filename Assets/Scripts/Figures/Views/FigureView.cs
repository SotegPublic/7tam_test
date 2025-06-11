using DG.Tweening;
using UnityEngine;
using Zenject;

public class FigureView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _iceShell;

    private IceAnimationConfig _iceAnimationConfig;
    private LayersConfig _layersConfig;
    private FiguresTypes _type;
    private bool _isInBar;

    public Rigidbody2D RigidBody2D => _rigidBody2D;
    public Collider2D Collider2D => _collider;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Transform ParentTransform => _parentTransform;
    public bool IsInBar => _isInBar;

    public FiguresTypes Type
    {
        get => _type;
        set => _type = value;
    }

    [Inject]
    public void Construct(IceAnimationConfig iceAnimationConfig, LayersConfig layersConfig)
    {
        _iceAnimationConfig = iceAnimationConfig;
        _layersConfig = layersConfig;
    }

    public void SetInBar()
    {
        _isInBar = true;
    }

    public void Clear()
    {
        _isInBar = false;
    }

    public void IceIt()
    {
        gameObject.layer = (int)Mathf.Log(_layersConfig.IceMask.value, 2);
        _iceShell.SetActive(true);
    }

    public void CrackIce()
    {
        var basePosition = _iceShell.transform.localPosition;

        _iceShell.transform.DOShakePosition(_iceAnimationConfig.Duration, _iceAnimationConfig.Force, _iceAnimationConfig.Vibrato, 0, fadeOut: false)
            .SetUpdate(UpdateType.Normal)
            .OnUpdate(() =>
            {
                _iceShell.transform.localPosition = new Vector3(
                    _iceShell.transform.localPosition.x,
                    basePosition.y,
                    basePosition.z
                );
            })
         .OnComplete(() => 
         {
             _iceShell.SetActive(false);
             _iceShell.transform.localPosition = basePosition;
             gameObject.layer = (int)Mathf.Log(_layersConfig.BaseMask.value, 2);
         });
    }

    public void ClearIce()
    {
        gameObject.layer = (int)Mathf.Log(_layersConfig.BaseMask.value, 2);
        _iceShell.SetActive(false);
    }
}
