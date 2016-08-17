﻿#region License
/*
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
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Xml.XPath;

namespace Modulo.Collect.OVAL.Definitions
{
    public class Schematron
    {
        public bool Validate(string id, string definitions, out string errors)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            var xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Modulo.Collect.OVAL.Definitions.schematron-compiled.xsl");
            XmlReader xmlReader = XmlReader.Create(xmlStream);
            xslt.Load(xmlReader);

            byte[] byteArray = Encoding.UTF8.GetBytes(definitions);
            MemoryStream definitionsStream = new MemoryStream(byteArray);
            var definitionsXmlReader = XmlReader.Create(definitionsStream);

            var xsltArgs = new XsltArgumentList();
            var stream = new MemoryStream();
            xslt.Transform(definitionsXmlReader, xsltArgs, stream);
            stream.Flush();
            stream.Position = 0;

            XPathDocument document = new XPathDocument(stream);
            XPathNavigator navigator = document.CreateNavigator();
            XPathExpression query = navigator.Compile("//svrl:successful-report/svrl:text");
            XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            manager.AddNamespace("svrl", "http://purl.oclc.org/dsdl/svrl");
            query.SetContext(manager);
            XPathNodeIterator nodes = navigator.Select(query);

            if (nodes.Count == 0)
            {
                errors = null;
                return true;
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Definition \"{0}\" didn't pass schematron validation:", id);
                sb.AppendLine();
                while (nodes.MoveNext())
                {
                    sb.AppendLine(nodes.Current.Value.Trim());
                }

                errors = sb.ToString().Trim();
                return false;
            }
        }
    }
}