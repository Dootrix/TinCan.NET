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
    using System.Linq;

    using NUnit.Framework;

    using TinCan;

    [TestFixture]
    class ActivityTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        #region Validate

        [Test]
        public void Validate_NoId_ValidationFailure()
        {
            // Arrange
            var activity = new Activity { id = null };

            // Act
            var result = activity.Validate(false);

            // Assert
            CollectionAssert.AreEqual(new[] { "Activity does not have an identifier" }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_IdExists_NoValidationFailure()
        {
            // Arrange
            var activity = new Activity { id = new Uri("http://url.my") };

            // Act
            var result = activity.Validate(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        #endregion
    }
}
