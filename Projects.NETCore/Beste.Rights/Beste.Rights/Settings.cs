using System;
using System.Collections.Generic;
using System.Text;

namespace Beste.Rights
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Settings : Beste.Xml.Xml
    {
        public TokenInterval TokenInitialTimeout { get; set; }
        public TokenInterval TokenRefreshOnUsage { get; set; }
        
    }
    public partial class TokenInterval
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public string TokenRefreshOnUsage { get; set; }

    }
}
