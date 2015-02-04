/*
    Copyright 2015 Rustici Software

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
namespace TinCanTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TinCan;

    [TestFixture]
    class InteractionActivityDefinitionTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        [Test]
        public void ToJObject_AllProperties_Serialized()
        {
            // Arrange
            var interactionComponent1 = new InteractionComponent { id = "ic_id1", description = new LanguageMap(new Dictionary<string, string> { { "te-st", "ic1" } }) };
            var interactionComponent2 = new InteractionComponent { id = "ic_id2", description = new LanguageMap(new Dictionary<string, string> { { "te-st", "ic2" } }) };

            var target = new InteractionActivityDefinition
                             {
                                 name = new LanguageMap(new Dictionary<string, string> { { "te-st", "testname" } }),
                                 interactionType = InteractionType.Numeric,
                                 description = new LanguageMap(new Dictionary<string, string> { { "te-st", "testdescription" } }),
                                 type = new Uri("http://test.type"),
                                 moreInfo = new Uri("http://more.info"),
                                 correctResponsesPattern = new List<string> { "1", "6" },
                                 choices = new List<InteractionComponent> { interactionComponent1, interactionComponent2 }
                             };

            // Act
            var result = target.ToJObject();

            // Assert
            var name = result["name"].Value<string>("te-st");
            var interactionType = result.Value<string>("interactionType");
            var description = result["description"].Value<string>("te-st");
            var type = result.Value<string>("type");
            var moreInfo = result.Value<string>("moreInfo");
            var correctResponsePattern = result["correctResponsesPattern"].Values<string>().ToList();
            var choices = result["choices"].ToList();

            Assert.AreEqual("testname", name);
            Assert.AreEqual("numeric", interactionType);
            Assert.AreEqual("testdescription", description);
            Assert.AreEqual("http://test.type/", type);
            Assert.AreEqual("http://more.info/", moreInfo);
            Assert.AreEqual(2, correctResponsePattern.Count);
            Assert.AreEqual("1", correctResponsePattern[0]);
            Assert.AreEqual("6", correctResponsePattern[1]);
            Assert.AreEqual(2, choices.Count);
            Assert.AreEqual("ic_id1", choices[0].Value<string>("id"));
            Assert.AreEqual("ic_id2", choices[1].Value<string>("id"));
            Assert.AreEqual("ic1", choices[0]["description"].Value<string>("te-st"));
            Assert.AreEqual("ic2", choices[1]["description"].Value<string>("te-st"));
        }

        [Test]
        public void Constructor_FromJObject_InstanceWithValues()
        {
            // Arrange
            var jobject = new JObject();
            
            var name = new JObject();
            name["te-st"] = "testname";

            var description = new JObject();
            description["te-st"] = "testdescription";

            var choice1description = new JObject();
            choice1description["te-st"] = "ic1";
            var choice2description = new JObject();
            choice2description["te-st"] = "ic2";

            var choice1 = new JObject();
            choice1.Add("id", "ic_id1");
            choice1.Add("description", choice1description);

            var choice2 = new JObject();
            choice2.Add("id", "ic_id2");
            choice2.Add("description", choice2description);

            var choices = new JArray();
            choices.Add(choice1);
            choices.Add(choice2);

            var correctResponsePattern = new JArray();
            correctResponsePattern.Add("1");
            correctResponsePattern.Add("6");

            jobject.Add("name", name);
            jobject.Add("interactionType", "true-false");
            jobject.Add("description", description);
            jobject.Add("type", "http://test.type/");
            jobject.Add("moreInfo", "http://more.info/");
            jobject.Add("correctResponsesPattern", correctResponsePattern);
            jobject.Add("choices", choices);

            // Act
            var target = new InteractionActivityDefinition(jobject);

            // Assert
            Assert.AreEqual("testname", target.name.ToJObject().Value<string>("te-st"));
            Assert.AreEqual(InteractionType.TrueFalse, target.interactionType);
            Assert.AreEqual("testdescription", target.description.ToJObject().Value<string>("te-st"));
            Assert.AreEqual(new Uri("http://test.type/"), target.type);
            Assert.AreEqual(new Uri("http://more.info/"), target.moreInfo);
            Assert.AreEqual(2, target.correctResponsesPattern.Count);
            Assert.AreEqual("1", target.correctResponsesPattern[0]);
            Assert.AreEqual("6", target.correctResponsesPattern[1]);
            Assert.AreEqual(2, target.choices.Count);
            Assert.AreEqual("ic_id1", target.choices[0].id);
            Assert.AreEqual("ic_id2", target.choices[1].id);
            Assert.AreEqual("ic1", target.choices[0].description.ToJObject().Value<string>("te-st"));
            Assert.AreEqual("ic2", target.choices[1].description.ToJObject().Value<string>("te-st"));
        }
    }
}
