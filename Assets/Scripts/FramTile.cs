using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmTile : MonoBehaviour
{
    public static FarmTile Instance { get; private set; }

    public Tilemap farmTilemap;
    public TileBase farmTile;
    public TileBase witheredTile; // A tile to show when a crop dies

    public float timeBeforeDeath = 10f; // Time before a crop dies if not watered

    public List<CropEntry> cropDataList;
    public Inventory inventory;

    private Transform playerTransform;

    // Crop Dictionary: stores crop type, growth stage, and watered stage
    private Dictionary<Vector3Int, (string cropType, int growthStage, int wateredStage)> crops = new Dictionary<Vector3Int, (string, int, int)>();
    // Dictionary to track time since last watered
    private Dictionary<Vector3Int, float> timeSinceLastWatered = new Dictionary<Vector3Int, float>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3Int tilePosition = GetTilePosition();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlantCrop(tilePosition);
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            WaterCrop(tilePosition);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HarvestCrop(tilePosition);
        }
        // Check if any crops should wither
        CheckForWithering();
    }

    Vector3Int GetTilePosition()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform not found!");
            return Vector3Int.zero;
        }

        Vector3 playerFeetPosition = playerTransform.position;
        playerFeetPosition.y -= 0.5f;
        return farmTilemap.WorldToCell(playerFeetPosition);
    }

    void PlantCrop(Vector3Int tilePosition)
    {
        TileBase currentTile = farmTilemap.GetTile(tilePosition);

        string seedType = inventory.selectedItem;
        string cropType = GetCropFromSeed(seedType);

        if (cropType == null)
        {
            Debug.Log("Invalid seed type!");
            return;
        }

        if (currentTile == farmTile && !crops.ContainsKey(tilePosition))
        {
            if (inventory.RemoveItem(seedType))
            {
                crops[tilePosition] = (cropType, 0, 0);
                timeSinceLastWatered[tilePosition] = 0f; // Start timer at 0
                farmTilemap.SetTile(tilePosition, GetCropStages(cropType)[0]);
                Debug.Log($"Planted {cropType} at {tilePosition}");
            }
            else
            {
                Debug.Log("No seeds available!");
            }
        }
        else
        {
            Debug.Log("Cannot plant here!");
        }
    }

    void WaterCrop(Vector3Int tilePosition)
    {
        if (!crops.ContainsKey(tilePosition))
        {
            Debug.Log("No crop to water!");
            return;
        }

        if (inventory.selectedItem != "Watering Can")
        {
            Debug.Log("You need a watering can to water crops!");
            return;
        }

        var cropData = crops[tilePosition];
        //string cropType = cropData.cropType;
        //int growthStage = cropData.growthStage;
        //int wateredStage = cropData.wateredStage;

        List<TileBase> wateredStages = GetWateredCropStages(cropData.cropType);

        if (cropData.wateredStage < 1) // Ensure watering is needed
        {
            farmTilemap.SetTile(tilePosition, wateredStages[cropData.growthStage]); // Change to watered version
            crops[tilePosition] = (cropData.cropType, cropData.growthStage, cropData.wateredStage + 1);
            Debug.Log($"Watered {cropData.cropType} at {tilePosition}, stage {cropData.growthStage}");

            if (cropData.wateredStage + 1 >= 1) // Ensures that the crop is fully watered
            {
                StartCoroutine(GrowCrop(tilePosition));
            }
        }
        else
        {
            Debug.Log("Already watered enough for this stage!");
        }
    }

    IEnumerator GrowCrop(Vector3Int tilePosition)
    {
        if (!crops.ContainsKey(tilePosition))
            yield break;

        var cropData = crops[tilePosition];
        

        if (cropData.wateredStage < 1) // Ensure it was watered before growing
        {
            Debug.Log("Crop needs to be watered before it can grow!");
            yield break;
        }

        yield return new WaitForSeconds(5f); // Growth delay

        List<TileBase> dryStages = GetCropStages(cropData.cropType);

        if (cropData.growthStage + 1 < dryStages.Count)
        {
            farmTilemap.SetTile(tilePosition, dryStages[cropData.growthStage + 1]);
            crops[tilePosition] = (cropData.cropType, cropData.growthStage + 1, 0); // Reset watering for the next stage
            Debug.Log($"{cropData.cropType} grew to stage {cropData.growthStage + 1} at {tilePosition}");
        }
    }
    void CheckForWithering()
    {
        List<Vector3Int> cropsToWither = new List<Vector3Int>();

        foreach (var crop in crops)
        {
            Vector3Int position = crop.Key;
            (string cropType, int growthStage, int wateredStage) cropData = crop.Value;

            // If crop is fully grown (harvestable), do not wither
            if (cropData.growthStage == GetCropStages(cropData.cropType).Count - 1)
                continue;

            // Only process crops that exist in the watering timer
            if (timeSinceLastWatered.ContainsKey(position))
            {
                timeSinceLastWatered[position] += Time.deltaTime;

                // If time exceeds limit and crop has not been watered, wither it
                if (timeSinceLastWatered[position] >= timeBeforeDeath && cropData.wateredStage == 0)
                {
                    cropsToWither.Add(position);
                }
            }
        }

        foreach (var position in cropsToWither)
        {
            WitherCrop(position);
        }
    }

    void WitherCrop(Vector3Int tilePosition)
    {
        if (crops.ContainsKey(tilePosition))
        {
            // Mark the crop as withered
            crops[tilePosition] = (crops[tilePosition].cropType, -1, -1); // Growth stage -1 means withered
            farmTilemap.SetTile(tilePosition, witheredTile);

            // Stop tracking watering time to prevent re-checking
            timeSinceLastWatered.Remove(tilePosition);

            Debug.Log($"Crop at {tilePosition} has withered!");
        }
    }


    void HarvestCrop(Vector3Int tilePosition)
    {
        if (inventory.selectedItem != "Basket")
        {
            Debug.Log("You need a Basket to harvest!");
            return;
        }

        if (crops.ContainsKey(tilePosition))
        {
            (string cropType, int growthStage, int wateredStage) cropData = crops[tilePosition];

            if (cropData.growthStage == GetCropStages(cropData.cropType).Count - 1)
            {
                // If fully grown, harvest normally
                inventory.AddItem(cropData.cropType, 1);
                Debug.Log($"Harvested {cropData.cropType} from {tilePosition}");
            }
            else if (cropData.growthStage == -1)
            {
                // If withered, allow harvesting but give nothing
                Debug.Log($"Harvested a withered crop from {tilePosition}, but it was useless.");
            }
            else
            {
                Debug.Log("Crop is not fully grown yet!");
                return;
            }

            // Remove crop and reset tile
            farmTilemap.SetTile(tilePosition, farmTile);
            crops.Remove(tilePosition);
            timeSinceLastWatered.Remove(tilePosition);
        }
    }

    public List<TileBase> GetCropStages(string cropType)
    {
        foreach (CropEntry entry in cropDataList)
        {
            if (entry.cropName == cropType)
            {
                return entry.growthStages;
            }
        }

        Debug.LogError($"No dry growth stages found for {cropType}!");
        return new List<TileBase>();
    }

    public List<TileBase> GetWateredCropStages(string cropType)
    {
        foreach (CropEntry entry in cropDataList)
        {
            if (entry.cropName == cropType)
            {
                return entry.wateredGrowthStages;
            }
        }

        Debug.LogError($"No watered growth stages found for {cropType}!");
        return new List<TileBase>();
    }

    private string GetCropFromSeed(string seedType)
    {
        if (seedType.EndsWith(" Seeds"))
        {
            return seedType.Replace(" Seeds", "");
        }

        Debug.LogError($"No crop found for seed {seedType}!");
        return null;
    }

    [System.Serializable]
    public class CropEntry
    {
        public string cropName;
        public List<TileBase> growthStages;
        public List<TileBase> wateredGrowthStages;
        
    }
}
