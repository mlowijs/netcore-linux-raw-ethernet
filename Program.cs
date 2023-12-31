﻿using System.Net.NetworkInformation;

const string networkInterfaceName = "enp61s0"; // eth0

var networkInterface = NetworkInterface.GetAllNetworkInterfaces().First(ni => ni.Name == networkInterfaceName);
var destinationMac = new byte[] { 0x00, 0x0d, 0xb9, 0x49, 0x15, 0x6d };

var endpoint = new LinuxEthernetEndPoint(networkInterface, destinationMac);

// https://en.wikipedia.org/wiki/Ethernet_frame#Structure
using var frame = new MemoryStream();
frame.Write(destinationMac); // Destination MAC
frame.Write(networkInterface.GetPhysicalAddress().GetAddressBytes()); // Source MAC
frame.Write(new byte[] { 0x08, 0x00 }); // EtherType (IP)
frame.Write(new byte[] { 0xde, 0xad, 0xbe, 0xef }); // Payload

using var sock = new LinuxRawSocket();
sock.SendTo(frame.ToArray(), endpoint);