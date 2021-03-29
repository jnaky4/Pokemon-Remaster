using UnityEngine;

public class GameplayLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask boundary;
    [SerializeField] LayerMask fovLayer;
    [SerializeField] LayerMask playerLayer;

    public static GameplayLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidLayer
    {
        get => solidObjectsLayer;
    }

    public LayerMask InteractLayer
    {
        get => interactableLayer;
    }

    public LayerMask GrassLayer
    {
        get => grassLayer;
    }

    public LayerMask BoundaryLayer
    {
        get => boundary;
    }

    public LayerMask FovLayer
    {
        get => fovLayer;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }
}
