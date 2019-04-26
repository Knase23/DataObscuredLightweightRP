//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//namespace Tests
//{
//    public class CustomValuesTest
//    {

//        const float BASE = 10;
//        const float MODIFIER = 100;
//        const float MULTIPLIER = 1.5f;
//        CustomValue testValue;
//        CustomValue testEffectValue;
//        [SetUp]
//        public void Setup()
//        {
//            testValue = new CustomValue(BASE);
//            testEffectValue = new CustomValue(modifier: MODIFIER, multiplier: MULTIPLIER);
//        }
//        // A Test behaves as an ordinary method
//        [Test]
//        public void PlusOperator()
//        {
//            testValue += testEffectValue;

//            Assert.AreEqual(BASE, testValue.Base);
//            Assert.AreEqual(MODIFIER, testValue.Modifier);
//            Assert.AreEqual(MULTIPLIER, testValue.Multiplier);
//        }
//        [Test]
//        public void MinusOperator()
//        {
//            testValue -= testEffectValue;

//            Assert.AreEqual(BASE, testValue.Base);
//            Assert.AreEqual(-MODIFIER, testValue.Modifier);
//            Assert.AreEqual(0.5, testValue.Multiplier);
//        }

//        [Test]
//        public void Result()
//        {

//            float expectedValue = (BASE + MODIFIER) * MULTIPLIER;
            
//            testValue += testEffectValue;

//            float result = testValue.Result();
//            Assert.AreEqual(expectedValue, result);

//        }


//    }
//}
