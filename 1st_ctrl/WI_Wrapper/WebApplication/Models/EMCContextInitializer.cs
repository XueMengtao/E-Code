using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication.Models
{
    //这个类仅做测试数据库功能用，正式使用时删除
    public class EMCContextInitializer:DropCreateDatabaseIfModelChanges<EMCContext>
    {
        protected override void Seed(EMCContext context)
        {
            var antennae = new List<Antenna>
            {
                new Antenna(){Name="TestAntenna1"},
                new Antenna(){Name="TestAntenna2"}
            };
            antennae.ForEach(o => context.Antennae.Add(o));

            var transmitters = new List<Transmitter>
            {
                new Transmitter(){Name="TestTransmitter1"},
                new Transmitter(){Name="TestTransmitter2"}
            };
            transmitters.ForEach(o => context.Transmitters.Add(o));

            var receivers = new List<Receiver>
            {
                new Receiver(){Name="TestReceiver1"},
                new Receiver(){Name="TestReceiver2"}
            };
            receivers.ForEach(o => context.Receivers.Add(o));

            var waveforms = new List<Waveform>
            {
                new Waveform(){Name="TestWaveform1"},
                new Waveform(){Name="TestWaveform2"}
            };
            context.SaveChanges();
        }
    }
}