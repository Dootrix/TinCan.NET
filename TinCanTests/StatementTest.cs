/*
    Copyright 2014 Rustici Software

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

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TinCan;

    [TestFixture]
    class StatementTest
    {
        [SetUp]
        public void Init()
        {
            Console.WriteLine("Running " + TestContext.CurrentContext.Test.FullName);
        }

        [Test]
        public void TestEmptyCtr()
        {
            Statement obj = new Statement();
            Assert.IsInstanceOf<Statement>(obj);
            Assert.IsNull(obj.id);
            Assert.IsNull(obj.actor);
            Assert.IsNull(obj.verb);
            Assert.IsNull(obj.target);
            Assert.IsNull(obj.result);
            Assert.IsNull(obj.context);
            Assert.IsNull(obj.version);
            Assert.IsNull(obj.timestamp);
            Assert.IsNull(obj.stored);

            StringAssert.AreEqualIgnoringCase("{\"version\":\"1.0.1\"}", obj.ToJSON());
        }

        [Test]
        public void TestJObjectCtrSubStatement()
        {
            JObject cfg = new JObject();
            cfg.Add("actor", Support.agent.ToJObject());
            cfg.Add("verb", Support.verb.ToJObject());
            cfg.Add("object", Support.subStatement.ToJObject());

            Statement obj = new Statement(cfg);
            Assert.IsInstanceOf<Statement>(obj);
            Assert.IsInstanceOf<SubStatement>(obj.target);
        }

        #region Validate

        [Test]
        public void Validate_NotEarlyReturnOnFailure_AllValidationFailureItems()
        {
            // Arrange
            var target = new Statement { actor = Support.agent, authority = new Agent { mbox = "wrong.mailbox.address" } };

            // Act
            // Assert
            Validate_ActAndAssert(
                target, 
                false,
                "Statement does not have a verb",
                "Statement does not have an object",
                "Mbox value wrong.mailbox.address must begin with mailto: prefix",
                "Mbox value wrong.mailbox.address is not a valid email address.");
        }

        [Test]
        public void Validate_EarlyReturnOnFailure_OnlyFirstValidationFailureItem()
        {
            // Arrange
            var target = new Statement { actor = Support.agent, authority = new Agent { mbox = "wrong.mailbox.address" } };

            // Act
            // Assert
            Validate_ActAndAssert(target, true, "Statement does not have a verb");
        }

        [Test]
        public void Validate_AuthorityHasValidationFailure_ValidationFailureItems()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent, 
                verb = Support.verb, 
                target = Support.subStatement, 
                authority = new Agent { mbox = "wrong.mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Mbox value wrong.mailbox.address must begin with mailto: prefix", "Mbox value wrong.mailbox.address is not a valid email address.");
        }

        [Test]
        public void Validate_AuthorityHasValidationFailureAndEarlyReturn_OneValidationFailureItem()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = Support.subStatement,
                authority = new Agent { mbox = "wrong.mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, true, "Mbox value wrong.mailbox.address must begin with mailto: prefix");
        }

        [Test]
        public void Validate_NullAuthority_NoValidationFailureItems()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = Support.subStatement,
                authority = null
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false);
        }

        [Test]
        public void Validate_AuthorityHasNoValidationFailure_NoValidationFailureItems()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = Support.subStatement,
                authority = new Agent { mbox = "mailto:good@mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false);
        }

        [Test]
        public void Validate_NoVerb_ValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = null,
                target = Support.subStatement,
                authority = new Agent { mbox = "mailto:good@mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Statement does not have a verb");
        }

        [Test]
        public void Validate_NoActor_ValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = null,
                verb = Support.verb,
                target = Support.subStatement,
                authority = new Agent { mbox = "mailto:good@mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Statement does not have an actor");
        }

        [Test]
        public void Validate_NoTarget_ValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = null,
                authority = new Agent { mbox = "mailto:good@mailbox.address" }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Statement does not have an object");
        }

        [Test]
        public void Validate_ActorHasMoreThanOneInverseFunctionalProperty_ActorValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = new Agent { mbox_sha1sum = "mboxsha1sum", openid = "agentopenid" },
                verb = Support.verb,
                target = Support.subStatement
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Exactly 1 inverse functional properties must be defined.  However, 2 are defined.");
        }

        [Test]
        public void Validate_TargetValidationFailure_TargetValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = new Activity { id = null }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Activity does not have an identifier");
        }

        [Test]
        public void Validate_ResultValidationFailure_ResultValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = Support.subStatement,
                result = new Result { score = new Score { scaled = -1.0 } }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Scaled score must be between 0.0 and 1.0");
        }

        [Test]
        public void Validate_ContextValidationFailure_ContextValidationFailure()
        {
            // Arrange
            var target = new Statement
            {
                actor = Support.agent,
                verb = Support.verb,
                target = Support.subStatement,
                context = new Context { instructor = new Agent { mbox_sha1sum = "mboxsha1sum", openid = "agentopenid", mbox = "mailto:mail@box.address" } }
            };

            // Act
            // Assert
            Validate_ActAndAssert(target, false, "Exactly 1 inverse functional properties must be defined.  However, 3 are defined.");
        }

        private static void Validate_ActAndAssert(Statement statement, bool isEarlyReturn, params string[] errors)
        {
            // Act
            var result = statement.Validate(isEarlyReturn);

            // Assert
            CollectionAssert.AreEqual(errors, result.Select(x => x.Error));
        }

        #endregion Validate
    }
}
