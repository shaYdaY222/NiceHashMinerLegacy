﻿using System.Collections.Generic;
using System.Linq;
using NiceHashMiner.Algorithms;
using NiceHashMinerLegacy.Common.Enums;

namespace NiceHashMiner.Devices.Algorithms
{
    /// <summary>
    /// GroupAlgorithms creates defaults supported algorithms. Currently based in Miner implementation
    /// </summary>
    public static class GroupAlgorithms
    {
        private static Dictionary<MinerBaseType, List<Algorithm>> CreateForDevice(ComputeDevice device)
        {
            if (device == null) return null;
            var algoSettings = CreateDefaultsForGroup(device.DeviceGroupType);
            if (algoSettings == null) return null;
            if (device.DeviceType == DeviceType.AMD && device is AmdComputeDevice amd)
            {
                // sgminer stuff
                if (algoSettings.ContainsKey(MinerBaseType.sgminer))
                {
                    var sgminerAlgos = algoSettings[MinerBaseType.sgminer];
                    var neoScryptIndex = sgminerAlgos.FindIndex(el => el.NiceHashID == AlgorithmType.NeoScrypt);
                    if (!device.Codename.Contains("Tahiti") && neoScryptIndex > -1)
                    {
                        sgminerAlgos[neoScryptIndex].ExtraLaunchParameters =
                            AmdGpuDevice.DefaultParam +
                            "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2";
                        Helpers.ConsolePrint("ComputeDevice",
                            "The GPU detected (" + device.Codename +
                            ") is not Tahiti. Changing default gpu-threads to 2.");
                    }
                }

                // non sgminer optimizations, this looks like dead code to me
                if (algoSettings.ContainsKey(MinerBaseType.Claymore_old) &&
                    algoSettings.ContainsKey(MinerBaseType.Claymore))
                {
                    var claymoreOldAlgos = algoSettings[MinerBaseType.Claymore_old];
                    var cryptoNightOldIndex =
                        claymoreOldAlgos.FindIndex(el => el.NiceHashID == AlgorithmType.CryptoNight_UNUSED);

                    var claymoreNewAlgos = algoSettings[MinerBaseType.Claymore];
                    var cryptoNightNewIndex =
                        claymoreNewAlgos.FindIndex(el => el.NiceHashID == AlgorithmType.CryptoNight_UNUSED);

                    if (cryptoNightOldIndex > -1 && cryptoNightNewIndex > -1)
                    {
                        //string regex_a_3 = "[5|6][0-9][0-9][0-9]";
                        var a4 = new List<string>
                        {
                            "270",
                            "270x",
                            "280",
                            "280x",
                            "290",
                            "290x",
                            "370",
                            "380",
                            "390",
                            "470",
                            "480"
                        };
                        foreach (var namePart in a4)
                        {
                            if (!device.Name.Contains(namePart)) continue;
                            claymoreOldAlgos[cryptoNightOldIndex].ExtraLaunchParameters = "-a 4";
                            break;
                        }

                        var old = new List<string>
                        {
                            "Verde",
                            "Oland",
                            "Bonaire"
                        };
                        foreach (var codeName in old)
                        {
                            var isOld = device.Codename.Contains(codeName);
                            claymoreOldAlgos[cryptoNightOldIndex].Enabled = isOld;
                            claymoreNewAlgos[cryptoNightNewIndex].Enabled = !isOld;
                        }
                    }
                }

                // drivers algos issue
                if (device.DriverDisableAlgos)
                {
                    algoSettings = FilterMinerAlgos(algoSettings, new List<AlgorithmType>
                    {
                        AlgorithmType.NeoScrypt,
                    });
                }

                // disable by default
                {
                    var minerBases = new List<MinerBaseType>
                    {
                        MinerBaseType.ethminer,
                        MinerBaseType.BMiner
                    };
                    foreach (var minerKey in minerBases)
                    {
                        if (!algoSettings.ContainsKey(minerKey)) continue;
                        foreach (var algo in algoSettings[minerKey])
                        {
                            algo.Enabled = false;
                        }
                    }
                    if (algoSettings.ContainsKey(MinerBaseType.sgminer))
                    {
                        foreach (var algo in algoSettings[MinerBaseType.sgminer])
                        {
                            if (algo.NiceHashID == AlgorithmType.DaggerHashimoto)
                            {
                                algo.Enabled = false;
                            }
                        }
                    }
                }

                // Remove Beam on GCN 3rd gen or lower (300 series or lower)
                if (!amd.IsGcn4)
                {
                    algoSettings = FilterMinerAlgos(algoSettings, new List<AlgorithmType> { AlgorithmType.Beam });
                }
            } // END AMD case

            // check if it is Etherum capable
            if (device.IsEtherumCapale == false)
            {
                algoSettings = FilterMinerAlgos(algoSettings, new List<AlgorithmType>
                {
                    AlgorithmType.DaggerHashimoto
                });
            }

            if (algoSettings.ContainsKey(MinerBaseType.NBMiner))
            {
                if (device is CudaComputeDevice cudaDev)
                {
                    // Only SM 6.1
                    // check the ram
                    const ulong minGrin29Mem = 5 << 30;
                    const ulong minGrin31Mem = 8 << 30;
                    var isSM61 = cudaDev.SMMajor == 6 && cudaDev.SMMinor == 1;
                    if (isSM61 == false || cudaDev.GpuRam < minGrin29Mem)
                    {
                        algoSettings.Remove(MinerBaseType.NBMiner);
                    }
                    else if (isSM61 && cudaDev.GpuRam < minGrin31Mem) 
                    {
                        algoSettings[MinerBaseType.NBMiner] = algoSettings[MinerBaseType.NBMiner].Where(a => a.NiceHashID != AlgorithmType.GrinCuckatoo31).ToList();
                    }
                }
            }

            // disable CryptoNightR on AMD by default
            if (device.DeviceType == DeviceType.AMD && algoSettings.ContainsKey(MinerBaseType.XmrStak))
            {
                foreach (var algo in algoSettings[MinerBaseType.XmrStak])
                {
                    if (algo.NiceHashID == AlgorithmType.CryptoNightR) algo.Enabled = false;
                }
            }

            if (algoSettings.ContainsKey(MinerBaseType.ccminer_alexis))
            {
                foreach (var unstableAlgo in algoSettings[MinerBaseType.ccminer_alexis])
                {
                    unstableAlgo.Enabled = false;
                }
            }
            if (algoSettings.ContainsKey(MinerBaseType.experimental))
            {
                foreach (var unstableAlgo in algoSettings[MinerBaseType.experimental])
                {
                    unstableAlgo.Enabled = false;
                }
            }
            // GMiner filters
            if (algoSettings.ContainsKey(MinerBaseType.GMiner))
            {
                var filterAlgos = new List<AlgorithmType>{};
                const ulong MinZHashMemory = 1879047230; // 1.75GB
                if (device.GpuRam > MinZHashMemory == false) filterAlgos.Add(AlgorithmType.ZHash);
                
                const ulong MinBeamMemory = 3113849695; // 2.9GB
                if (device.GpuRam > MinBeamMemory == false) filterAlgos.Add(AlgorithmType.Beam);

                const ulong MinGrinCuckaroo29Memory = 6012951136; // 5.6GB
                if (device.GpuRam > MinGrinCuckaroo29Memory == false) filterAlgos.Add(AlgorithmType.GrinCuckaroo29);

                if (filterAlgos.Count > 0) algoSettings = FilterMinerAlgos(algoSettings, filterAlgos);
            }
            // GMiner filters
            if (algoSettings.ContainsKey(MinerBaseType.TTMiner))
            {
                foreach (var algo in algoSettings[MinerBaseType.TTMiner])
                {
                    algo.Enabled = false;
                }
            }
            // Disable all MTP algorithms by default
            // and all dual algorithms
            foreach (var algos in algoSettings.Values)
            {
                foreach (var algo in algos)
                {
                    if (algo.NiceHashID == AlgorithmType.MTP) algo.Enabled = false;
                    if (algo is DualAlgorithm) algo.Enabled = false;
                }
            }

            // disable by default
            {
                var minerBases = new List<MinerBaseType>
                    {
                        MinerBaseType.BMiner
                    };
                foreach (var minerKey in minerBases)
                {
                    if (!algoSettings.ContainsKey(minerKey)) continue;
                    foreach (var algo in algoSettings[minerKey])
                    {
                        algo.Enabled = false;
                    }
                }
            }
            return algoSettings;
        }

