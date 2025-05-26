using UnityEngine;

public class FigureView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _parentTransform;

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

    public void SetInBar()
    {
        _isInBar = true;
    }

    public void Clear()
    {
        _isInBar = false;
    }
}
