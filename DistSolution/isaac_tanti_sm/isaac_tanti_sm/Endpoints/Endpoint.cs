﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace isaac_tanti_sm.Endpoints
{
    abstract class Endpoint
    {
        //protected string apiKey;
        protected string baseEndpoint;

        public Endpoint(/*string apiKey, */string baseEndpoint)
        {
            //this.apiKey = apiKey;
            this.baseEndpoint = baseEndpoint;
        }
    }
}