        public static List<Algorithm> CreateForDeviceList(ComputeDevice device)
        {
            var ret = new List<Algorithm>();
            var retDict = CreateForDevice(device);
            if (retDict != null)
            {
                foreach (var kvp in retDict)
                {
                    ret.AddRange(kvp.Value);
                }
            }
            return ret;
        }

        public static Dictionary<MinerBaseType, List<Algorithm>> CreateDefaultsForGroup(DeviceGroupType deviceGroupType)
        {
            switch (deviceGroupType)
            {
                case DeviceGroupType.CPU:
                    return DefaultAlgorithms.Cpu;

                case DeviceGroupType.AMD_OpenCL:
                    return DefaultAlgorithms.Amd;
                    
                case DeviceGroupType.NVIDIA_2_1:
                case DeviceGroupType.NVIDIA_3_x:
                case DeviceGroupType.NVIDIA_5_x:
                case DeviceGroupType.NVIDIA_6_x:
                    var toRemoveAlgoTypes = new List<AlgorithmType>();
                    var toRemoveMinerTypes = new List<MinerBaseType>();

                    var ret = DefaultAlgorithms.Nvidia;

                    switch (deviceGroupType)
                    {
                        case DeviceGroupType.NVIDIA_6_x:
                        case DeviceGroupType.NVIDIA_5_x:
                            toRemoveMinerTypes.AddRange(new[]
                            {
                                MinerBaseType.nheqminer
                            });
                            break;
                        case DeviceGroupType.NVIDIA_2_1:
                        case DeviceGroupType.NVIDIA_3_x:
                            toRemoveAlgoTypes.AddRange(new[]
                            {
                                AlgorithmType.NeoScrypt,
                                AlgorithmType.Lyra2REv3
                            });
                            toRemoveMinerTypes.AddRange(new[]
                            {
                                MinerBaseType.eqm,
                                MinerBaseType.EWBF,
                                MinerBaseType.dtsm
                            });
                            break;
                    }
                    if (DeviceGroupType.NVIDIA_2_1 == deviceGroupType)
                    {
                        toRemoveAlgoTypes.AddRange(new[]
                        {
                            AlgorithmType.DaggerHashimoto,
                            //AlgorithmType.CryptoNight,
                            AlgorithmType.Pascal,
                            //AlgorithmType.X11Gost,
                            AlgorithmType.X16R
                        });
                        toRemoveMinerTypes.AddRange(new[]
                        {
                            MinerBaseType.Claymore,
                            MinerBaseType.XmrStak
                        });
                    }

                    // filter unused
                    var finalRet = FilterMinerAlgos(ret, toRemoveAlgoTypes, new List<MinerBaseType>
                    {
                        MinerBaseType.ccminer
                    });
                    finalRet = FilterMinerBaseTypes(finalRet, toRemoveMinerTypes);

                    return finalRet;
            }

            return null;
        }

