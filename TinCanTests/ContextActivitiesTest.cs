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

    using NUnit.Framework;

    using TinCan;

    [TestFixture]
    class ContextActivitiesTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        [Test]
        public void Validate_Errors_ValidationFailures()
        {
            // Arrange
            var target = new ContextActivities
            {
                parent = new List<Activity> { new Activity { id = null } },
                category = new List<Activity> { new Activity { id = null } },
                grouping = new List<Activity> { new Activity { id = null } },
                other = new List<Activity> { new Activity { id = null } }
            };

            // Act
            var result = target.Validate(false);

            // Assert
            CollectionAssert.AreEqual(
                new[] 
                { 
                    "Activity does not have an identifier",
                    "Activity does not have an identifier",
                    "Activity does not have an identifier",
                    "Activity does not have an identifier"
                },
                result.Select(x => x.Error));
        }

        [Test]
        public void Validate_ErrorsEarlyReturn_FirstValidationFailure()
        {
            // Arrange
            var target = new ContextActivities
            {
                parent = new List<Activity> { new Activity { id = null }, new Activity { id = null } },
                category = new List<Activity> { new Activity { id = null } },
                grouping = new List<Activity> { new Activity { id = null } },
                other = new List<Activity> { new Activity { id = null } }
            };

            // Act
            var result = target.Validate(true);

            // Assert
            CollectionAssert.AreEqual(new[] { "Activity does not have an identifier" }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_NoErrors_NoValidationFailures()
        {
            // Arrange
            var target = new ContextActivities
            {
                parent = new List<Activity> { new Activity { id = new Uri("http://my.url") }, new Activity { id = new Uri("http://my.url") } },
                category = null,
                grouping = new List<Activity> { new Activity { id = new Uri("http://my.url") } },
                other = null
            };

            // Act
            var result = target.Validate(true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
    }
}
