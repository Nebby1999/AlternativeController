using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MineralOre : MonoBehaviour, IHarvesteable
{
    private SpriteRenderer _sprite;
    public MineralType Type => _type;
    [SerializeField] private MineralType _type;
    [SerializeField, Range(100, 1000)] private int _resources = 100;
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _sprite.color = _type == MineralType.Black ? Color.black : Color.red;
    }

    public void Harvest(int amount)
    {
        _resources -= amount;
        if(_resources <= 0) this.gameObject.SetActive(false);
    }
}
