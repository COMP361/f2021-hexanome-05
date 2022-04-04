using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// using Models;

public class UpdateTests
{
    private void CreateModel() {

    }

    [Test]
    public void UpdateTestsSimplePasses()
    {
        //Board board = new Board();
        //Game gameModel = new Game();
        //Assert.Equals(0, 0);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator UpdateTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