        private static Dictionary<MinerBaseType, List<Algorithm>> FilterMinerBaseTypes(
            Dictionary<MinerBaseType, List<Algorithm>> minerAlgos, List<MinerBaseType> toRemove)
        {
            var finalRet = new Dictionary<MinerBaseType, List<Algorithm>>();
            foreach (var kvp in minerAlgos)
            {
                if (toRemove.IndexOf(kvp.Key) == -1)
                {
                    finalRet[kvp.Key] = kvp.Value;
                }
            }
            return finalRet;
        }

        private static Dictionary<MinerBaseType, List<Algorithm>> FilterMinerAlgos(
            Dictionary<MinerBaseType, List<Algorithm>> minerAlgos, IList<AlgorithmType> toRemove,
            IList<MinerBaseType> toRemoveBase = null)
        {
            var finalRet = new Dictionary<MinerBaseType, List<Algorithm>>();
            if (toRemoveBase == null)
            {
                // all minerbasekeys
                foreach (var kvp in minerAlgos)
                {
                    var algoList = kvp.Value.FindAll(a => toRemove.IndexOf(a.NiceHashID) == -1);
                    if (algoList.Count > 0)
                    {
                        finalRet[kvp.Key] = algoList;
                    }
                }
            }
            else
            {
                foreach (var kvp in minerAlgos)
                {
                    // filter only if base key is defined
                    if (toRemoveBase.IndexOf(kvp.Key) > -1)
                    {
                        var algoList = kvp.Value.FindAll(a => toRemove.IndexOf(a.NiceHashID) == -1);
                        if (algoList.Count > 0)
                        {
                            finalRet[kvp.Key] = algoList;
                        }
                    }
                    else
                    {
                        // keep all
                        finalRet[kvp.Key] = kvp.Value;
                    }
                }
            }
            return finalRet;
        }
    }
}
