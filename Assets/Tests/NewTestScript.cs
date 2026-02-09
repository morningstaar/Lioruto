using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // Un Test se comporte comme une méthode ordinaire
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Utilise la classe Assert pour vérifier des conditions
    }

    // Un UnityTest se comporte comme une coroutine en Play Mode. En Edit Mode tu peux utiliser
    // `yield return null;` pour sauter une frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Utilise la classe Assert pour vérifier des conditions.
        // Utilise yield pour sauter une frame.
        yield return null;
    }
}
