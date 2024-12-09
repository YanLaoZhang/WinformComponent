using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMW500Lib
{
    /*
        *IDN?
        *RST;*CLS;*OPC?
        SYSTem:DISPlay:UPDate ON
        SYSTem:DISPlay:UPDate OFF
        TRACe:REMote:MODE:DISPlay:ENABle ANALysis
        TRACe:REMote:MODE:DISPlay:ENABle LIVE
        TRACe:REMote:MODE:DISPlay:ENABle OFF
        TRACe:REMote:MODE:DISPlay:CLEar
        SYSTem:PRESet:ALL
        SYSTem:RESet:ALL
    */
    public class CMW500Controler
    {
        public const string RST_CLS_OPC = "*RST;*CLS;*OPC?";
        public const string CMD_DISPLAY_ON = "SYSTem:DISPlay:UPDate ON";
        public const string CMD_DISPLAY_OFF = "SYSTem:DISPlay:UPDate ON";
        public const string CMD_LTE_Signal_ON = "SOURce:LTE:SIGN:CELL:STATe ON";
        public const string CMD_LTE_Signal_OFF = "SOURce:LTE:SIGN:CELL:STATe OFF";
        public const string CMD_LTE_Signal_STATE = "SOURce:LTE:SIGN:CELL:STATe:ALL?";
        public const string CMD_CELL_PSwitched_STATE = "FETCh:LTE:SIGN:PSWitched:STATe?";
        public const string CMD_CELL_PSwitched_CONN = "CALL:LTE:SIGN:PSWitched:ACTion CONNect";
        public const string CMD_CELL_PSwitched_DISCONN = "CALL:LTE:SIGN:PSWitched:ACTion DISConnect";
        public const string CMD_CELL_PSwitched_DETACH = "CALL:LTE:SIGN:PSWitched:ACTion DETach";
        public const string CMD_CELL_RRC_STATE = "SENSe:LTE:SIGN:RRCState?";
        public const string CMD_UEINFO_IMEI = "SENSe:LTE:SIGN:UESinfo:IMEI?";
        public const string CMD_UEINFO_IMSI = "SENSe:LTE:SIGN:UESinfo:IMSI?";
        public const string CMD_SET_DMODE_FDD = "CONFigure:LTE:SIGN:DMODe FDD";
        public const string CMD_SET_RF_OUTPUT_EATT = "CONFigure:LTE:SIGN:RFSettings:EATTenuation:OUTPut 2";
        public const string CMD_SET_RF_INPUT_EATT = "CONFigure:LTE:SIGN:RFSettings:EATTenuation:INPut 2";
        public const string CMD_SET_BAND_N = "CONFigure:LTE:SIGN:PCC:BAND OB{0}";
        public const string CMD_BAND_CUR = "CONFigure:LTE:SIGN:BAND?";
        public const string CMD_SET_BAND_3 = "CONFigure:LTE:SIGN:PCC:BAND OB3";
        public const string CMD_SET_BAND_7 = "CONFigure:LTE:SIGN:PCC:BAND OB7";
        public const string CMD_SET_BAND_28 = "CONFigure:LTE:SIGN:PCC:BAND OB28";
        public const string CMD_SET_RS_EPRE = "CONFigure:LTE:SIGN:DL:PCC:RSEPre:LEVel -57.8";
        public const string CMD_LTE_MEAS_INIT = "INIT:LTE:MEAS:MEValuation";
        public const string CMD_LTE_MEAS_STOP = "STOP:LTE:MEAS:MEValuation";
        public const string CMD_LTE_MEAS_ABORT = "ABORt:LTE:MEAS:MEValuation";
        public const string CMD_LTE_MEAS_STATE = "FETCh:LTE:MEAS:MEValuation:STATe?";
        public const string CMD_SET_LTE_MEAS_SCENario_CSPath = "ROUTe:LTE:MEAS:SCENario:CSPath \"LTE Sig1\"";
        public const string CMD_LTE_MEAS_SCENario = "ROUTe:LTE:MEAS:SCENario?";
        public const string CMD_LTE_MEAS_REPetition_CONT = "CONFigure:LTE:MEAS:MEValuation:REPetition CONTinuous";
        public const string CMD_LTE_MEAS_TXPOWER_CURRENT = "FETCh:LTE:MEAS:MEValuation:SEMask:CURRent?";
        public const string CMD_LTE_MEAS_TXPOWER_AVERAGE = "FETCh:LTE:MEAS:MEValuation:SEMask:AVERage?";
        public const string CMD_LTE_MEAS_TXPOWER_EXTREME = "FETCh:LTE:MEAS:MEValuation:SEMask:EXTReme?";

        // GPRF 非信令测试
        public const string GPRF_MEAS_SCENARIO = "ROUTe:GPRF:MEASurement:SCENario:SALone RFAC, RX1";
        public const string GPRF_GEN_SCENARIO = "ROUTe:GPRF:GENerator:SCENario:SALone RFAC, TX1";
        public const string GPRF_MEAS_EATTENUATION = "CONFigure:GPRF:MEAS:RFSettings:EATTenuation {0}";
        public const string GPRF_GEN_EATTENUATION = "SOURce:GPRF:GENerator:RFSettings:EATTenuation {0}";
        public const string GPRF_MEAS_RF_FREQUENCY = "CONFigure:GPRF:MEASurement:RFSettings:FREQuency {0}";
        public const string GPRF_MEAS_GET_RF_FREQUENCY = "CONFigure:GPRF:MEASurement:RFSettings:FREQuency?";
        public const string GPRF_GEN_RF_FREQUENCY = "SOURce:GPRF:GENerator:RFSettings:FREQuency {0}";
        public const string GPRF_GEN_STATE_ON = "SOURce:GPRF:GEN:STATe ON";
        public const string GPRF_GEN_STATE_OFF = "SOURce:GPRF:GEN:STATe OFF";
        public const string GPRF_GEN_STATE = "SOURce:GPRF:GEN:STATe?";
        public const string GPRF_GEN_BBMODE_CW = "SOURce:GPRF:GEN:BBMode CW";
        public const string GPRF_GEN_RF_LEVEL = "SOURce:GPRF:GEN:RFSettings:LEVel {0}";
        public const string GPRF_MEAS_POWER_FILTER_TYPE_BANDPASS = "CONFigure:GPRF:MEASurement:POWer:FILTer:TYPE BANDpass";
        public const string GPRF_MEAS_POWER_FILTER_TYPE_BANDPASS_BWIDTH = "CONFigure:GPRF:MEASurement:POWer:FILTer:BANDpass:BWIDth {0}MHz";
        public const string GPRF_MEAS_POWER_SOUR_FREERUN = "TRIGger:GPRF:MEAS:POWer:SOUR 'Free Run'";
        public const string GPRF_MEAS_POWER_REPETITION_SING = "CONFigure:GPRF:MEASurement:POWer:REPetition SINGleshot";
        public const string GPRF_MEAS_POWER_SCOUNT = "CONFigure:GPRF:MEASurement:POWer:SCOunt {0}";
        public const string GPRF_MEAS_RF_UMARGIN = "CONFigure:GPRF:MEASurement:RFSettings:UMARgin {0}";
        public const string GPRF_MEAS_RF_ENPOWER = "CONFigure:GPRF:MEAS:RFSettings:ENPower {0}";
        public const string GPRF_MEAS_POWER_INIT = "INIT:GPRF:MEASurement:POWer;*OPC?";
        public const string GPRF_MEAS_POWER_STATE = "FETC:GPRF:MEAS:POWer:STAT?";
        public const string GPRF_MEAS_POWER_CURRENT = "FETCh:GPRF:MEASurement:POWer:CURRent?";
        public const string GPRF_MEAS_POWER_ABORT = "ABORt:GPRF:MEAS:POWer;*OPC?";

        private string resourceName;
        private MessageBasedSession session;

        public CMW500Controler(string resourceName)
        {
            this.resourceName = resourceName;
        }

        // 打开与设备的连接
        public void Open()
        {
            try
            {
                session = (MessageBasedSession)ResourceManager.GetLocalManager().Open(resourceName);
                session.Timeout = 5000;
                Console.WriteLine("Connection opened to " + resourceName);
            }
            catch (VisaException ex)
            {
                Console.WriteLine("Error opening session: " + ex.Message);
            }
        }

        // 发送命令到设备
        public bool SendCommandWithResponse(string command, out SCPIRunDetail sCPIRunDetail)
        {
            sCPIRunDetail = new SCPIRunDetail();
            sCPIRunDetail.CMD = command;
            if (session == null)
            {
                sCPIRunDetail.Error = "Session is not open.";
                return false;
            }

            try
            {
                session.Write(command);
                if (!command.Contains("?"))
                {
                    return true; // 不需要读取响应
                }
                string response = session.ReadString();
                sCPIRunDetail.Response = response;
                return true;
            }
            catch (VisaException ex)
            {
                Console.WriteLine("Error sending command: " + ex.Message);
                sCPIRunDetail.Error = ex.Message;
                return false;
            }
        }

        // 关闭与设备的连接
        public void Close()
        {
            if (session != null)
            {
                session.Dispose();
                session = null;
                Console.WriteLine("Connection closed.");
            }
        }
    }

    public class SCPIRunDetail
    {
        public string CMD {  get; set; }
        public string Response { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return $"CMD:\r\n-> {CMD};\r\nResponse:\r\n<- \r\n{Response}-- Error:\r\n{Error}\r\n";
        }
    }

    public class MeasResult
    {
        public string ModulationType { get; set; }
        public int PayloadLength { get; set; }
        public double BrustPower { get; set; } //dBm
        public double EVMAllCarriers { get; set; } //dB
        public double EVMDataCarriers { get; set; } // dB
        public double EVMPilotCarriers { get; set; } // dB
        public double CenterFrequencyError { get; set; } // Hz
        public double SymbolClockError { get; set; } // ppm
        public double IQOffset { get; set; } // dB
        public double GainImbalance { get; set; } // dB
        public double QuadratureError { get; set; } // °
        public double OutOfTolerance { get; set; } // %
        public double GuardInterval { get; set; } // Only relevant for 802.11n.
        public double BrustRate { get; set; } // %

        public override string ToString()
        {
            return $"ModulationType: {ModulationType}\n" +
               $"PayloadLength: {PayloadLength}\n" +
               $"BrustPower: {BrustPower} dBm\n" +
               $"EVMAllCarriers: {EVMAllCarriers} dB\n" +
               $"EVMDataCarriers: {EVMDataCarriers} dB\n" +
               $"EVMPilotCarriers: {EVMPilotCarriers} dB\n" +
               $"CenterFrequencyError: {CenterFrequencyError} Hz\n" +
               $"SymbolClockError: {SymbolClockError} ppm\n" +
               $"IQOffset: {IQOffset} dB\n" +
               $"GainImbalance: {GainImbalance} dB\n" +
               $"QuadratureError: {QuadratureError} °\n" +
               $"OutOfTolerance: {OutOfTolerance} %\n" +
               $"GuardInterval: {GuardInterval}\n" +
               $"BrustRate: {BrustRate} %"; ;
        }
    }

    public class TestCMW500
    {
        public static void Test()
        {
            // 替换为实际的设备资源名称
            string resourceName = "TCPIP0::10.10.10.12::inst0::INSTR";

            CMW500Controler cMW500Controler = new CMW500Controler(resourceName);
            cMW500Controler.Open();

            // 发送SCPI命令以获取设备标识
            cMW500Controler.SendCommandWithResponse("*IDN?", out SCPIRunDetail sCPIRunDetail);
            Console.WriteLine(sCPIRunDetail.ToString());

            /*
             * // 发送其他命令
            cMW500Controler.SendCommand("CONFigure:LTE:SIGN:CELL:BAND 1");
            */
            cMW500Controler.SendCommandWithResponse("FETCH:LTE:SIGN:CELL:BAND?", out SCPIRunDetail sCPIRunDetail1);
            Console.WriteLine(sCPIRunDetail1.ToString());

            // 关闭会话
            cMW500Controler.Close();
        }
    }
}
