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
    public void Setup()
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
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");
        GameObject item = Object.Instantiate(itemPrefab, player.transform.position, Quaternion.identity);
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(item == null);
    }
    [UnityTest]
    public IEnumerator ItemAnimPlay()
    {
        Vector3 pos = new Vector3(.5f, 1, 3.5f);
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Entities/Item");
        GameObject item = Object.Instantiate(itemPrefab, pos, Quaternion.identity);
        Quaternion prevrot = item.transform.rotation;
        yield return new WaitForEndOfFrame();
        Quaternion newrot = item.transform.rotation;
        Assert.IsTrue(newrot != prevrot);
    }
    [UnityTest]
    public IEnumerator PlayerMove()
    {
        float prevpos = Vector3.Magnitude(player.transform.position);
        player.rigid.AddForce(2f, 0, 0, ForceMode.VelocityChange);
        yield return new WaitForFixedUpdate();
        float newpos = Vector3.Magnitude(player.transform.position);
        Assert.IsTrue(newpos != prevpos);
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
    public void Teardown()
    {
        Object.Destroy(game);
    }

}
