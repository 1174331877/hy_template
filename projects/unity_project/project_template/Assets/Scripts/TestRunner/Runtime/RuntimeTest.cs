using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class RuntimeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void RuntimeTestSimplePasses()
    {
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator RuntimeTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        Debug.Log(Time.time);
        yield return new WaitForSeconds(2f);
        Debug.Log(Time.time);
    }
}