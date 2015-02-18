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
using System.ComponentModel;
using System.Linq;

namespace TinCan
{
    public static class InteractionTypeExtensions
    {
        private static readonly Type InteractionTypeType = typeof(InteractionType);

        private static readonly Dictionary<InteractionType, string> InteractionTypeValuesAndStrings =
            Enum.GetValues(typeof(InteractionType)).Cast<InteractionType>().ToDictionary(x => x, GetStringValue);

        private static readonly Dictionary<string, InteractionType> InteractionTypeStringsAndValues =
            Enum.GetValues(typeof(InteractionType)).Cast<InteractionType>().ToDictionary(x => InteractionTypeValuesAndStrings[x], x => x, StringComparer.InvariantCultureIgnoreCase);

        public static string GetInteractionTypeString(this InteractionType interactionType)
        {
            return InteractionTypeValuesAndStrings[interactionType];
        }

        public static InteractionType GetInteractionType(this string interactionTypeString)
        {
            InteractionType interactionType;
            if (!InteractionTypeStringsAndValues.TryGetValue(interactionTypeString.Trim(), out interactionType))
            {
                interactionType = InteractionType.Undefined;
            }

            return interactionType;
        }

        private static string GetStringValue(InteractionType interactionType)
        {
            var fieldInfo = InteractionTypeType.GetField(interactionType.ToString());
            var attributes = (AmbientValueAttribute[])fieldInfo.GetCustomAttributes(typeof(AmbientValueAttribute), true);

            return attributes.Any() ? attributes[0].Value as string : null;
        }
    }
}
