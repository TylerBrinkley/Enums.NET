// Enums.NET
// Copyright 2016 Tyler Brinkley. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.ComponentModel;

namespace EnumsNET.Tests.TestEnums
{
    // Duplicate example
    public enum NumericOperator
    {
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        Equals = 0,
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        NotEquals,
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        GreaterThan,
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        LessThan,
        [Primary]
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        [Description("Greater than or equal")]
        GreaterThanOrEquals,
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        [Description("Not less than")]
        NotLessThan = GreaterThanOrEquals,
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        LessThanOrEquals,
        [Primary]
        [NumericOperatorType(NumericOperatorType.OneNumber)]
        NotGreaterThan = LessThanOrEquals,
        [NumericOperatorType(NumericOperatorType.TwoNumbers)]
        Between,
        [NumericOperatorType(NumericOperatorType.TwoNumbers)]
        NotBetween
    }

    public enum NumericOperatorType
    {
        OneNumber = 0,
        TwoNumbers
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NumericOperatorTypeAttribute : Attribute
    {
        public NumericOperatorType Type { get; }

        public NumericOperatorTypeAttribute(NumericOperatorType type)
        {
            Type = type;
        }
    }
}
