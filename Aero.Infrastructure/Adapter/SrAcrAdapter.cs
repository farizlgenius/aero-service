using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class SrAcrAdapter : ISrAcr
{
    private readonly SCPReplyMessage.SCPReplySrAcr _src;

    public SrAcrAdapter(SCPReplyMessage.SCPReplySrAcr sts_acr)
    {
        _src = sts_acr;
    }

    public short number => _src.number;
    public short mode => _src.mode;

    public short rdr_status => _src.rdr_status;
    public short strk_status => _src.strk_status;
    public short door_status => _src.door_status;
    public short ap_status => _src.ap_status;

    public short rex_status0 => _src.rex_status0;
    public short rex_status1 => _src.rex_status1;

    public short led_mode => _src.led_mode;
    public short actl_flags => _src.actl_flags;
    public short altrdr_status => _src.altrdr_status;
    public short actl_flags_extd => _src.actl_flags_extd;

    public short nExtFeatureType => _src.nExtFeatureType;
    public short nHardwareType => _src.nHardwareType;

    public byte[] nExtFeatureStatus => _src.nExtFeatureStatus;
    public int nAuthModFlags => _src.nAuthModFlags;
}
