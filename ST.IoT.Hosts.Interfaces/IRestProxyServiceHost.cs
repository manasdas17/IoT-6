﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.API.REST.Proxy.Interfaces;

namespace ST.IoT.Hosts.Interfaces
{
    public interface IRestProxyServiceHost 
    {
        IRestApiProxyHost RestApiProxyHost { get; set; }
    }
}