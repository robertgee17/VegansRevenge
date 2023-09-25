using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingPlacer : MonoBehaviour
{
    private gameManager gm;
    private InputManager im;
    private GameObject prefab;
    private Garrison prefabGarrison;
    private NavMeshObstacle prefabObstacle;
    private ShopDesigner shop;
    private float cost;
    private bool validPlace;
    public LayerMask groundMask;
    public Material invalidPlacementMat;
    public Material validPlacementMat;
    private List<Material[]> originalMaterials;

    private bool previousValid;
    private void Awake()
    {
        im = FindObjectOfType<InputManager>();
        originalMaterials = new List<Material[]>();
    }
    public void Start()
    {
        gm = gameManager.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (prefab == null)
            return;
        prefab.transform.position = getMousePosition();

        bool reDraw = false;
        Material placementMat;
        if (isValidPlace())
        {
            if (!previousValid)
            {
                reDraw = true;
                previousValid = true;
            }
            placementMat = validPlacementMat;
        }
        else
        {
            if (previousValid)
            {
                reDraw = true;
                previousValid = false;
            }
            placementMat = invalidPlacementMat;
        }
        if (reDraw)
        {
            foreach (MeshRenderer m in prefab.GetComponent<Building>().renderers)
            {
                Material[] mats = new Material[m.materials.Length];
                for (int i = 0; i < m.materials.Length; i++)
                {
                    mats[i] = placementMat;
                }
                m.materials = mats;
            }
        }
        
    }
    private bool isValidPlace()
    {
        //check if current location overlaps with other objects
        Collider collider = prefab.GetComponent<Collider>();
        foreach (var other in gm.getAllUnits())
        {
            if (other == null)
            {
                continue;
            }
            if (other.gameObject.GetComponent<Collider>() != null && other.gameObject != prefab && collider.bounds.Intersects(other.gameObject.GetComponent<Collider>().bounds))
            {
                return false;
            }
        }
        //check if the current location is outside the map
        RaycastHit hit;

        //get location slightly above the prefab's position since it's height is at 0
        Vector3 increasedHeightPos = new Vector3(prefab.transform.position.x, prefab.transform.position.y+2, prefab.transform.position.z);
        if (Physics.Raycast(increasedHeightPos, Vector3.down, out hit, groundMask))
        {
            if (hit.transform.gameObject.name != "PlayArea")
            {
                return false;
            }
        }
        return true;
    }
    public bool placeBuilding()
    {
        if (prefab == null||gm.selected!=null||!isValidPlace())
            return false;
        //check if it's overlapping with any other object
        for(int i = 0; i < originalMaterials.Count; i++)
        {
            MeshRenderer m = prefab.GetComponent<Building>().renderers[i];
            m.materials = originalMaterials[i];
        }
        if (prefab.CompareTag("Farmhouse"))
        {
            gm.farmhouse = prefab.GetComponent<Building>();
        }

        gm.coin -= cost;
        if (prefabGarrison != null)
        {
            prefabGarrison.enabled = true;
        }
        prefabObstacle.enabled = true;
        gm.nextState();
        if (gm.gameState != gameManager.state.GAMEPLAY)
        {
            if (!gm.isTutorial())
            {
                gm.selectTrader();
            }
        }
        clear();
        gm.getTrader().buySFX();
        return true;
    }
    public void selectBuilding(gameManager.BuildingType type,float price)
    {
        prefab = gm.createBuilding(type, getMousePosition()).gameObject;
        originalMaterials.Clear();
        foreach (MeshRenderer m in prefab.GetComponent<Building>().renderers)
        {
            //setup the original materials
            originalMaterials.Add(m.materials);
            //set the materials to a valid placement
            Material[] mats = new Material[m.materials.Length];
            for (int i = 0; i < m.materials.Length; i++)
            {
                mats[i] = validPlacementMat;
            }
            m.materials = mats;
        }
        previousValid = true;

        //disable the garrison on the building so that it doesn't accidentally garrison animals while dragging
        prefabGarrison = prefab.GetComponent<Garrison>();
        if (prefabGarrison != null)
        {
            //make sure awake and start are called in garrison before it is disabled
            prefabGarrison.Awake();
            prefabGarrison.Start();
            prefabGarrison.enabled = false;
        }
        prefabObstacle = prefab.GetComponent<NavMeshObstacle>();
        prefabObstacle.enabled = false;
        cost = price;
    }
    public void selectBuilding(gameManager.BuildingType type,gameManager.AnimalType garrisonType, float price)
    {
        selectBuilding(type, price);
        prefab.GetComponent<Garrison>().setValidGarrison(garrisonType);
    }
    public void cancelPlace()
    {
        if (prefab == null)
        {
            return;
        }
        Destroy(prefab);
        clear();

    }
    private Vector3 getMousePosition()
    {
        Vector3 pos = im.getMouseLocationOnMap();
        if (pos == null)
        {
            pos = new Vector3(0, 0, 0);
        }
        return pos;
    }

    private void clear()
    {
        prefab = null;
        prefabGarrison = null;
        prefabObstacle = null;
        cost = 0;
    }

    public void setShop(ShopDesigner sd)
    {
        shop = sd;
    }
}
