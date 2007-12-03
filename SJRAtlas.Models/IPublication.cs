using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models
{
    public interface IPublication
    {
        string Title
        {
            get;
            set;
        }

        string Abstract
        {
            get;
            set;
        }

        string Author
        {
            get;
            set;
        }

        string File
        {
            get;
            set;
        }
    }
}
