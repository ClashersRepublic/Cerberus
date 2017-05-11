using BL.Servers.CoC.Logic;

namespace BL.Servers.CoC.Packets
{
    internal class Debug
    {

        internal Device Device
        {
            get;
            set;
        }

        internal string[] Parameters
        {
            get;
            set;
        }

        internal Debug(Device Device, params string[] Parameters)
        {
            this.Device = Device;
            this.Parameters = Parameters;
        }

        internal virtual void Decode()
        {
            // Decode.
        }

        internal virtual void Process()
        {
            // Process.
        }
    }
}