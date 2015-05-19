using System.Collections.Generic;

namespace DocSearcher.Model
{
    public class DocTypeCollection
    {
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private List<ExtensionSpace> _extensions;

        public List<ExtensionSpace> Extensions
        {
            get { return _extensions; }
            set { _extensions = value; }
        }

        public DocTypeCollection(string fileType)
        {
            Type = fileType;
            Extensions = new List<ExtensionSpace>();
        }
    }
}