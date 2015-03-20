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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modulo.Collect.Probe.Common.Extensions;

namespace Modulo.Collect.Probe.Unix.SSHCollectors
{
    public class EnvVarInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0}=\"{1}\"", this.Name, this.Value);
        }
    }

    public class EnvironmentVariableCollector
    {
        public SshCommandLineRunner CommandRunner { get; set; }

        public virtual Dictionary<String, String> GetTargetEnvironmentVariables()
        {
            var retList = new Dictionary<string, string>();
            var commandResultLines = CommandRunner.ExecuteCommand("set").SplitStringByDefaultNewLine();
            var allInfo = commandResultLines.Select(cmdLine => ParseEnvVarInfo(cmdLine));
            return allInfo.Where(info => info != null).ToDictionary(x => x.Name, x => x.Value);


            //foreach (string line in lines)
            //{
            //    EnvVarInfo thisInfo = this.ParseEnvVarInfo(line);
            //    if (thisInfo != null)
            //        retList[thisInfo.Name] = thisInfo.Value;
            //}

            //return retList;
        }

        private EnvVarInfo ParseEnvVarInfo(string line)
        {
            EnvVarInfo retInfo = null;
            char[] elemseps = { '=' };
            string[] ffields = line.Split(elemseps, 2);
            if (ffields.GetUpperBound(0) == 1)
            {
                retInfo = new EnvVarInfo();
                retInfo.Name = ffields[0];
                retInfo.Value = ffields[1];
            }

            return retInfo;
        }
    }
}
