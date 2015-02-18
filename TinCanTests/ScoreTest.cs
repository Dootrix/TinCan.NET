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
    class ScoreTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        [Test]
        public void Validate_Errors_ValidationFailureItems()
        {
            // Arange
            var target = new Score
            {
                scaled = -1,
                max = 0.5,
                min = 0.51,
                raw = 0.505
            };

            // Act
            var result = target.Validate(false);

            // Assert
            CollectionAssert.AreEqual(
                new[]
                {
                    "Scaled score must be between 0.0 and 1.0", 
                    "Max score cannot be lower than min score",
                    "Raw score cannot be greater than max score", 
                    "Raw score cannot be less than min score"
                },
                result.Select(x => x.Error));
        }

        [Test]
        public void Validate_ErrorsAndEarlyReturn_FirstValidationFailureItem()
        {
            // Arange
            var target = new Score
            {
                scaled = 2,
                max = 0.5,
                min = 0.51,
                raw = 0.505
            };

            // Act
            var result = target.Validate(true);

            // Assert
            CollectionAssert.AreEqual(new[] { "Scaled score must be between 0.0 and 1.0" }, result.Select(x => x.Error));
        }

        [Test]
        public void Validate_NoErrors_NoValidationFailureItems()
        {
            // Arange
            var target = new Score
            {
                scaled = 0.0,
                max = 3,
                min = -10,
                raw = -0.505
            };

            // Act
            var result = target.Validate(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
    }
}
