This is an example of how to send raw Ethernet frames using .NET Core.

To use raw sockets, the executable need to be run as root or the NET_RAW capability must be set on the executable

```
$ sudo setcap 'cap_net_raw=ep' netcore-linux-raw-ethernet
$ ./netcore-linux-raw-ethernet
```