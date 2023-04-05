using AdaptiveTestingSystem.ServerLibraly.CScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdaptiveTestingSystem.ServerLibraly.Command
{


    // var stat = await DBSearchMethods.GetGeneralStatistic();

    public class Command_StatisticGeneral : Commands
    {

        ThreadSendPacket sendPacket;

        public override void Execut(string json, ClientObject client, ServerObject activeServer)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Data_StatisticPacket>(json);

                if (obj != null)
                {
                    switch (obj.IsCode)
                    {
                        case Code.ThreadStart: StartSendData(client, activeServer); break;
                        case Code.ThreadNext: sendPacket.CancelWaitSendPacket(); break;
                        case Code.ThreadEnd: StopSend(); break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"Command_StatisticGeneral вызвал ошибку: {ex.Message}");
            }
        }

        private void StopSend()
        {
            if (sendPacket != null) { sendPacket.Dispose(); sendPacket = null; }
        }

        private async void StartSendData(ClientObject client, ServerObject activeServer)
        {
            sendPacket = new ThreadSendPacket("Command_StatisticGeneral", client, activeServer);
            var listEmployee = await DBSearchMethods.GetGeneralStatistic();
            sendPacket.StartSend(listEmployee);
        }
    }
 }