using System;

namespace Metadata.Indexer
{
    public class Rule
    {
        public Rule(string name, string xpath)
        {
            this.name = name;
            this.xpath = xpath;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string xpath;

        public string Xpath
        {
            get { return xpath; }
            set { xpath = value; }
        }

        public override string ToString()
        {
            return String.Format("Rule '{0}', XPath '{1}'",
                Name, Xpath);
        }
    }
}
