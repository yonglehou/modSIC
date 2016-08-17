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

using System.Collections.Generic;
using System.Linq;
using Modulo.Collect.OVAL.Definitions;
using Modulo.Collect.OVAL.Definitions.variableEvaluator;
using Modulo.Collect.OVAL.Definitions.Independent;

namespace Modulo.Collect.Probe.Windows.FileContent
{
    public class FileContentEntityVariableEvaluator
    {
        private VariablesEvaluated variablesEvaluated;

        public FileContentEntityVariableEvaluator(VariablesEvaluated variablesEvaluated)
        {
            this.variablesEvaluated = variablesEvaluated;
        }

        public IEnumerable<ObjectType> ProcessVariables(ObjectType objcetType)
        {
            textfilecontent_object fileContentObject = (textfilecontent_object)objcetType;
            List<ObjectType> fileContentObjects = new List<ObjectType>();

            IEnumerable<string> variablesFromfileName = this.ProcessVariableForFileName(fileContentObject);
            IEnumerable<string> variablesFromline = this.ProcessVariableForLine(fileContentObject);
            IEnumerable<string> variablesFrompath = this.ProcessVariableForPath(fileContentObject);

            if (this.IsVariablesWasProcessed(variablesFromfileName, variablesFromline, variablesFrompath))
            {
                FileContentObjectTypeFactory factory = new FileContentObjectTypeFactory();
                fileContentObjects.AddRange(factory.CreateObjectTypeByCombinationOfEntities(fileContentObject, variablesFromfileName, variablesFromline, variablesFrompath));
            }

            return fileContentObjects;

        }

        private IEnumerable<string> ProcessVariableForFileName(textfilecontent_object fileContentObject)
        {
            EntityObjectStringType fileName = (EntityObjectStringType)fileContentObject.GetItemValue(textfilecontent_ItemsChoices.filename);
            return this.processVariablesForEntity(fileName);
        }

        private IEnumerable<string> ProcessVariableForLine(textfilecontent_object fileContentObject)
        {
            EntityObjectStringType line = (EntityObjectStringType)fileContentObject.GetItemValue(textfilecontent_ItemsChoices.line);
            return this.processVariablesForEntity(line);
        }

        private IEnumerable<string> ProcessVariableForPath(textfilecontent_object fileContentObject)
        {
            EntityObjectStringType path = (EntityObjectStringType)fileContentObject.GetItemValue(textfilecontent_ItemsChoices.path);
            return this.processVariablesForEntity(path);
        }

        private IEnumerable<string> processVariablesForEntity(EntityObjectStringType entity)
        {
            List<string> variables = new List<string>();
            if (entity == null)
                return variables;

            VariableEntityEvaluator variableEvaluator = new VariableEntityEvaluator(this.variablesEvaluated);
            variables.AddRange(variableEvaluator.EvaluateVariableForEntity(entity));
            if (variables.Count() == 0)
                variables.Add(entity.Value);
            return variables;

        }

        private bool IsVariablesWasProcessed(IEnumerable<string> fileNames, IEnumerable<string> lines, IEnumerable<string> paths)
        {
            return ((paths.Count()) > 0 || (fileNames.Count() > 0) || (lines.Count() > 0));
        }
    }
}

