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
        gameManager = GameManager.Instance;
        //get Player
        player = game.GetComponentInChildren<Player>();
    }

    [UnityTest]
   public IEnumerator GamePrefabLoaded()
    {
        yield return new WaitForEndOfFrame();

        //after we wait
        Assert.NotNull(game, "Make sure game prefab is working");
    }

    [UnityTest]
    public IEnumerator PlayerExists()
    {
        yield return new WaitForEndOfFrame();

        Assert.NotNull(player, "Player dont exist");
    }


    [TearDown]
   public  void Teardown()
    {
        Object.Destroy(game);
    }

}
