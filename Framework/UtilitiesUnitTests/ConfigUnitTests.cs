﻿//--------------------------------------------------
// <copyright file="ConfigUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test configuration tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Configuration unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigUnitTests
    {
        /// <summary>
        /// Gets a value from a string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetValueWithString()
        {
            #region GetValueString
            string value = Config.GetValue("WaitTime");
            #endregion
            Assert.AreEqual(value, "100");
        }

        /// <summary>
        /// Gets a value with a string or default
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GetValueWithStringAndDefault()
        {
            #region GetValueWithDefault
            string value = Config.GetValue("DoesNotExist", "Default");
            #endregion
            Assert.AreEqual(value, "Default");
        }

        /// <summary>
        /// Checks if a key exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void DoesKeyExist()
        {
            #region DoesKeyExist
            bool value = Config.DoesKeyExist("DoesNotExist");
            #endregion
            Assert.AreEqual(value, false);
        }

        /// <summary>
        ///  Verify simple override of an existing configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SimpleOverrideConfig()
        {
            // Simple override data
            string key = "SimpleOverride";
            string baseValue = Config.GetValue(key);
            string overrideValue = baseValue + "_Override";

            // Override the configuration
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Make sure it worked
            Assert.AreEqual(overrideValue, Config.GetValue(key));
        }

        /// <summary>
        ///  Verify simple override of a new configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void OverrideNewConfig()
        {
            string key = "AddNewKey";
            string value = "TestValue";
            
            // Make sure the new key is not present
            Assert.AreEqual(string.Empty, Config.GetValue(key));

            // Set the override
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, value);
            Config.AddTestSettingValues(overrides);

            // Make sure the override worked
            Assert.AreEqual(value, Config.GetValue(key));
        }

        /// <summary>
        ///  Verify complex configuration overrides
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ComplexOverrideConfig()
        {
            // Define keys
            string key = "Override";
            string key2 = "Override2";

            // Get base key values
            string baseValue = Config.GetValue(key);
            string baseValue2 = Config.GetValue(key2);

            // Set override value
            string overrideValue = baseValue + "_Override";

            // Override first key value
            Dictionary<string, string> overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides);

            // Try to override something that has already been overriden
            overrides = new Dictionary<string, string>();
            overrides.Add(key, "ValueThatShouldNotOverride");
            Config.AddTestSettingValues(overrides);

            // The secondary override should fail as we already overrode it once
            Assert.AreEqual(overrideValue, Config.GetValue(key));

            // Try the override again, but this time tell the override to allow itself to be overrode
            overrideValue += "_SecondOverride";
            overrides = new Dictionary<string, string>();
            overrides.Add(key, overrideValue);
            Config.AddTestSettingValues(overrides, true);

            // Make sure the force override worked
            Assert.AreEqual(overrideValue, Config.GetValue(key));

            // Make sure the value we didn't override was not affected
            Assert.AreEqual(baseValue2, Config.GetValue(key2));
        }
    }
}
