using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
public class Plant : MonoBehaviour
{
    public DayManager _dayManager;
    public List<GameObject> _stages;
    private int _growthStage;
    public PlantScriptableObject _plant_SO;
    public bool readyToHarvest;
    private int _daysLeftForGrowth;
    public GameObject fruit;
    public GameObject deadPlant;
    public int daysBetweenHarvest;
    public bool dead;
    private bool _multipleHarvests;
    
    

    public bool watered;
    
    // Start is called before the first frame update
    void Start()
    {
        _dayManager.NewDay.AddListener(this.NewDay);
        _growthStage = 0;
        _stages[_growthStage].SetActive(true);
        _multipleHarvests = _plant_SO.multipleHarvests;
        daysBetweenHarvest = _plant_SO.daysBetweenHarvest;
    }

    private void Update()
    {
    }


    private void NewDay()
    {
        if (watered)
        {
            GrowPlant();
        }
        else
        {
            DryPlant();
        }
    }

    private void GrowPlant()
    {
        if (!readyToHarvest && _growthStage < (_plant_SO.stages - 1))
        {
            _stages[_growthStage].SetActive(false);
            _growthStage++;
            _stages[_growthStage].SetActive(true);
            if(_growthStage == _plant_SO.stages - 1 && _multipleHarvests)
            {
                _daysLeftForGrowth = daysBetweenHarvest;
            }
        }
        if (_growthStage == _plant_SO.stages - 1 && !_multipleHarvests)
        {
            readyToHarvest = true;
        }
        else if (_growthStage == _plant_SO.stages - 1 && _multipleHarvests && !dead)
        {
            if (_daysLeftForGrowth > 0)
            {
                _daysLeftForGrowth--;
            }
            else
            {
                fruit.SetActive(true);
                readyToHarvest = true;
            }
        }
        watered = false;
    }

    private void DryPlant()
    {
        if (Random.Range(0, 10) >= 4) return;
        _stages[_growthStage].SetActive(false);
        dead = true;
        fruit.SetActive(false);
        deadPlant.SetActive(true);
    }

   

    public void Harvest()
    {
        if (dead)
        {
            deadPlant.SetActive(false);
            Destroy(this.gameObject);
            return;
        }
        if (readyToHarvest && _multipleHarvests)
        {
            fruit.SetActive(false);
            readyToHarvest = false;
            _daysLeftForGrowth = daysBetweenHarvest;
        }
        else if (readyToHarvest)
        {
            _stages[_growthStage].SetActive(false);
            readyToHarvest = false;
            Destroy(this.gameObject);
        }
    }
}
