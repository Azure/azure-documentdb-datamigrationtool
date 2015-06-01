using Microsoft.DataTransfer.DocumentDb.Sink.Substitutions.Range;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.DataTransfer.DocumentDb.UnitTests.Sink.Patterns
{
    [TestClass]
    public class RangeSubstitutionsResolverTests
    {
        [TestMethod]
        public void Resolve_NoSubstitutions_ReturnsOriginalString()
        {
            CollectionAssert.AreEqual(
                new[] { "test" },
                new RangeSubstitutionResolver().Resolve("test").ToArray(), 
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_NoSubstitutionsWithEscapedCharacter_HonorsEscaping()
        {
            CollectionAssert.AreEqual(
                new[] { "test[0-5]" },
                new RangeSubstitutionResolver().Resolve(@"test\[0-5\]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_ZeroBasedSubstitutionAtTheEnd_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "test0", "test1", "test2", "test3", "test4", "test5" },
                new RangeSubstitutionResolver().Resolve("test[0-5]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_NonZeroBasedSubstitutionAtTheEnd_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "test5", "test6", "test7", "test8", "test9", "test10" },
                new RangeSubstitutionResolver().Resolve("test[5-10]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_TwoSubstitutions_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "te5st0", "te5st1", "te5st2", "te6st0", "te6st1", "te6st2" },
                new RangeSubstitutionResolver().Resolve("te[5-6]st[0-2]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_ZeroBasedSubstitutionWithEscaping_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "test0[5-10]", "test1[5-10]", "test2[5-10]", "test3[5-10]", "test4[5-10]", "test5[5-10]" },
                new RangeSubstitutionResolver().Resolve(@"test[0-5]\[5-10\]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_ZeroBasedSubstitutionOnly_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "0", "1", "2", "3", "4", "5" },
                new RangeSubstitutionResolver().Resolve(@"[0-5]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_EscapedSubstitutionOnly_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { "[0-5]" },
                new RangeSubstitutionResolver().Resolve(@"\[0-5\]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_EscapeCharacterInfrontOfSubstitution_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { @"test\0", @"test\1", @"test\2", @"test\3", @"test\4", @"test\5" },
                new RangeSubstitutionResolver().Resolve(@"test\\[0-5]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }

        [TestMethod]
        public void Resolve_SubstitutionWithEndValueOnly_ReturnsAllPossibleCombinations()
        {
            CollectionAssert.AreEqual(
                new[] { @"test0", @"test1", @"test2", @"test3", @"test4", @"test5" },
                new RangeSubstitutionResolver().Resolve(@"test[5]").ToArray(),
                TestResources.InvalidSubstitutionsApplied);
        }
    }
}
