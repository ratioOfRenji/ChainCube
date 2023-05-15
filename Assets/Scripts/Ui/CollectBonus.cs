using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectBonus : MonoBehaviour
{
    [SerializeField] private IntValue _multyCubes;
    [SerializeField] private IntValue _bombCubes;
    [SerializeField] private GameObject _MultyCubeUi;
    [SerializeField] private GameObject _BombCubeUi;
    [SerializeField] private TMP_Text _multyCubesCount;
    [SerializeField] private TMP_Text _bombCubesCount;
    private void OnEnable()
    {
        CubeSpawner.OnBonusNumberReached += GenerateBonus;
    }
    private void OnDisable()
    {
        CubeSpawner.OnBonusNumberReached -= GenerateBonus;
    }
    private void GenerateBonus()
    {
        float rand = Random.value;
        if (rand < 0.5f) // 50% chance
        {
            _MultyCubeUi.SetActive(true);
        }
        else
        {
            _BombCubeUi.SetActive(true);
        }
    }
    public void collectMultyBonus()
    {
        _multyCubes.Value++;
        _MultyCubeUi.SetActive(false);
    }
    public void collectBombBonus()
    {
        _bombCubes.Value++;
        _BombCubeUi.SetActive(false);
    }
    private void Update()
    {
        _multyCubesCount.text = _multyCubes.Value.ToString();
        _bombCubesCount.text = _bombCubes.Value.ToString();
    }
}
