using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AwesomeChat.BechmarkingTest.BenchmarkingTest
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]

    public class AwesomeChatBenchmarking
    {
        [Benchmark]
        public void SendChatMessage_Benchmarking()
        {

        }

        [Benchmark]
        public void GetMessageByRoomName_Benchmarking()
        {

        }
    }
}
