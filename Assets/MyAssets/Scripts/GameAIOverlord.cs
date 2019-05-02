using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameAIOverlord : MonoBehaviour
{
    VirusManager virus;
    EnemySpawner enemySpawner;
    Statistics stats;
    State currentState;
    public Transform player;
    public string state;
    public TextMeshProUGUI tutorialText;
    public Canvas tutorialCanvas;
    private void Start()
    {
        virus = FindObjectOfType<VirusManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        stats = Statistics.INSTANCE;
        currentState = new Tutorial(tutorialText);
        
    }

    private void Update()
    {
        currentState = currentState.Run(virus, enemySpawner,player);
        state = currentState.ToString();
        if(!(currentState is Tutorial))
        {
            tutorialCanvas.enabled = false;
        }
    }
    public abstract class State
    {
        public abstract State Run(VirusManager virusMan, EnemySpawner enSpawn, Transform player);
    }
    public class Tutorial : State
    {
        TextMeshProUGUI text;
        TutorialStates state;
        Vector3 lastPos;
        int numberOfAttacks;
        float timer = 1;
        public Tutorial(TextMeshProUGUI t)
        {
            this.text = t;
            state = TutorialStates.Movement;
        }
        public override State Run(VirusManager virusMan, EnemySpawner enSpawn, Transform player)
        {
            if(Input.GetKey(KeyCode.Tab))
            {
                state = TutorialStates.Done;
            }
            int numberOfEnemiesAlive = enSpawn.GetNubmerOfEnemies();
            switch (state)
            {
                case TutorialStates.Movement:
                    text.text = "Movement: \nW: Forward \nA: Left \nS: Backwards \nD: Right";
                    if (player.position != lastPos)
                    {
                        lastPos = player.position;
                        timer -= Time.deltaTime;
                    }
                    if(timer <= 0)
                    {
                        state = TutorialStates.Shooting;
                    }

                    break;
                case TutorialStates.Shooting:
                    text.text = "Shooting: \nMouse = Aim \nLeft Click = Shoot \n\nKilling Enemey gives you Data \nKill 2 Enemies";
                    
                    if (numberOfEnemiesAlive < 3)
                    {
                        //Select a enemy
                        int enemyIndexToSpawn = 0;
                        //Get a spawnOption
                        List<int> spawnOptions = enSpawn.GetListOfValideSpawnLocations(player, 10, true);
                        int selectedSpawnOption = spawnOptions[Random.Range(0, spawnOptions.Count)];
                        enSpawn.SpawnEnemy(enemyIndexToSpawn, selectedSpawnOption);
                    }

                    if (Statistics.INSTANCE.Kills >= 2)
                    {
                        state = TutorialStates.Drone;
                    }
                    break;
                case TutorialStates.Drone:
                    text.text = "Command Drone: \n1 = Follow me \n2 = Harvest Mode \n\nIn Harvest mode it will harvest Data from nodes.\nCollect 50 Data";

                    if (virusMan.GetNumberOfActiveViruses() < 5)
                    {
                        virusMan.ActivateANode();
                    }
                    if (numberOfEnemiesAlive < 2)
                    {
                        //Select a enemy
                        int enemyIndexToSpawn = 0;
                        //Get a spawnOption
                        List<int> spawnOptions = enSpawn.GetListOfValideSpawnLocations(player, 10, true);
                        int selectedSpawnOption = spawnOptions[Random.Range(0, spawnOptions.Count)];
                        enSpawn.SpawnEnemy(enemyIndexToSpawn, selectedSpawnOption);
                    }

                    if (Statistics.INSTANCE.TotalOfDataGatherd >= 50)
                    {
                        state = TutorialStates.Upgrade;
                    }
                    break;
                case TutorialStates.Upgrade:
                    text.text = "Upgrades: \nT = Open Upgrade Page \n\n Buy 1 Upgrade";
                    if (Statistics.INSTANCE.BoughtUpgrades >= 1)
                    {
                        state = TutorialStates.Done;
                        timer = 2;
                    }
                    break;
                case TutorialStates.Done:
                    text.text = "Tutorial Done";
                    if (timer <= 0)
                    {
                        text.text = string.Empty;
                        return new LowRisk();
                    }
                    timer -= Time.deltaTime;
                    break;
                default:
                    break;
            }
            return this;
        }
        enum TutorialStates
        {
            Movement,
            Shooting,
            Drone,
            Upgrade,
            Done
        }
    }

    public class HighRisk : State
    {
        float numberOfKillsThisState = 0;
        float numberOfEnemiesSpawned = 0;
        float MaxNumberOfEnemiesSpawn = 70;
        float alwaysThisAmount = 10;


        float timeBetweenSpawns = 3;
        float timeSpawner = 0;
        public HighRisk()
        {
            alwaysThisAmount = 10 + (Statistics.INSTANCE.Kills / 5) + (Statistics.INSTANCE.BoughtUpgrades);
            Enemy.OnKilled += OnKill;
        }
        private void OnKill()
        {
            numberOfKillsThisState++;
        }
        public override State Run(VirusManager virusMan, EnemySpawner enSpawn, Transform player)
        {
            //If Player have killed over 50 enemies we can go to lowRisk

            int numberOfEnemiesAlive = enSpawn.GetNubmerOfEnemies();
            if (numberOfEnemiesAlive < alwaysThisAmount && numberOfEnemiesAlive < MaxNumberOfEnemiesSpawn)
            {
                //Select a enemy
                int enemyIndexToSpawn = 0;
                //Get a spawnOption
                List<int> spawnOptions = enSpawn.GetListOfValideSpawnLocations(player, 10, true);
                int selectedSpawnOption = spawnOptions[Random.Range(0, spawnOptions.Count)];
                enSpawn.SpawnEnemy(enemyIndexToSpawn, selectedSpawnOption);
                numberOfEnemiesSpawned++;
            } else if (timeSpawner <= 0 && numberOfEnemiesSpawned < MaxNumberOfEnemiesSpawn )
            {
                int enemyIndexToSpawn = 0;
                //Get a spawnOption
                List<int> spawnOptions = enSpawn.GetListOfValideSpawnLocations(player, 10, true);
                int selectedSpawnOption = spawnOptions[Random.Range(0, spawnOptions.Count)];
                enSpawn.SpawnEnemy(enemyIndexToSpawn, selectedSpawnOption);
                numberOfEnemiesSpawned++;
                timeSpawner = timeBetweenSpawns;
            }
            else
            {
                timeSpawner -= Time.deltaTime;
            }

            int numberOfActiveViruses = virusMan.GetNumberOfActiveViruses();
            if (numberOfActiveViruses < 3)
            {
                virusMan.ActivateANode();
            }

            if (numberOfKillsThisState > 50)
            {
                Debug.Log("gameAIOverlord change state to LowRisk");
                return new LowRisk();
            }
            return this;
        }
        public override string ToString()
        {
            return "HighRisk";
        }
    }

    public class LowRisk : State
    {
        float dataGatherdThisState = 0;
        float dataToGather = 50;
        float minimumTime = 3;
        float minTimer = 3;
        float maxTimer = 60;
        public LowRisk()
        {
            PlayerVirusData.OnGatherData += OnDataGatherd;
            dataToGather = 10 - (Statistics.INSTANCE.BoughtUpgrades);
            minTimer = minimumTime;
        }

        public void OnDataGatherd(float amount)
        {
            dataGatherdThisState += amount;
        }
        public override State Run(VirusManager virusMan, EnemySpawner enSpawn,Transform player)
        {
            // If player have gatherd more then 10 data since this state started
            // Return the HighRisk State

            int numberOfEnemiesAlive = enSpawn.GetNubmerOfEnemies();
            if (numberOfEnemiesAlive < 3)
            {
                //Select a enemy
                int enemyIndexToSpawn = 0;
                //Get a spawnOption
                List<int> spawnOptions = enSpawn.GetListOfValideSpawnLocations(player, 10, true);
                int selectedSpawnOption = spawnOptions[Random.Range(0,spawnOptions.Count)];
                enSpawn.SpawnEnemy(enemyIndexToSpawn, selectedSpawnOption);
            }

            int numberOfActiveViruses = virusMan.GetNumberOfActiveViruses();
            if (numberOfActiveViruses < 4)
            {
                virusMan.ActivateANode();
            }

            if(dataGatherdThisState > dataToGather && minTimer < 0 && (Statistics.INSTANCE.BoughtUpgrades > 0 || maxTimer < 0))
            {
                Debug.Log("gameAIOverlord change state to HighRisk");
                return new HighRisk();
            }
            minTimer -= Time.deltaTime;
            maxTimer -= Time.deltaTime;
            return this;
        }
        public override string ToString()
        {
            return "LowRisk";
        }
    }
}
