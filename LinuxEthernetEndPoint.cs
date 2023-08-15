using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

/// <summary>
/// Endpoint for sending raw Ethernet frames on Linux hosts.
/// </summary>
public sealed class LinuxEthernetEndPoint : EndPoint
{
    private const int SocketAddressLinkLayerLength = 20;
    private const byte EthernetAddressLength = 6;
    private const string LinuxNetworkInterfaceTypeName = "LinuxNetworkInterface";
    private const string LinuxNetworkInterfaceIndexPropertyName = "Index";
    
    private readonly SocketAddress _socketAddress;

    public LinuxEthernetEndPoint(NetworkInterface networkInterface, byte[] destinationMac)
    {
        var type = networkInterface.GetType();
        
        if (type.Name != LinuxNetworkInterfaceTypeName)
            throw new ArgumentException("Must be a Linux network interface.", nameof(networkInterface));
        
        var interfaceIndex = (int)type.GetProperty(LinuxNetworkInterfaceIndexPropertyName)!.GetValue(networkInterface)!;
        var interfaceIndexBytes = BitConverter.GetBytes(interfaceIndex);
        
        _socketAddress = new(AddressFamily, SocketAddressLinkLayerLength)
        {
            [4] = interfaceIndexBytes[0],
            [5] = interfaceIndexBytes[1],
            [6] = interfaceIndexBytes[2],
            [7] = interfaceIndexBytes[3],
            [11] = EthernetAddressLength,
            [12] = destinationMac[0],
            [13] = destinationMac[1],
            [14] = destinationMac[2],
            [15] = destinationMac[3],
            [16] = destinationMac[4],
            [17] = destinationMac[5]
        };
    }
    
    public override AddressFamily AddressFamily => AddressFamily.Unspecified;
    
    public override SocketAddress Serialize() => _socketAddress;
}