using NUnit.Framework;

namespace Agile.Shared.Tests
{
    /// <summary>
    /// Tests for common methods
    /// </summary>
    [TestFixture]
    public class CommonExtensionsTests
    {
        [Test]
        public void RegexNumberOnlyTests()
        {
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("0"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("12"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("9999999"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("123456789"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("121131123123"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("1"));
            Assert.IsTrue(RegexPatterns.AllNumbers.IsMatch("1"));

            Assert.IsFalse(RegexPatterns.AllNumbers.IsMatch("12e"));
            Assert.IsFalse(RegexPatterns.AllNumbers.IsMatch("a"));
            Assert.IsFalse(RegexPatterns.AllNumbers.IsMatch("a12"));
            Assert.IsFalse(RegexPatterns.AllNumbers.IsMatch(""));
            Assert.IsFalse(RegexPatterns.AllNumbers.IsMatch("-"));
        }

        /// <summary>
        /// Verifies that the function still works when there are no instances of the string to remove
        ///     - Create a string that doesnt contain commas
        ///     - Set string to remove to be a comma
        ///     - Check RemoveFirstInstanceOf returns the original string
        /// </summary>
        [Test]
        public void RemoveFirstInstanceOfWithNoInstancesTests()
        {
            // Create a string that doesnt contain commas
            string someString = "Some string that doesn't contain commas<>@#$%";
            // Set string to remove to be a comma
            string toRemove = ",";
            // Check RemoveFirstInstanceOf returns the original string
            Assert.AreEqual(someString, someString.RemoveFirstInstanceOf(toRemove),
                            "Check RemoveFirstInstanceOf returns the original string");
        }
        /// <summary>
        /// Checks functionality of RemoveNonAlphaNumericCharacters.
        ///     - Check a string with all valid characters - uppercase
        ///     - Check a string with all valid characters - lowercase
        ///     - Check string with all invalid characters returns an empty string.
        ///     - check a string with both valid and invalid characters returns the correct SAFE string
        ///     - check spaces are removed.
        /// </summary>
        [Test]
        public void RemoveNonAlphanumericCharactersTests()
        {
            string tester = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // Check a string with all valid characters - uppercase
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", tester.RemoveNonAlphanumericCharacters(),
                            "Check a string with all valid characters");

            tester = "abcdefghijklmnopqrstuvwxyz0123456789";
            // Check a string with all valid characters - lowercase
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz0123456789", tester.RemoveNonAlphanumericCharacters(),
                            "Check a string with all valid characters");

            // Check string with all invalid characters returns an empty string.
            Assert.AreEqual("_", "!£$%^&*()_-+=[]{}#~;':@,.<>?|".RemoveNonAlphanumericCharacters(),
                            "Check string with all invalid characters returns an empty string.");

            // Check a string with both valid and invalid characters returns the correct SAFE string
            Assert.AreEqual("asdfghjkl0_987654321qwertyuiopm",
                            "a!s£d$f%g^h&j*k(l)0_9-8+7=6[5]4{3}2#1~q;w'e:r@t,y.u<i>o?p|m".RemoveNonAlphanumericCharacters(),
                            "check a string with both valid and invalid characters returns the correct SAFE string");

            // check spaces are removed.
            Assert.AreEqual("stringwithspaces", " string with spaces ".RemoveNonAlphanumericCharacters(),
                            "check spaces are removed.");
        }

        /// <summary>
        /// Checks functionality of RemoveSpacesFromString
        ///     - check string with no spaces
        ///     - check string with one space
        ///     - check string with lots of spaces
        ///     - check string with space at start
        ///     - check string with space at end
        ///     - check multiple spaces together
        /// </summary>
        [Test]
        public void RemoveSpacesFromStringTests()
        {
            // check string with no spaces
            Assert.AreEqual("nospaces", "nospaces".RemoveSpacesFromString(), "check string with no spaces");

            // check string with one space
            Assert.AreEqual("onespace", "one space".RemoveSpacesFromString(), "check string with one space");

            // check string with lots of spaces
            Assert.AreEqual("lotsofspaces", "l o t s o f s p a c e s".RemoveSpacesFromString(),
                            "check string with lots of spaces");

            // check string with space at start
            Assert.AreEqual("spaceatstart", " spaceatstart".RemoveSpacesFromString(),
                            "check string with space at start");

            // check string with space at end
            Assert.AreEqual("spaceatend", "spaceatend ".RemoveSpacesFromString(),
                            "check string with space at end");

            // check multiple spaces together
            Assert.AreEqual("multiplespaces", "mult      iple    spa c   es".RemoveSpacesFromString(),
                            "check multiple spaces together");
        }
        /// <summary>
        /// Tests the IsValidNumber function
        /// </summary>
        [Test]
        public void TestisValidNumber()
        {
            Assert.AreEqual(true, "123".IsValidNumber());
            Assert.AreEqual(true, "123.45".IsValidNumber());
            Assert.AreEqual(true, "12314987129487214682146871247825.12486128496875970123".IsValidNumber());
            Assert.AreEqual(false, "".IsValidNumber());
            Assert.AreEqual(false, "hi".IsValidNumber());
            Assert.AreEqual(false, "5x".IsValidNumber());
            Assert.AreEqual(false, "5 ".IsValidNumber());
            Assert.AreEqual(false, "5.".IsValidNumber());
            Assert.AreEqual(true, "5.7".IsValidNumber());
            Assert.AreEqual(true, "0.7".IsValidNumber());
            Assert.AreEqual(true, "0000.7".IsValidNumber());
            Assert.AreEqual(true, ".77".IsValidNumber());
            Assert.AreEqual(true, "-.77".IsValidNumber());
            Assert.AreEqual(false, "-44.".IsValidNumber());
            Assert.AreEqual(true, "-44.38".IsValidNumber());
            Assert.AreEqual(true, "-0000.7888".IsValidNumber());
        }



        /// <summary>
        /// Verifies we can get the string that is between two other strings
        ///     - Create a string
        ///     - Check GetStringBetween returns the expected result
        /// </summary>
        [Test]
        public void GetStringBetweenTests()
        {
            // Create a string
            string searchIn = @"abc
def!>@#GKG#$#K^ 56&%$&L$% 4 #
PS$)#%K     
sdf
-65E_^$%_&5";
            // Check GetStringBetween returns the expected result
            Assert.AreEqual("GKG", searchIn.GetStringBetween("!>@#", "#$#K")
                            , "Check GetStringBetween returns the expected result");

            Assert.AreEqual(" ", searchIn.GetStringBetween("K^", "56&")
                            , "Check GetStringBetween returns the expected result");
        }
    }
}
