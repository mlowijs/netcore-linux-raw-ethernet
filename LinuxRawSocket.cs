using System.Net.Sockets;

public sealed class LinuxRawSocket : Socket
{
    private const ProtocolType ProtocolTypeAll = (ProtocolType)3;
    
    public LinuxRawSocket(ProtocolType protocolType = ProtocolTypeAll) : base(AddressFamily.Packet, SocketType.Raw, protocolType)
    {
    }
}