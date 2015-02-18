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
using System.ComponentModel;

namespace TinCan
{
    /// <summary>
    /// The enumeration interaction types
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// The undefined interaction type
        /// </summary>
        [AmbientValue("")]
        Undefined,

        /// <summary>
        /// The true-false interaction type
        /// </summary>
        [AmbientValue("true-false")]
        TrueFalse,

        /// <summary>
        /// The choice interaction type
        /// </summary>
        [AmbientValue("choice")]
        Choice,

        /// <summary>
        /// The fill-in interaction type
        /// </summary>
        [AmbientValue("fill-in")]
        FillIn,

        /// <summary>
        /// The long-fill-in interaction type
        /// </summary>
        [AmbientValue("long-fill-in")]
        LongFillIn,

        /// <summary>
        /// The likert interaction type
        /// </summary>
        [AmbientValue("likert")]
        Likert,

        /// <summary>
        /// The matching interaction type
        /// </summary>
        [AmbientValue("matching")]
        Matching,

        /// <summary>
        /// The performance interaction type
        /// </summary>
        [AmbientValue("performance")]
        Performance,

        /// <summary>
        /// The sequencing interaction type
        /// </summary>
        [AmbientValue("sequencing")]
        Sequencing,

        /// <summary>
        /// The numeric interaction type
        /// </summary>
        [AmbientValue("numeric")]
        Numeric,

        /// <summary>
        /// The other interaction type
        /// </summary>
        [AmbientValue("other")]
        Other,
    }
}
