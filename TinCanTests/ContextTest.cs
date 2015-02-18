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
    class ContextTest
    {
        #region Validate

        [Test]
        public void Validate_ErrorsNotEarlyReturn_ValidationFailures()
        {
            // Arrange
            var target = new Context
            {
                instructor = new Agent { mbox_sha1sum = "test", openid = "test" },
                team = new Agent { mbox = "mailto:mymail@mail.com", mbox_sha1sum = "test2", openid = "test2" },
                contextActivities = new ContextActivities { parent = new List<Activity> { new Activity { id = null } } },
            };

            // Act
            var result = target.Validate(false);

            // Assert
            CollectionAssert.AreEqual(
                new[] 
                { 
                    "Exactly 1 inverse functional properties must be defined.  However, 2 are defined.",
                    "Exactly 1 inverse functional properties must be defined.  However, 3 are defined.",
                    "Activity does not have an identifier"
                }, 
                result.Select(x => x.Error));
        }

        [Test]
        public void Validate_ErrorsEarlyReturn_ValidationFailures()
        {
            // Arrange
            var target = new Context
            {
                instructor = new Agent { mbox_sha1sum = "test", openid = "test" },
                team = new Agent { mbox = "test2", mbox_sha1sum = "test2", openid = "test2" },
                contextActivities = new ContextActivities { parent = new List<Activity> { new Activity { id = null } } },
            };

            // Act
            var result = target.Validate(true);

            // Assert
            CollectionAssert.AreEqual(new[] { "Exactly 1 inverse functional properties must be defined.  However, 2 are defined." }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_NoErrors_NoValidationFailures()
        {
            // Arrange
            var target = new Context
            {
                instructor = new Agent { mbox_sha1sum = "test" },
                team = new Agent { openid = "test2" },
                contextActivities = new ContextActivities { parent = new List<Activity> { new Activity { id = new Uri("http://my.uri") } } },
            };

            // Act
            var result = target.Validate(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        #endregion
    }
}
