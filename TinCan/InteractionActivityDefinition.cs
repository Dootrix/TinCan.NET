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
using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

using TinCan.Json;

namespace TinCan
{
    public class InteractionActivityDefinition : ActivityDefinition
    {
        public const String INTERACTION_DEFINITION_TYPE = "http://adlnet.gov/expapi/activities/cmi.interaction";

        public InteractionType interactionType { get; set; }
        public List<String> correctResponsesPattern { get; set; }
        public List<InteractionComponent> choices { get; set; }

        public InteractionActivityDefinition()
        {
        }

        public InteractionActivityDefinition(StringOfJSON json)
            : this(json.toJObject())
        {
        }

        public InteractionActivityDefinition(JObject jobj)
            : base(jobj)
        {
            if (jobj["interactionType"] != null)
            {
                interactionType = jobj.Value<String>("interactionType").GetInteractionType();
            }

            if (jobj["correctResponsesPattern"] != null)
            {
                correctResponsesPattern = jobj["correctResponsesPattern"].Values<String>().ToList();
            }

            if (jobj["choices"] != null)
            {
                choices = jobj["choices"].Select(x => (InteractionComponent)x).ToList();
            }
        }

        public override JObject ToJObject(TCAPIVersion version)
        {
            var result = base.ToJObject(version);

            if (interactionType != null)
            {
                result.Add("interactionType", interactionType.GetInteractionTypeString());
            }

            if (correctResponsesPattern != null)
            {
                var jCorrectResponsesPattern = new JArray();
                foreach (var correctResponse in correctResponsesPattern)
                {
                    jCorrectResponsesPattern.Add(correctResponse);
                }

                result.Add("correctResponsesPattern", jCorrectResponsesPattern);
            }

            if (choices != null)
            {
                var jChoices = new JArray();
                foreach (var choice in choices)
                {
                    jChoices.Add(choice.ToJObject(version));
                }

                result.Add("choices", jChoices);
            }

            return result;
        }

        public static explicit operator InteractionActivityDefinition(JObject jobj)
        {
            return new InteractionActivityDefinition(jobj);
        }
    }
}
