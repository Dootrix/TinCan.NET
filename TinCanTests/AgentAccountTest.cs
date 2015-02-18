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
    class AgentAccountTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        #region Validate

        [Test]
        public void Validate_NoHomePage_ValidationFailure()
        {
            // Arrange
            var target = new AgentAccount { homePage = null, name = "name" };

            // Act
            var result = target.Validate(false);

            // Assert
            CollectionAssert.AreEqual(new[] { "Homepage cannot be null" }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_NoName_ValidationFailure()
        {
            // Arrange
            var target = new AgentAccount { homePage = new Uri("http://myurl.com"), name = null };

            // Act
            var result = target.Validate(false);

            // Assert
            CollectionAssert.AreEqual(new[] { "Account name cannot be null" }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_IdExists_NoValidationFailure()
        {
            // Arrange
            var target = new AgentAccount { homePage = new Uri("http://myurl.com"), name = "name" };

            // Act
            var result = target.Validate(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        #endregion
    }
}
