using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using WSSAT.DataTypes;

namespace WSSAT.BusinessLayer
{
    public class Parser
    {
        ServiceDescription serviceDescription = null;
        BindingCollection bindColl;
        PortTypeCollection portTypColl;
        MessageCollection msgColl;
        Types types;
        XmlSchemas schemas;
        public string TargetNameSpace { set; get; }
        public XDocument rawWSDL;

        public Parser(WSDescriber wsDesc, ref bool untrustedSSLSecureChannel, ref List<Param> respHeader, string customRequestHeader)
        {
            HttpWebRequest wr = GetHttpWebReq(wsDesc, customRequestHeader);

            HttpWebResponse wres = null;
            try
            {
                wres = (HttpWebResponse)wr.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.TrustFailure)
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    wr = GetHttpWebReq(wsDesc, customRequestHeader);
                    wres = (HttpWebResponse)wr.GetResponse();

                    untrustedSSLSecureChannel = true;
                }
            }

            if (wres != null)
            {
                for (int i = 0; i < wres.Headers.Count; ++i)
                {
                    respHeader.Add(new Param() { Name = wres.Headers.Keys[i].ToLowerInvariant(), Value = wres.Headers[i].ToLowerInvariant() });
                }

                try
                {
                    StreamReader streamReader = new StreamReader(wres.GetResponseStream());

                    rawWSDL = XDocument.Parse(streamReader.ReadToEnd());

                    TextReader myTextReader = new StringReader(rawWSDL.ToString());
                    serviceDescription = ServiceDescription.Read(myTextReader);

                    TargetNameSpace = serviceDescription.TargetNamespace;
                    bindColl = serviceDescription.Bindings;
                    portTypColl = serviceDescription.PortTypes;
                    msgColl = serviceDescription.Messages;
                    types = serviceDescription.Types;
                    schemas = types.Schemas;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private HttpWebRequest GetHttpWebReq(WSDescriber wsDesc, string customRequestHeader)
        {
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(wsDesc.WSDLAddress);

            wr.UserAgent = MainForm.UserAgentHeader;
            //wr.Connection = "keep-alive";

            wr.Proxy = WebRequest.DefaultWebProxy;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            if (!string.IsNullOrEmpty(customRequestHeader))
            {
                wr.Headers.Add(customRequestHeader);
            }

            if (wsDesc.BasicAuthentication != null && !string.IsNullOrEmpty(wsDesc.BasicAuthentication.Username))
            {
                wr.Credentials = new NetworkCredential(wsDesc.BasicAuthentication.Username, wsDesc.BasicAuthentication.Password);
            }

            return wr;
        }

        public List<WSOperation> GetOperations()
        {
            List<WSOperation> lst = new List<WSOperation>();
            if (serviceDescription != null)
            {
                foreach (Service ser in serviceDescription.Services)
                {
                    String webServiceName = ser.Name.ToString();
                    foreach (Port port in ser.Ports)
                    {
                        string portName = port.Name;
                        string binding = port.Binding.Name;
                        Binding bind = bindColl[binding];

                        if (bind != null)
                        {
                            PortType portTyp = portTypColl[bind.Type.Name];

                            foreach (Operation op in portTyp.Operations)
                            {
                                WSOperation operObj = new WSOperation();
                                operObj.ClassName = webServiceName;
                                operObj.MethodName = op.Name;

                                if (lst.Where(it => it.ClassName.Equals(operObj.ClassName) && it.MethodName.Equals(operObj.MethodName)).Count() == 0)
                                {
                                    OperationMessageCollection opMsgColl = op.Messages;
                                    OperationInput opInput = opMsgColl.Input;
                                    OperationOutput opOutput = opMsgColl.Output;
                                    string inputMsg = opInput.Message.Name;
                                    string outputMsg = opOutput.Message.Name;

                                    Message msgInput = msgColl[inputMsg];
                                    List<WSParameter> InputParam = GetParameters(msgInput);

                                    Message msgOutput = msgColl[outputMsg];
                                    List<WSParameter> OutputParams = GetParameters(msgOutput);

                                    operObj.Parameters = InputParam;
                                    if (OutputParams != null && OutputParams.Count > 0)
                                    {
                                        operObj.ReturnType = OutputParams[0].TypeName;
                                    }

                                    lst.Add(operObj);
                                }
                            }
                        }
                    }
                }
            }
            return lst;
        }

        public List<WSParameter> GetParameters(Message msg)
        {
            List<WSParameter> parameters = new List<WSParameter>();
            foreach (MessagePart msgpart in msg.Parts)
            {
                if (!msgpart.Element.IsEmpty)
                {
                    XmlQualifiedName typName = msgpart.Element;

                    XmlSchemaElement lookup = (XmlSchemaElement)schemas.Find(typName, typeof(XmlSchemaElement));

                    if (lookup != null)
                    {
                        XmlSchemaComplexType tt = (XmlSchemaComplexType)lookup.SchemaType;

                        XmlSchemaSequence sequence = (XmlSchemaSequence)tt.Particle;
                        //int i = 0;
                        if (sequence != null)
                        {
                            foreach (XmlSchemaElement childElement in sequence.Items)
                            {
                                WSParameter param = new WSParameter();
                                param.Name = childElement.Name;
                                param.TypeName = childElement.SchemaTypeName.Name;
                                param.MinOccurs = childElement.MinOccurs;
                                param.MaxOccurs = childElement.MaxOccurs.ToString();

                                parameters.Add(param);
                                //ParameterAndType.Add(childElement.Name, childElement.SchemaTypeName.Name);
                                //Console.WriteLine("Element: {0} ,{1}", childElement.Name,childElement.SchemaTypeName.Name);
                            }
                        }
                    }
                }
                else
                {
                    WSParameter param = new WSParameter();
                    param.Name = msgpart.Name;
                    param.TypeName = msgpart.Type.Name;
                    param.MinOccurs = 0;
                    param.MaxOccurs = "0";

                    parameters.Add(param);
                }
            }

            return parameters;
        }


        public string GetUnboundedOccurrence()
        {
            string result = string.Empty;

            // get xml element nodes by maxOccurs attribute value
            var elements = rawWSDL.Root
                        .DescendantsAndSelf()
                        .Elements()
                        //.Where(d => d.Name.LocalName.ToLower().Equals("maxoccurs") && d.Value.ToLower().Equals("unbounded"));
                        .Where(d => (string)d.Attribute("maxOccurs") == "unbounded");

            if (elements != null && elements.Count() > 0)
            {
                foreach (var elm in elements.Cast<XElement>())
                {
                    result += elm.ToString() + "\r\n";
                }
            }

            return result;
        }
    }
}
