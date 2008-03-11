using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Models.Query
{
    public class QueryParser
    {
        public static string BuildContainsTerms(string query)
        {
            StringBuilder contains = new StringBuilder();
            string[] terms = query.Split(' ');
            bool quoted_term = false;
            for (int i = 0; i < terms.Length; i++)
            {
                string term = terms[i];
                contains.Append(term);

                if (term.StartsWith("\""))
                    quoted_term = true;
                else if (term.EndsWith("\""))
                    quoted_term = false;

                if (!quoted_term && i != terms.Length - 1)
                    contains.Append(" AND ");
                else
                    contains.Append(" ");
            }
            return contains.ToString();
        }
    }
}
