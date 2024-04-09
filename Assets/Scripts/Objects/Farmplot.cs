
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Farmplot : Interactable, IInteractable
{
   public DayManager _dayManager;
   public Plant plantPrefab;
   public Plant currentPlant;
   public bool watered;
   public MeshRenderer ground;
   public Material DryGround;
   public Material WetGround;
   public Material PlainGround;

   public bool tilled;

   private void Start()
   {
      
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.P))
      {
         plant(plantPrefab);
      }
      if (Input.GetKeyDown(KeyCode.X))
      {
         WaterPlot();
      }

      ground.material = !tilled? PlainGround : watered? WetGround : DryGround;
   }

   public void plant(Plant plant)
   {
      if (!tilled || currentPlant != null) return;
      currentPlant = Instantiate(plant);
      currentPlant._dayManager = _dayManager;
      currentPlant.watered = watered;
   }

   public void WaterPlot()
   {
      if (!tilled) return;
      watered = true;
      if (currentPlant != null)
      {
         currentPlant.watered = true;
      }
   }

   public void TillPlot()
   {
      if (_dayManager == null)
      {
         _dayManager = DayManager.Instance;
      }
      _dayManager.NewDay.AddListener(this.NewDay);
      tilled = true;
   }

   private void NewDay()
   {
      if (currentPlant == null)
      {
         if (Random.Range(0, 10) > 4)
         {
            tilled = false;
         }
      }
      watered = false;
   }

   public void Interact(ItemScriptableObject item = null)
   {
      if (currentPlant != null)
      {
         if(currentPlant.readyToHarvest || currentPlant.dead)
         {
            currentPlant.Harvest();
            return;
         }
      }

      if (item is not ToolScriptableObject tool)
         return;
      switch (tool.type)
      {
         case ToolScriptableObject.EquippedItemType.Hoe:
            TillPlot();
            break;
         case ToolScriptableObject.EquippedItemType.Seed:
            plant(tool.seed.plant);
            break;
         case ToolScriptableObject.EquippedItemType.WateringCan:
            WaterPlot();
            break;
         
      }
   }
   
   
}
