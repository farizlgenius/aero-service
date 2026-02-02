using System;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class BaseAeroCommand
{
      //////
      // Method: SendCommand to Driver
      // PreCondition: SCP online
      // PostCondition: Pass/Fail response is sent from the driver
      //////
      protected bool Send(short command, IConfigCommand cfg)
      {
            SCPConfig scp = new SCPConfig();
            bool success = scp.scpCfgCmndEx(command, cfg);
            return success;
      }

      //////
      // Method: Turn on debug to file
      //////
      public void TurnOnDebug()
      {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
            if (flag)
            {
                  Console.WriteLine("Debug to file on");
            }
            else
            {
                  Console.WriteLine("Debug to file off");
            }

      }

      //////
      // Method: Turn off debug to file
      //////
      public void TurnOffDebug()
      {
            bool flag = SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugOff);
            if (flag)
            {
                  Console.WriteLine("Debug to file off");
            }
            else
            {
                  Console.WriteLine("Debug to file on");
            }
      }

      //////
      // Method: SendCommand to Driver
      // PreCondition: SCP online
      // PostCondition: Pass/Fail response is sent from the driver
      //////
      public async Task<bool> SendASCIICommand(string command)
      {
            // send the command
            return SCPDLL.scpConfigCommand(command);
      }
}
