using System;
namespace HarperDBStudioMobile.Models
{
    /// <summary>
    /// Get Alarms
    ///
    /// POST https://prod.harperdbcloudservices.com/getAlarms
    /// </summary>
    public partial class GetAlarmsModel
{
        public object[] Body { get; set; }
    }
}
