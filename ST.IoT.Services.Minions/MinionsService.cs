﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using ST.IoT.Services.Interfaces;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions
{
    public class MinionsService : IIoTService
    {
        private IBusControl _bus;
        private IRabbitMqHost _host;
        private BusHandle _handle;

        private const string _address = "rabbitmq://localhost/minions_virtual_host";

        public void Start()
        {
            Console.WriteLine("Creating bus");

            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _host = x.Host(new Uri(_address), h =>
                {
                    h.Username("minion_boss");
                    h.Password("minion_boss");
                });

                x.ReceiveEndpoint(_host, "minion_requests", e => { e.Consumer<MinionsRequestMessageConsumer>(); });
            });

            Console.WriteLine("Starting bus");
            _handle = _bus.Start();
            Console.WriteLine("Started bus");
        }

        public void Stop()
        {
            if (_handle != null)
            {
                _handle.Stop();
            }
        }
    }
}
