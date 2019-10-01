using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    // Start is called before the first frame update
    public static BuildingPlacer BL;
    public InputMaster Controlls;
    private int GameTick;
    private int NextSpawn;
    private void Awake()
    {
        BL = this;
        Controlls = new InputMaster();
        Controlls.Player.PlaceBuilding.performed += _ => PlaceB();
    }






    public List<GameObject> Spawned = new List<GameObject>();
    public List<GameObject> Buildings = new List<GameObject>();
    public List<Transform> SpPositions = new List<Transform>();
    public List<Transform> ReplaceBuildings = new List<Transform>();
    void Start()
    {
        GameTick = 0;
       var AM = transform.childCount;
        for (int i = 0; i < AM; i++)
        {
            SpPositions.Add(transform.GetChild(i));
         //   if (i % 2 == 1)
         //   {
         //    //  Destroy(transform.GetChild(i).gameObject);
        //   }
        }

        Spawn();
        FindObjectOfType<TileAutomata>().doSim(6);
        AIBuild();
    }

    public void Spawn ()
    {
        foreach (Transform i in SpPositions)
        {
            var RB = Random.Range(0, Buildings.Count);
            var SObj = Instantiate(Buildings[RB], i.position, i.rotation);
            Spawned.Add(SObj);
        }
    }


    /// <summary>
    /// PlayerBuilding
    /// </summary>
    public Animator Tray;
    public bool Building;
    public GameObject SelectedBuilding;
    public GameObject FinishedBuilding;
    public GameObject GhostPlaced;
    public int CostBuilding;

    void Update()
    {
      
        // if (i)
        if (Building == true)
        {
            if (Input.GetMouseButtonDown (0))
            {
                PlaceB();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Building = false;
                Tray.SetBool("TrayState", false);
            }
            //we are building
            if (GhostPlaced == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Physics.Raycast(ray))
                {
                    GhostPlaced = Instantiate(SelectedBuilding, worldPosition, transform.rotation);
                    if (GhostPlaced.GetComponent<Collider2D>())
                    {
                        GhostPlaced.GetComponent<Collider2D>().enabled = false;
                    }
                   var SL = GhostPlaced.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer VI in SL)
                    {
                        VI.sortingLayerName = "Placement";
                    }
                    //set its display data shit
                    //get all the correct data first
                    //clean, dirty, cost
                    var dir = GhostPlaced.GetComponent<PollutionEffector>().PollutionDirty;
                    var Cir = GhostPlaced.GetComponent<PollutionEffector>().PollutionClean;
                    var HP = GhostPlaced.GetComponent<HappinessEffector>().HappyIncrease;
                    GhostPlaced.GetComponentInChildren<PlaceData>().SetThem(CostBuilding, Cir, dir, HP);
                }
                }  
            else
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GhostPlaced.transform.position = worldPosition;

                // LayerMask mask = LayerMask.GetMask ("Building");
                //check if we can place and change colour of object
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                var FI = GhostPlaced.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer iii in FI)
                {
                    iii.color = Color.red;
                }

                // If it hits something...
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Building")
                    {
                        //var FI =  GhostPlaced.GetComponentsInChildren<SpriteRenderer>();
                        //what would our current cash take cost of this thing
                        var CC = Economy.money;

                        if ((CC - CostBuilding) >= 0)
                        {
                            foreach (SpriteRenderer iii in FI)
                            {
                                iii.color = Color.green;
                            }
                            GhostPlaced.transform.position = hit.transform.position;
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Building = true;
                Tray.SetBool("TrayState", true);
            }
            if (GhostPlaced != null)
            {
                Destroy(GhostPlaced);
                GhostPlaced = null;
            }
        }
    }

    public void PlaceB()
    {
        Debug.Log("Placing Building NOW!!!!");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

      //  var FI = GhostPlaced.GetComponentsInChildren<SpriteRenderer>();
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Building")
            {
                var CC = Economy.money;

                if ((CC - CostBuilding) >= 0)
                {
                    Instantiate(FinishedBuilding, hit.transform.position, transform.rotation);
                    Destroy(hit.transform.gameObject);
                    Building = false;
                    Destroy(GhostPlaced);
                    GhostPlaced = null;
                    Economy.money -= CostBuilding;
                }
            }
        }
    }



    /// <summary>
    /// AI building, allows the AI to build, between every 3-10 seconds
    /// </summary>
  
    public void AIBuild ()
    {
        if (NextSpawn == GameTick)
        {
            //find all buildings
            var BUI = FindObjectsOfType<Building>();


            //choose build
            var cO = BUI[Random.Range(0, BUI.Length)];
            //chosen building, now replace it
            var NEW = ReplaceBuildings[Random.Range(0, ReplaceBuildings.Count)];
            var SL = Instantiate(NEW, cO.transform.position, transform.rotation);
            SL.GetComponent<HappinessEffector>().HappyDecrease = SL.GetComponent<HappinessEffector>().HappyIncrease;
            SL.GetComponent<HappinessEffector>().HappyIncrease = 0;
            Destroy(cO.gameObject);
            //choose a crap building to replace it with


            //spawn a new 
            //figure out how long till next build (in ticks)
            var NB = Random.Range(1, 7);

            NextSpawn += NB;
           // Invoke("AIBuild", NB);
        }
        GameTick++;
    }
}
