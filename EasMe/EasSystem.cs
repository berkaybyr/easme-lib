using EasMe.Exceptions;
using EasMe.Extensions;
using EasMe.Models.SystemModels;
using Microsoft.Win32;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;

namespace EasMe
{
    public static class EasSystem
    {
        //private  ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        //private  ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
        internal static HWIDModel Hwid { get; set; }
        internal static string HashedHwid { get; set; }

        enum HardwareType
        {
            EthernetMAC = 0,
            Baseboard = 1,
            CPU = 2,
            Disk = 3,
        }
        #region Read System.Management
        private static string GetIdentifier(string wmiClass, string wmiProperty)
        {
            var result = "";

            try
            {
                ManagementClass mc =
            new ManagementClass(wmiClass);
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    //Only get the first one
                    if (string.IsNullOrEmpty(result))
                    {
                        try
                        {
                            var prop = mo[wmiProperty];
                            if (prop != null)
                                result = prop.ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
            if (string.IsNullOrEmpty(result)) return "Unknown";
            return result;

        }
        //private static string GetIdentifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        //{
        //    string? result = "";

        //    try
        //    {
        //        ManagementClass mc =
        //    new ManagementClass(wmiClass);
        //        ManagementObjectCollection moc = mc.GetInstances();
        //        foreach (ManagementObject mo in moc)
        //        {
        //            if (mo[wmiMustBeTrue].ToString() == "True")
        //            {
        //                //Only get the first one
        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    try
        //                    {
        //                        var prop = mo[wmiProperty];
        //                        if (prop != null)
        //                            result = prop.ToString();
        //                        break;
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    if (string.IsNullOrEmpty(result)) return "Unknown";
        //    return result;

        //}

        private static List<ManagementObject> GetManagementObjList(string className, string searchCol = "*")
        {
            var list = new List<ManagementObject>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select " + searchCol + " from " + className);
            foreach (ManagementObject obj in searcher.Get())
            {
                if (obj == null)
                    continue;
                list.Add(obj);
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Returns this computers MAC Address.
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            try
            {
                var macAddr = (
                                from nic in NetworkInterface.GetAllNetworkInterfaces()
                                where nic.OperationalStatus == OperationalStatus.Up
                                select nic.GetPhysicalAddress().ToString()
                            ).FirstOrDefault();
                if (!string.IsNullOrEmpty(macAddr))
                    return macAddr;
            }
            catch
            {
            }
            return "NOT_FOUND";

        }

        /// <summary>
        /// Returns this computers Ram information as a list of RamModel object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static List<RamModel> GetRamList()
        {
            try
            {
                var list = new List<RamModel>();

                foreach (var obj in GetManagementObjList("Win32_PhysicalMemory"))
                {
                    var ramModel = new RamModel();
                    ramModel.Attributes = obj.Properties["Attributes"].Value.ToString();
                    ramModel.Capacity = obj.Properties["Capacity"].Value.ToString();
                    ramModel.Caption = obj.Properties["Caption"].Value.ToString();
                    ramModel.DeviceLocator = obj.Properties["DeviceLocator"].Value.ToString();
                    ramModel.FormFactor = obj.Properties["FormFactor"].Value.ToString();
                    ramModel.Manufacturer = obj.Properties["Manufacturer"].Value.ToString();
                    ramModel.Name = obj.Properties["Name"].Value.ToString();
                    ramModel.PartNumber = obj.Properties["PartNumber"].Value.ToString();
                    ramModel.SerialNumber = obj.Properties["SerialNumber"].Value.ToString();
                    ramModel.Speed = obj.Properties["Speed"].Value.ToString();
                    ramModel.Tag = obj.Properties["Tag"].Value.ToString();
                    ramModel.TotalWidth = obj.Properties["TotalWidth"].Value.ToString();
                    ramModel.TypeDetail = obj.Properties["TypeDetail"].Value.ToString();
                    ramModel.BankLabel = obj.Properties["BankLabel"].Value.ToString();
                    ramModel.ConfiguredClockSpeed = obj.Properties["ConfiguredClockSpeed"].Value.ToString();
                    ramModel.ConfiguredVoltage = obj.Properties["ConfiguredVoltage"].Value.ToString();
                    ramModel.CreationClassName = obj.Properties["CreationClassName"].Value.ToString();
                    ramModel.DataWidth = obj.Properties["DataWidth"].Value.ToString();
                    ramModel.Description = obj.Properties["Description"].Value.ToString();
                    ramModel.MaxVoltage = obj.Properties["MaxVoltage"].Value.ToString();
                    ramModel.MinVoltage = obj.Properties["MinVoltage"].Value.ToString();
                    ramModel.SMBIOSMemoryType = obj.Properties["SMBIOSMemoryType"].Value.ToString();
                    list.Add(ramModel);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetRamList", ex);
            }

        }

        /// <summary>
        /// Returns this computers Motherboard information as a MotherboardModel object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static MotherboardModel GetMotherboard()
        {
            var motherboardModel = new MotherboardModel();

            try
            {
                var item = GetManagementObjList("Win32_BaseBoard").FirstOrDefault();
                motherboardModel.Caption = item["Caption"].ToString();
                motherboardModel.ConfigOptions = item["ConfigOptions"].ToString();
                motherboardModel.CreationClassName = item["CreationClassName"].ToString();
                motherboardModel.Description = item["Description"].ToString();
                motherboardModel.HostingBoard = item["HostingBoard"].ToString();
                motherboardModel.HotSwappable = item["HotSwappable"].ToString();
                motherboardModel.Manufacturer = item["Manufacturer"].ToString();
                motherboardModel.Name = item["Name"].ToString();
                motherboardModel.PoweredOn = item["PoweredOn"].ToString();
                motherboardModel.Product = item["Product"].ToString();
                motherboardModel.Removable = item["Removable"].ToString();
                motherboardModel.Replaceable = item["Replaceable"].ToString();
                motherboardModel.RequiresDaughterBoard = item["RequiresDaughterBoard"].ToString();
                motherboardModel.SerialNumber = item["SerialNumber"].ToString();
                motherboardModel.Status = item["Status"].ToString();
                motherboardModel.Tag = item["Tag"].ToString();
                motherboardModel.Version = item["Version"].ToString();
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetMotherboard", ex);
            }
            return motherboardModel;


        }

        /// <summary>
        /// Returns this computers Processor information as a CPUModel object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static CPUModel GetProcessor()
        {
            var CPUModel = new CPUModel();
            try
            {
                var item = GetManagementObjList("Win32_Processor").FirstOrDefault();
                CPUModel.AddressWidth = item["AddressWidth"].ToString();
                CPUModel.Architecture = item["Architecture"].ToString();
                CPUModel.AssetTag = item["AssetTag"].ToString();
                CPUModel.Availability = item["Availability"].ToString();
                CPUModel.Caption = item["Caption"].ToString();
                CPUModel.Characteristics = item["Characteristics"].ToString();
                CPUModel.CpuStatus = item["CpuStatus"].ToString();
                CPUModel.CreationClassName = item["CreationClassName"].ToString();
                CPUModel.CurrentClockSpeed = item["CurrentClockSpeed"].ToString();
                CPUModel.CurrentVoltage = item["CurrentVoltage"].ToString();
                CPUModel.DataWidth = item["DataWidth"].ToString();
                CPUModel.Description = item["Description"].ToString();
                CPUModel.DeviceID = item["DeviceID"].ToString();
                CPUModel.ExtClock = item["ExtClock"].ToString();
                CPUModel.Family = item["Family"].ToString();
                CPUModel.L2CacheSize = item["L2CacheSize"].ToString();
                CPUModel.L3CacheSize = item["L3CacheSize"].ToString();
                CPUModel.L3CacheSpeed = item["L3CacheSpeed"].ToString();
                CPUModel.Level = item["Level"].ToString();
                CPUModel.LoadPercentage = item["LoadPercentage"].ToString();
                CPUModel.Manufacturer = item["Manufacturer"].ToString();
                CPUModel.MaxClockSpeed = item["MaxClockSpeed"].ToString();
                CPUModel.Name = item["Name"].ToString();
                CPUModel.NumberOfCores = item["NumberOfCores"].ToString();
                CPUModel.NumberOfLogicalProcessors = item["NumberOfLogicalProcessors"].ToString();
                CPUModel.PartNumber = item["PartNumber"].ToString();
                CPUModel.PowerManagementSupported = item["PowerManagementSupported"].ToString();
                CPUModel.ProcessorId = item["ProcessorId"].ToString();
                CPUModel.ProcessorType = item["ProcessorType"].ToString();
                CPUModel.Revision = item["Revision"].ToString();
                CPUModel.Role = item["Role"].ToString();
                CPUModel.SecondLevelAddressTranslationExtensions = item["SecondLevelAddressTranslationExtensions"].ToString();
                CPUModel.SerialNumber = item["SerialNumber"].ToString();
                CPUModel.SocketDesignation = item["SocketDesignation"].ToString();
                CPUModel.Status = item["Status"].ToString();
                CPUModel.StatusInfo = item["StatusInfo"].ToString();
                CPUModel.Stepping = item["Stepping"].ToString();
                CPUModel.SystemCreationClassName = item["SystemCreationClassName"].ToString();
                CPUModel.SystemName = item["SystemName"].ToString();
                CPUModel.ThreadCount = item["ThreadCount"].ToString();
                CPUModel.UpgradeMethod = item["UpgradeMethod"].ToString();
                CPUModel.Version = item["Version"].ToString();
                CPUModel.VirtualizationFirmwareEnabled = item["VirtualizationFirmwareEnabled"].ToString();
                CPUModel.VMMonitorModeExtensions = item["VMMonitorModeExtensions"].ToString();
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetProcessor", ex);
            }
            return CPUModel;


        }

        /// <summary>
        /// Returns this computers Disk information as a list of DiskModel objects.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static List<DiskModel> GetDiskList()
        {
            var list = new List<DiskModel>();
            try
            {
                var disk = GetManagementObjList("Win32_DiskDrive").FirstOrDefault();
                var model = new DiskModel();
                model.BytesPerSector = disk["BytesPerSector"].ToString();
                model.Capabilities = disk["Capabilities"].ToString();
                model.CapabilityDescriptions = disk["CapabilityDescriptions"].ToString();
                model.Caption = disk["Caption"].ToString();
                model.ConfigManagerErrorCode = disk["ConfigManagerErrorCode"].ToString();
                model.ConfigManagerUserConfig = disk["ConfigManagerUserConfig"].ToString();
                model.CreationClassName = disk["CreationClassName"].ToString();
                model.FirmwareRevision = disk["FirmwareRevision"].ToString();
                model.Index = disk["Index"].ToString();
                model.InterfaceType = disk["InterfaceType"].ToString();
                model.Manufacturer = disk["Manufacturer"].ToString();
                model.MediaLoaded = disk["MediaLoaded"].ToString();
                model.MediaType = disk["MediaType"].ToString();
                model.Model = disk["Model"].ToString();
                model.Name = disk["Name"].ToString();
                model.Partitions = disk["Partitions"].ToString();
                model.PNPDeviceID = disk["PNPDeviceID"].ToString();
                model.SCSIBus = disk["SCSIBus"].ToString();
                model.SCSILogicalUnit = disk["SCSILogicalUnit"].ToString();
                model.SCSIPort = disk["SCSIPort"].ToString();
                model.SCSITargetId = disk["SCSITargetId"].ToString();
                model.SectorsPerTrack = disk["SectorsPerTrack"].ToString();
                model.SerialNumber = disk["SerialNumber"].ToString();
                model.Size = disk["Size"].ToString();
                model.Status = disk["Status"].ToString();
                model.SystemCreationClassName = disk["SystemCreationClassName"].ToString();
                model.SystemName = disk["SystemName"].ToString();
                model.TotalCylinders = disk["TotalCylinders"].ToString();
                model.TotalHeads = disk["TotalHeads"].ToString();
                model.TotalSectors = disk["TotalSectors"].ToString();
                model.TotalTracks = disk["TotalTracks"].ToString();
                model.TracksPerCylinder = disk["TracksPerCylinder"].ToString();
                list.Add(model);
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetDiskList", ex);
            }
            return list;

        }


        /// <summary>
        /// Returns this computers GPU information as a list of GPUModel objects.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static List<GPUModel> GetGPUList()
        {
            var list = new List<GPUModel>();
            try
            {
                var GPUList = GetManagementObjList("Win32_VideoController");
                foreach (var video in GPUList)
                {
                    var model = new GPUModel();
                    model.AdapterRAM = video["AdapterRAM"].ToString();
                    model.Availability = video["Availability"].ToString();
                    model.Caption = video["Caption"].ToString();
                    model.ConfigManagerErrorCode = video["ConfigManagerErrorCode"].ToString();
                    model.ConfigManagerUserConfig = video["ConfigManagerUserConfig"].ToString();
                    model.CreationClassName = video["CreationClassName"].ToString();
                    model.CurrentBitsPerPixel = video["CurrentBitsPerPixel"].ToString();
                    model.CurrentHorizontalResolution = video["CurrentHorizontalResolution"].ToString();
                    model.CurrentNumberOfColors = video["CurrentNumberOfColors"].ToString();
                    model.CurrentNumberOfColumns = video["CurrentNumberOfColumns"].ToString();
                    model.CurrentNumberOfRows = video["CurrentNumberOfRows"].ToString();
                    model.CurrentScanMode = video["CurrentScanMode"].ToString();
                    model.CurrentVerticalResolution = video["CurrentVerticalResolution"].ToString();
                    model.Description = video["Description"].ToString();
                    model.DeviceID = video["DeviceID"].ToString();
                    model.DriverVersion = video["DriverVersion"].ToString();
                    model.InstalledDisplayDrivers = video["InstalledDisplayDrivers"].ToString();
                    model.MaxRefreshRate = video["MaxRefreshRate"].ToString();
                    model.MinRefreshRate = video["MinRefreshRate"].ToString();
                    model.Name = video["Name"].ToString();
                    model.PNPDeviceID = video["PNPDeviceID"].ToString();
                    model.Status = video["Status"].ToString();
                    model.SystemCreationClassName = video["SystemCreationClassName"].ToString();
                    model.SystemName = video["SystemName"].ToString();
                    model.VideoArchitecture = video["VideoArchitecture"].ToString();
                    model.VideoMemoryType = video["VideoMemoryType"].ToString();
                    model.VideoProcessor = video["VideoProcessor"].ToString();
                    model.AdapterCompatibility = video["AdapterCompatibility"].ToString();
                    model.AdapterDACType = video["AdapterDACType"].ToString();
                    model.CurrentRefreshRate = video["CurrentRefreshRate"].ToString();
                    model.DitherType = video["DitherType"].ToString();
                    model.DriverDate = video["DriverDate"].ToString();
                    model.InfFilename = video["InfFilename"].ToString();
                    model.InfSection = video["InfSection"].ToString();
                    model.Monochrome = video["Monochrome"].ToString();
                    model.VideoModeDescription = video["VideoModeDescription"].ToString();
                    list.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetGPUList", ex);
            }
            return list;
        }

        /// <summary>
        /// Returns this computers BIOS information as a BIOSModel object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static BIOSModel GetBIOS()
        {
            var model = new BIOSModel();
            try
            {
                var bios = GetManagementObjList("Win32_BIOS").FirstOrDefault();
                model.BIOSVersion = bios["BIOSVersion"].ToString();
                model.BiosCharacteristics = bios["BiosCharacteristics"].ToString();
                model.Caption = bios["Caption"].ToString();
                model.Description = bios["Description"].ToString();
                model.EmbeddedControllerMajorVersion = bios["EmbeddedControllerMajorVersion"].ToString();
                model.EmbeddedControllerMinorVersion = bios["EmbeddedControllerMinorVersion"].ToString();
                model.Manufacturer = bios["Manufacturer"].ToString();
                model.Name = bios["Name"].ToString();
                model.PrimaryBIOS = bios["PrimaryBIOS"].ToString();
                model.ReleaseDate = bios["ReleaseDate"].ToString();
                model.SerialNumber = bios["SerialNumber"].ToString();
                model.SMBIOSBIOSVersion = bios["SMBIOSBIOSVersion"].ToString();
                model.SMBIOSMajorVersion = bios["SMBIOSMajorVersion"].ToString();
                model.SMBIOSMinorVersion = bios["SMBIOSMinorVersion"].ToString();
                model.SMBIOSPresent = bios["SMBIOSPresent"].ToString();
                model.SoftwareElementID = bios["SoftwareElementID"].ToString();
                model.SoftwareElementState = bios["SoftwareElementState"].ToString();
                model.Status = bios["Status"].ToString();
                model.SystemBiosMajorVersion = bios["SystemBiosMajorVersion"].ToString();
                model.SystemBiosMinorVersion = bios["SystemBiosMinorVersion"].ToString();
                model.TargetOperatingSystem = bios["TargetOperatingSystem"].ToString();
                model.Version = bios["Version"].ToString();
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetBIOS", ex);
            }
            return model;
        }


        public static string GetMotherboardSerial()
        {
            try
            {
                var serial = GetIdentifier("Win32_BaseBoard", "SerialNumber");
                return serial;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetMotherboardSerial", ex);
            }
        }
        public static string GetProcessorId()
        {
            try
            {
                var id = GetIdentifier("Win32_Processor", "ProcessorId");
                return id;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetProcessorId", ex);
            }
        }
        public static string GetDiskSerial()
        {
            try
            {
                var id = GetIdentifier("Win32_DiskDrive", "SerialNumber");
                return id;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetDiskSerial", ex);
            }
        }
        public static string GetVideoProcessorName()
        {
            try
            {
                var id = GetIdentifier("Win32_VideoController", "VideoProcessor");
                return id;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetVideoProcessorName", ex);
            }
        }
        public static string GetTimezone()
        {
            return TimeZoneInfo.Local.StandardName;

        }

        public static string GetOSVersion()
        {
            return Environment.OSVersion.ToString();
        }

        public static string GetMachineName()
        {
            return Environment.MachineName.ToString();

        }

        public static string GetThreadId()
        {
            return Environment.CurrentManagedThreadId.ToString();
        }


        public static string GetRemoteIPAddress()
        {
            try
            {
                var httpClient = new HttpClient();
                var response = httpClient.GetAsync("https://api.ipify.org/").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return "";
                //throw new EasException("GetRemoteIPAddress", ex);
            }
        }
        /// <summary>
        /// Gets MachineGUID assigned by Windows.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="EasException"></exception>
        private static string GetMachineGuid()
        {
            try
            {
                string location = @"SOFTWARE\Microsoft\Cryptography";
                string name = "MachineGuid";

                using RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using RegistryKey rk = localMachineX64View.OpenSubKey(location);
                if (rk == null)
                    throw new KeyNotFoundException("Cannot find the key: " + location);
                object machineGuid = rk.GetValue(name);
                if (machineGuid == null)
                    throw new IndexOutOfRangeException("Cannot find the value: " + name);

                return machineGuid.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("GetMachineGuid", ex);
            }
        }

        /// <summary>
        /// Returns Disk UUID from Win32_ComputerSystemProduct
        /// </summary>
        /// <returns></returns>
        private static string GetDiskUUID()
        {
            string run = "get-wmiobject Win32_ComputerSystemProduct  | Select-Object -ExpandProperty UUID";
            var oProcess = new Process();
            var oStartInfo = new ProcessStartInfo("powershell.exe", run);
            oStartInfo.UseShellExecute = false;
            oStartInfo.RedirectStandardInput = true;
            oStartInfo.RedirectStandardOutput = true;
            oStartInfo.CreateNoWindow = true;
            oProcess.StartInfo = oStartInfo;
            oProcess.Start();
            oProcess.WaitForExit();
            var result = oProcess.StandardOutput.ReadToEnd();
            return result.TrimAbsolute();
        }
        /// <summary>
        /// Returns List of Hardware Ids, first two is reliable for general usage.
        /// </summary>
        /// <returns></returns>
        private static List<string> GetHardwareIds()
        {
            List<string> list = new();
            var model = GetHardwareModel();
            var id1 = $"{model.ProcessorId}:{model.MotherboardId}:{model.BiosId}:{model.MACAddresses}";
            list.Add(id1.TrimAbsolute());
            var id2 = $"{model.DiskUUID}:{model.MachineGuid}:{model.MachineName}";
            list.Add(id2.TrimAbsolute());
            var id3 = $"{model.GPU}";
            list.Add(id3.TrimAbsolute());
            var id4 = $"{model.Ram}";
            list.Add(id4.TrimAbsolute());
            var id5 = $"{model.Disk}";
            list.Add(id5.TrimAbsolute());
            return list;
        }
        /// <summary>
        /// Returns list of Hardware Ids MD5Hashed Ids first two is reliable for general usage.
        /// </summary>
        /// <returns></returns>
        private static List<string> GetHashedHardwareIds()
        {
            List<string> list = GetHardwareIds();
            List<string> newList = new();
            foreach (var item in list)
            {
                var hashed = EasHash.BuildString(EasHash.MD5Hash(item)).ToUpper();
                newList.Add(hashed);
            }

            return newList;
        }

        /// <summary>
        /// Returns this computers Hardware Model.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static HWIDModel GetHardwareModel()
        {
            try
            {
                if (Hwid != null) return Hwid;
                var processor = GetProcessor();
                var bios = GetBIOS();
                var mainboard = GetMotherboard();
                var gpuList = GetGPUList();
                var diskList = GetDiskList();
                var ramList = GetRamList();

                var hwidModel = new HWIDModel();
                hwidModel.MachineName = GetMachineName();
                hwidModel.MACAddresses = GetMACAddress();
                hwidModel.DiskUUID = GetDiskUUID();
                hwidModel.MachineGuid = GetMachineGuid();

                var processorIdentifier = $"{processor.Name}:{processor.Manufacturer}:{processor.ProcessorId}";
                hwidModel.ProcessorId = processorIdentifier;

                //var ramIdentifier = string.Join("::", ramList.Select(x => $"{x.Name}:{x.Manufacturer}:{x.SerialNumber}"));
                //var gpuIdentifier = string.Join("::", gpuList.Select(x => $"{x.Name}"));                
                //var diskIdentifier = string.Join("::", diskList.Select(x => $"{x.Name}:{x.Manufacturer}:{x.SerialNumber}:{x.Size}"));
                if (ramList != null || ramList.Count > 0)
                {
                    var ramIdentifier = string.Join("::", ramList.Select(x => $"{x.Name}:{x.Manufacturer}:{x.SerialNumber}"));
                    hwidModel.Ram = ramIdentifier;
                }
                if (gpuList != null || gpuList.Count > 0)
                {
                    var gpuIdentifier = string.Join("::", gpuList.Select(x => $"{x.Name}"));
                    hwidModel.GPU = gpuIdentifier;
                }
                if (diskList != null || diskList.Count > 0)
                {
                    var diskIdentifier = string.Join("::", diskList.Select(x => $"{x.SerialNumber}:{x.Size}"));
                    hwidModel.Disk = diskIdentifier;
                }

                var biosIdentifier = $"{bios.Manufacturer}:{bios.SMBIOSBIOSVersion}:{bios.SerialNumber}";
                hwidModel.BiosId = biosIdentifier;

                var mainboardIdentifier = $"{mainboard.Manufacturer}:{mainboard.SerialNumber}";
                hwidModel.MotherboardId = mainboardIdentifier;


                Hwid = hwidModel;
                return hwidModel;
            }
            catch (Exception ex)
            {
                throw new FailedToReadException("Getting machine ids", ex);
            }
        }
        public static string GetMachineIdHashed()
        {
            if (HashedHwid != null) return HashedHwid;
            var hashed = GetHashedHardwareIds().FirstOrDefault();
            if (string.IsNullOrEmpty(hashed)) return "NOT_FOUND";
            HashedHwid = hashed;
            return hashed;
        }
    }
}
