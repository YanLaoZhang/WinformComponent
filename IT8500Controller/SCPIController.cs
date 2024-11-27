using System;
using System.IO.Ports;

public class DeviceController
{
    private SerialPort _serialPort;

    public DeviceController(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = 5000,
            WriteTimeout = 2000
        };
    }

    public void Open()
    {
        if (!_serialPort.IsOpen)
        {
            _serialPort.Open();
        }
    }

    public void Close()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public string SendCommand(string command)
    {
        if (!_serialPort.IsOpen)
        {
            throw new InvalidOperationException("Serial port is not open.");
        }

        _serialPort.WriteLine(command);
        return _serialPort.ReadLine();
    }

    public void SetRemote()
    {
        SendCommand("SYSTem:REMote");
    }

    public string GetVersion()
    {
        return SendCommand("SYSTem:VERSion?");
    }

    public void SetInputState(bool state)
    {
        string command = state ? "INPut ON" : "INPut OFF";
        SendCommand(command);
    }

    public string QueryInputState()
    {
        return SendCommand("INPut?");
    }

    public void SetCurrent(double current)
    {
        string command = $"CURRent:LEVel {current}";
        SendCommand(command);
    }

    public string QueryCurrent()
    {
        return SendCommand("CURRent:LEVel?");
    }

    // Add more methods as needed for other SCPI commands
}
