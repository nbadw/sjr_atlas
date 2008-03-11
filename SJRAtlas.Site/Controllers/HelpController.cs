using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;
using NHibernate.Expression;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{    
    [DefaultAction("Index")]
    public class HelpController : BaseController
    {
        public void Index()
        {
            if(Logger.IsDebugEnabled)
                Logger.Debug("Help/Index action called");

            PropertyBag["help_contents"] = new HelpContents()
                .AddSection("Website Overview")
                    .Add("Overview of Maps")
                    .Add("Interactive Maps")
                    .Add("ToolBar")
                    .Add("Tasks")
                    .Add("Map Contents")
                    .Add("Navigation")
                .AddSection("Searching")
                    .Add("Quick Search")
                    .Add("Search Tips")
                    .Add("Advanced Search");
            
            RenderView("index");
        }

        public class HelpContents
        {
            private readonly HelpNode root;
            private HelpNode currentNode;

            public HelpContents()
            {
                root = new HelpNode("Help Contents", null, null);
                currentNode = root;
            }

            public string Title
            {
                get { return root.Name; }
            }

            public IList<HelpNode> Contents
            {
                get { return root.Contents; }
            }

            public HelpContents GoToRoot()
            {
                currentNode = root;
                return this;
            }

            public HelpContents AddSection(string name)
            {
                return AddSection(name, CreateAnchor(name));
            }

            public HelpContents AddSection(string name, string anchor)
            {
                currentNode = new HelpNode(name, anchor, null);
                root.Add(currentNode);
                return this;
            }

            public HelpContents Add(string name)
            {
                return Add(name, CreateAnchor(name), CreateView(name));
            }

            public HelpContents Add(string name, string view)
            {
                return Add(name, CreateAnchor(name), view);
            }

            public HelpContents Add(string name, string anchor, string view)
            {
                currentNode.Add(new HelpNode(name, anchor, view));
                return this;
            }

            private string CreateAnchor(string name)
            {
                return name.ToLower().Replace(" ", "_");
            }

            private string CreateView(string name)
            {
                return String.Format("help/_{0}.vm", name.ToLower().Replace(" ", "_"));
            }
        }

        public class HelpNode
        {
            public HelpNode(string name, string anchor, string view)
            {
                this.name = name;
                this.anchor = anchor;
                this.view = view;
                this.contents = new List<HelpNode>();
            }

            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private string anchor;

            public string Anchor
            {
                get { return anchor; }
                set { anchor = value; }
            }

            private string view;

            public string View
            {
                get { return view; }
                set { view = value; }
            }
            
            private IList<HelpNode> contents;

            public IList<HelpNode> Contents
            {
                get { return contents; }
                set { contents = value; }
            }

            public void Add(HelpNode node)
            {
                Contents.Add(node);
            }
        }
    }
    
}
