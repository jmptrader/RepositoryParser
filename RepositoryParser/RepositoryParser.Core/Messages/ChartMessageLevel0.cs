﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace RepositoryParser.Core.Messages
{
    public class ChartMessageLevel0 : MessageBase
    {
        public List<string> AuthorsList { get; private set; }
        public string FilteringQuery { get; private set; }

        public ChartMessageLevel0(string generatedQuery)
        {
            FilteringQuery = generatedQuery;
        }
        public ChartMessageLevel0(List<string> authorsList, string generatedQuery)
        {
            FilteringQuery = generatedQuery;
            AuthorsList = authorsList;
        }
    }
}
