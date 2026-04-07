using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public class MonitorPoint : BaseDomain
{
        public int Id {get;private set;}
    public int ScpId { get;private set; }
  public short MpId {get;private set;}
      public string Name { get;private set; } = string.Empty;
        public int ModuleId { get;private set; }
        public short ModuleDriverId {get;private set;}
        public string ModuleDescription { get;private set; } = string.Empty;
        public short InputNo { get;private set; }
        public short InputMode { get;private set; }
        public string InputModeDetail { get;private set; } = string.Empty;
        public short Debounce { get;private set; }
        public short HoldTime { get;private set; }
        public short LogFunction { get;private set; } = 1;
        public string LogFunctionDetail { get;private set; } = string.Empty;
        public short MonitorPointMode { get;private set; } = -1;
        public string MonitorPointModeDetail { get;private set; } = string.Empty;
        public short DelayEntry { get;private set; } = -1;
        public short DelayExit { get;private set; } = -1;
        public bool IsMask { get;private set; }

        public MonitorPoint() { }

        public MonitorPoint(int Id,int scpid, short mpid, string name, int moduleId,short moduledriver, string moduleDescription, short inputNo, short inputMode, string inputModeDetail, short debounce, short holdTime, short logFunction, string logFunctionDetail, short monitorPointMode, string monitorPointModeDetail, short delayEntry, short delayExit, bool isMask)
        {
                this.Id = Id;
                ScpId = scpid;
                MpId = mpid;
                Name = ValidateRequiredString(name, nameof(name));
                ModuleId = moduleId;
                this.ModuleDriverId = moduledriver;
                ModuleDescription = ValidateRequiredString(moduleDescription, nameof(moduleDescription));
                InputNo = inputNo;
                InputMode = inputMode;
                InputModeDetail = ValidateRequiredString(inputModeDetail, nameof(inputModeDetail));
                Debounce = debounce;
                HoldTime = holdTime;
                LogFunction = logFunction;
                LogFunctionDetail = ValidateRequiredString(logFunctionDetail, nameof(logFunctionDetail));
                MonitorPointMode = monitorPointMode;
                MonitorPointModeDetail = ValidateRequiredString(monitorPointModeDetail, nameof(monitorPointModeDetail));
                DelayEntry = delayEntry;
                DelayExit = delayExit;
                IsMask = isMask;
        }

        private static string ValidateRequiredString(string value, string field)
        {
                ArgumentException.ThrowIfNullOrWhiteSpace(value, field);
                if (!RegexHelper.IsValidName(value))
                {
                        throw new ArgumentException($"{field} invalid.", field);
                }

                return value;
        }


}
