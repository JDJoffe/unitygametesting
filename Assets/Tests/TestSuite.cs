using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.TestTools;
using NUnit.Framework;
public class TestSuite 
{
   
    public GameObject game;
    private GameManager gameManager;
    private Player player;
   
   [SetUp]
   public void  Setup()
    {
       
        //load and spawn game prefab
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Game");
         game = Object.Instantiate(prefab);
        //get gamemanager
        gameManager = game.GetComponent<GameManager>();
        //get Player
        player = game.GetComponentInChildren<Player>();
    }

    [UnityTest]
   public IEnumerator GamePrefabLoaded()
    {
        yield return new WaitForEndOfFrame();

        Assert.NotNull(game, "Make sure game prefab is working");
    }

    [UnityTest]
    public IEnumerator PlayerExists()
    {
        yield return new WaitForEndOfFrame();

        Assert.NotNull(player, "Player dont exist");
    }

    [UnityTest]
    public IEnumerator ItemCollideWithPlayer()
    {
        //  Item item = gameManager.itemManager.GetItem(0);
        //  item.transform.position = player.transform.position;

        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");
        GameObject item = Object.Instantiate(itemPrefab, player.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(.1f);
        
        Assert.IsTrue(item == null);
    }

    [UnityTest]
    public IEnumerator ItemCollectedAndScoreAdded()
    {
       
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");
        GameObject item = Object.Instantiate(itemPrefab, player.transform.position, Quaternion.identity);
        int oldScore = gameManager.score;
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();
      
        int newScore = gameManager.score;

        Assert.IsTrue(oldScore <= newScore);
    }
    [TearDown]
   public  void Teardown()
    {
        Object.Destroy(game);
    }

}
