using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public class MonitorPoint : BaseDomain
{
    public int DeviceId { get; set; }
  public short DriverId {get; set;}
      public string Name { get; set; } = string.Empty;
        public short ModuleId { get; set; }
        public string ModuleDescription { get; set; } = string.Empty;
        public short InputNo { get; set; }
        public short InputMode { get; set; }
        public string InputModeDetail { get; set; } = string.Empty;
        public short Debounce { get; set; }
        public short HoldTime { get; set; }
        public short LogFunction { get; set; } = 1;
        public string LogFunctionDetail { get; set; } = string.Empty;
        public short MonitorPointMode { get; set; } = -1;
        public string MonitorPointModeDetail { get; set; } = string.Empty;
        public short DelayEntry { get; set; } = -1;
        public short DelayExit { get; set; } = -1;
        public bool IsMask { get; set; }

        public MonitorPoint() { }

        public MonitorPoint(int deviceId, short driverId, string name, short moduleId, string moduleDescription, short inputNo, short inputMode, string inputModeDetail, short debounce, short holdTime, short logFunction, string logFunctionDetail, short monitorPointMode, string monitorPointModeDetail, short delayEntry, short delayExit, bool isMask)
        {
                DeviceId = deviceId;
                DriverId = driverId;
                Name = ValidateRequiredString(name, nameof(name));
                ModuleId = moduleId;
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
                var trimmed = value.Trim();
                var sanitized = trimmed.Replace("-", string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Replace(":", string.Empty).Replace("/", string.Empty);
                if (!RegexHelper.IsValidName(trimmed) && !RegexHelper.IsValidOnlyCharAndDigit(sanitized))
                {
                        throw new ArgumentException($"{field} invalid.", field);
                }

                return value;
        }
}
