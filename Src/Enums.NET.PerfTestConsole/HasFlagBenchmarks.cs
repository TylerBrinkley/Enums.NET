#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace EnumsNET.Tests.Benchmarks;

[SimpleJob(RuntimeMoniker.Net48), SimpleJob(RuntimeMoniker.Net80)]
public class HasFlagBenchmarks
{
    private readonly AttributeTargets[] _attributeTargets;
    private readonly AttributeTargets[] _allAttributeTargets;
    private readonly Type _enumType;

    public HasFlagBenchmarks()
    {
        _enumType = typeof(AttributeTargets);
        _attributeTargets = new AttributeTargets[15];
        _attributeTargets[0] = (AttributeTargets)0;
        _attributeTargets[1] = (AttributeTargets)1;
        for (var i = 2; i < _attributeTargets.Length; ++i)
        {
            _attributeTargets[i] = (AttributeTargets)(1 << (i - 1)) | (AttributeTargets)(1 << (i - 2));
        }
        _allAttributeTargets = (AttributeTargets[])Enum.GetValues(typeof(AttributeTargets));
    }

    [Benchmark]
    public bool Enum_HasFlag()
    {
        var result = false;
        foreach (var attributeTargets in _attributeTargets)
        {
            foreach (var otherAttributeTargets in _allAttributeTargets)
            {
                result |= attributeTargets.HasFlag(otherAttributeTargets);
            }
        }
        return result;
    }

    [Benchmark]
    public bool NonGenericFlagEnums_HasAllFlags()
    {
        var result = false;
        foreach (var attributeTargets in _attributeTargets)
        {
            foreach (var otherAttributeTargets in _allAttributeTargets)
            {
                result |= FlagEnums.HasAllFlags(_enumType, attributeTargets, otherAttributeTargets);
            }
        }
        return result;
    }

    [Benchmark]
    public bool FlagEnums_HasAllFlags()
    {
        var result = false;
        foreach (var attributeTargets in _attributeTargets)
        {
            foreach (var otherAttributeTargets in _allAttributeTargets)
            {
                result |= attributeTargets.HasAllFlags(otherAttributeTargets);
            }
        }
        return result;
    }

    [Benchmark]
    public bool FlagEnums_HasAnyFlags()
    {
        var result = false;
        foreach (var attributeTargets in _attributeTargets)
        {
            foreach (var otherAttributeTargets in _allAttributeTargets)
            {
                result |= attributeTargets.HasAnyFlags(otherAttributeTargets);
            }
        }
        return result;
    }

    [Benchmark(Baseline = true)]
    public bool Manual_HasFlag()
    {
        var result = false;
        foreach (var attributeTargets in _attributeTargets)
        {
            foreach (var otherAttributeTargets in _allAttributeTargets)
            {
                result |= (attributeTargets & otherAttributeTargets) == otherAttributeTargets;
            }
        }
        return result;
    }
}