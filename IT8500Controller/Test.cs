using System;
using System.Linq;
using System.Threading;

namespace IT8500Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            /*try
            {
                DeviceController controller = new DeviceController("COM7");
                controller.Open();
                controller.SetRemote();
                Console.WriteLine($"Version: " + controller.GetVersion());
                // 设置输入状态
                //controller.SetInputState(true);
                Console.WriteLine("Input State: " + controller.QueryInputState());

                // 设置电流值
                controller.SetCurrent(2.5);
                Console.WriteLine("Current Setting: " + controller.QueryCurrent());

                controller.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            PauseBeforeExit();*/

            using (var controller = new IT8500Controller("COM7"))
            {
                controller.SetControlMode(0, 1);

                controller.SetLoadInputState(0, 0);

                //controller.SetLoadMode(0, 0); // 设置为定电流模式 (CC)
                controller.SetLoadMode(0, 1); // 设置为定电流模式 (CV)
                //controller.SetLoadMode(0, 2); // 设置为定电流模式 (CW)
                //controller.SetLoadMode(0, 3); // 设置为定电流模式 (CR)
                Thread.Sleep(500);
                Console.WriteLine($"负载模式: {controller.GetLoadMode(0)}");
                //controller.SetLoadConstantCurrentValue(0, 2.5);
                //Console.WriteLine($"负载定电流值: {controller.GetLoadConstantCurrentValue(0)}");

                controller.SetLoadConstantVoltageValue(0, 5.0);
                Thread.Sleep(500);
                Console.WriteLine($"负载定电压值: {controller.GetLoadConstantVoltageValue(0)}");

                controller.SetLoadInputState(0, 1);
                Thread.Sleep(500);

                double voltage = controller.ReadInputVoltage(0);
                Console.WriteLine($"Input Voltage: {voltage} V");
                double current = controller.ReadInputCurrent(0);
                Console.WriteLine($"Input Current: {current} A");
                double power = controller.ReadInputPower(0);
                Console.WriteLine($"Input Power: {power} W");
            }
            PauseBeforeExit();

            /*if (args.Length == 0 || args.Contains("-h") || args.Contains("--help"))
            {
                ShowHelp();
                PauseBeforeExit();
                return;
            }

            try
            {
                string port = GetArgument(args, "-p", "--port") ?? "COM3";
                byte address = byte.Parse(GetArgument(args, "-a", "--address") ?? "0");
                string command = args[0].ToLower();

                using (var controller = new IT8500Controller(port))
                {
                    switch (command)
                    {
                        case "setmode":
                            byte mode = byte.Parse(GetArgument(args, "-m", "--mode") ?? "0");
                            controller.SetLoadMode(address, mode);
                            Console.WriteLine($"Set load mode to {mode}");
                            break;

                        case "setcurrent":
                            double current = double.Parse(GetArgument(args, "-c", "--current") ?? "0");
                            controller.SetCurrent(address, current);
                            Console.WriteLine($"Set current to {current} A");
                            break;

                        case "readvoltage":
                            double voltage = controller.ReadInputVoltage(address);
                            Console.WriteLine($"Input Voltage: {voltage} V");
                            break;

                        default:
                            Console.WriteLine("Unknown command. Use -h or --help for usage information.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                PauseBeforeExit();
            }*/
        }

        static void ShowHelp()
        {
            Console.WriteLine("Usage: IT8500Tool [command] [options]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  setmode      Set the load mode (e.g., CC, CV, CW, CR).");
            Console.WriteLine("  setcurrent   Set the constant current value.");
            Console.WriteLine("  readvoltage  Read the input voltage.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -p, --port     Serial port name (default: COM3).");
            Console.WriteLine("  -a, --address  Device address (default: 0).");
            Console.WriteLine("  -m, --mode     Load mode: 0 (CC), 1 (CV), 2 (CW), 3 (CR).");
            Console.WriteLine("  -c, --current  Current value in amps.");
            Console.WriteLine("  -h, --help     Show this help message.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  IT8500Tool setmode -p COM3 -a 0 -m 0");
            Console.WriteLine("  IT8500Tool setcurrent -p COM3 -a 0 -c 2.5");
            Console.WriteLine("  IT8500Tool readvoltage -p COM3 -a 0");
        }

        static string GetArgument(string[] args, string shortOption, string longOption)
        {
            int index = Array.FindIndex(args, arg => arg == shortOption || arg == longOption);
            return (index != -1 && index + 1 < args.Length) ? args[index + 1] : null;
        }

        static void PauseBeforeExit()
        {
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }

}
