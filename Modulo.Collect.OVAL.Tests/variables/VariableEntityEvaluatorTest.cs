﻿/*
 * Modulo Open Distributed SCAP Infrastructure Collector (modSIC)
 * 
 * Copyright (c) 2011-2015, Modulo Solutions for GRC.
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 *   
 * - Redistributions in binary form must reproduce the above copyright 
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 *   
 * - Neither the name of Modulo Security, LLC nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific  prior written permission.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 * */
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulo.Collect.OVAL.Definitions.variableEvaluator;
using Modulo.Collect.OVAL.Definitions;

namespace Modulo.Collect.OVAL.Tests.variables
{
    /// <summary>
    /// Summary description for VariableEntityEvaluatorTest
    /// </summary>
    [TestClass]
    public class VariableEntityEvaluatorTest
    {
        public VariableEntityEvaluatorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod,Owner("lcosta")]
        public void Should_be_possible_to_process_a_variable_given_EntityBaseType()
        {
            List<string> variableValues = new List<string>() { "Multiprocessor Free" };
            VariableValue variable = new VariableValue("oval:org.mitre.oval:obj:6000", "oval:com.hp:var:1", variableValues);
            IEnumerable<VariableValue> variables = new List<VariableValue>() { variable };
            VariablesEvaluated variablesEvaluated = new VariablesEvaluated(variables);

            VariableEntityEvaluator variableEntityEvaluator = new VariableEntityEvaluator(variablesEvaluated);
            EntityObjectStringType entity = new EntityObjectStringType();
            entity.var_ref = "oval:com.hp:var:1";

            IEnumerable<string> values = variableEntityEvaluator.EvaluateVariableForEntity(entity);
            Assert.IsTrue(values.Count() > 0 , "the quantity of entities is not expected");
            Assert.AreEqual(values.ElementAt(0), variableValues.ElementAt(0), "the value is not expected");        
        }

        [TestMethod, Owner("lcosta")]
        public void Should_be_possible_to_process_a_variable_given_EntityBaseType_with_multiples_return_values()
        {
            List<string> variableValues = new List<string>() { "CSDVersion", "CSDBuild" };
            VariableValue variable = new VariableValue("oval:org.mitre.oval:obj:6000", "oval:com.hp:var:1", variableValues);
            IEnumerable<VariableValue> variables = new List<VariableValue>() { variable };
            VariablesEvaluated variablesEvaluated = new VariablesEvaluated(variables);

            VariableEntityEvaluator variableEntityEvaluator = new VariableEntityEvaluator(variablesEvaluated);
            EntityObjectStringType entity = new EntityObjectStringType();
            entity.var_ref = "oval:com.hp:var:1";

            IEnumerable<string> values = variableEntityEvaluator.EvaluateVariableForEntity(entity);
            Assert.IsTrue(values.Count() == 2, "the quantity of entities is not expected");
            Assert.AreEqual(values.ElementAt(0), variableValues.ElementAt(0), "the value is not expected");
            Assert.AreEqual(values.ElementAt(1), variableValues.ElementAt(1), "the value is not expected");  
        }
    }
}
