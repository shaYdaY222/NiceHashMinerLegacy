﻿using NiceHashMiner.Algorithms;
using NiceHashMinerLegacy.Common.Enums;
using NiceHashMinerLegacy.Extensions;
using System.Collections.Generic;

namespace NiceHashMiner.Devices.Algorithms
{
    public static class DefaultAlgorithms
    {
        #region All

        private static Dictionary<MinerBaseType, List<Algorithm>> All => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.XmrStak,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightV7, "cryptonight_v7"),
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightHeavy, "cryptonight_heavy"),
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightV8, "cryptonight_v8"),
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightR, "cryptonight_r"),
                }
            }
        };

        #endregion

        #region GPU

        private static Dictionary<MinerBaseType, List<Algorithm>> Gpu => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.Claymore,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Decred),
                    //new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Lbry),
                    //new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Pascal),
                    //new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Sia),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Blake2s),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Keccak)
                }
            },
            {
                MinerBaseType.Phoenix,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Phoenix, AlgorithmType.DaggerHashimoto)
                }
            },
            {
                MinerBaseType.GMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.GMiner, AlgorithmType.Beam), // BEAM added for NVIDIA and AMD check GPU section
                    //new Algorithm(MinerBaseType.GMiner, AlgorithmType.ZHash),
                    //new Algorithm(MinerBaseType.GMiner, AlgorithmType.GrinCuckaroo29)
                }
            },
        };

        #endregion

        #region CPU

        public static Dictionary<MinerBaseType, List<Algorithm>> Cpu => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            // {
            //     MinerBaseType.Xmrig,
            //     new List<Algorithm>
            //     {
            //         //new Algorithm(MinerBaseType.Xmrig, AlgorithmType.CryptoNight, ""),
            //         //new Algorithm(MinerBaseType.Xmrig, AlgorithmType.CryptoNightV7)
            //     }
            // },
            // {
            //     MinerBaseType.cpuminer,
            //     new List<Algorithm>
            //     {
            //         //new Algorithm(MinerBaseType.cpuminer, AlgorithmType.Lyra2z, "lyra2z"),
            //         //new Algorithm(MinerBaseType.cpuminer, AlgorithmType.MTP, "mtp")
            //     }
            // }
        }.ConcatDict(All);

        #endregion

        #region AMD

        private const string RemDis = " --remove-disabled";
        private const string DefaultParam = RemDis + AmdGpuDevice.DefaultParam;

        public static Dictionary<MinerBaseType, List<Algorithm>> Amd => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.sgminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.NeoScrypt, "neoscrypt", false)
                    {
                        ExtraLaunchParameters =
                            DefaultParam +
                            "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4"
                    },
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.DaggerHashimoto, "ethash")
                    {
                        ExtraLaunchParameters = RemDis + "--xintensity 512 -w 192 -g 1"
                    },
                    //new Algorithm(MinerBaseType.sgminer, AlgorithmType.Decred, "decred", false)
                    //{
                    //    ExtraLaunchParameters = RemDis + "--gpu-threads 1 --xintensity 256 --lookup-gap 2 --worksize 64"
                    //},
                    //new Algorithm(MinerBaseType.sgminer, AlgorithmType.Lbry, "lbry", false)
                    //{
                    //    ExtraLaunchParameters = DefaultParam + "--xintensity 512 --worksize 128 --gpu-threads 2"
                    //},
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Pascal, "pascal", false)
                    {
                        ExtraLaunchParameters = DefaultParam + "--intensity 21 -w 64 -g 2"
                    },
                    //new Algorithm(MinerBaseType.sgminer, AlgorithmType.X11Gost, "sibcoin-mod", false)
                    //{
                    //    ExtraLaunchParameters = DefaultParam + "--intensity 16 -w 64 -g 2"
                    //},
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Keccak, "keccak")
                    {
                        ExtraLaunchParameters = DefaultParam + "--intensity 15"
                    },
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.X16R, "x16r")
                    {
                        ExtraLaunchParameters = "-X 256"
                    },
                    //new Algorithm(MinerBaseType.sgminer, AlgorithmType.MTP, "mtp")
                    //{
                    //    ExtraLaunchParameters = "--worksize 256 --intensity 18"
                    //}
                }
            },
            {
                MinerBaseType.Claymore,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.Claymore, AlgorithmType.CryptoNightV7),
                    //new Algorithm(MinerBaseType.Claymore, AlgorithmType.Equihash, "equihash")
                }
            },
            //{
            //    MinerBaseType.OptiminerAMD,
            //    new List<Algorithm>
            //    {
            //        new Algorithm(MinerBaseType.OptiminerAMD, AlgorithmType.Equihash, "equihash")
            //    }
            //},
            {
                MinerBaseType.Prospector,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Prospector, AlgorithmType.Skunk, "sigt", false),
                    //new Algorithm(MinerBaseType.Prospector, AlgorithmType.Sia, "sia", false)
                }
            },
            {
                MinerBaseType.BMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.BMiner, AlgorithmType.Beam)
                }
            }
        }.ConcatDictList(All, Gpu);

        #endregion

        #region NVIDIA

        public static Dictionary<MinerBaseType, List<Algorithm>> Nvidia => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.ccminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.NeoScrypt, "neoscrypt"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lyra2REv2, "lyra2v2"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Decred, "decred", false),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lbry, "lbry", false),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.X11Gost, "sib", false),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Blake2s, "blake2s"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Sia, "sia", false),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Keccak, "keccak"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Skunk, "skunk"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lyra2z, "lyra2z"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.X16R, "x16r"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lyra2REv3, "lyra2v3"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.MTP, "mtp"),
                }
            },
            {
                MinerBaseType.ccminer_alexis,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.ccminer_alexis, AlgorithmType.X11Gost, "sib", false),
                    new Algorithm(MinerBaseType.ccminer_alexis, AlgorithmType.Keccak, "keccak")
                }
            },
            {
                MinerBaseType.ethminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.ethminer, AlgorithmType.DaggerHashimoto, "daggerhashimoto")
                }
            },
            //{
            //    MinerBaseType.nheqminer,
            //    new List<Algorithm>
            //    {
            //        new Algorithm(MinerBaseType.nheqminer, AlgorithmType.Equihash, "equihash")
            //    }
            //},
            {
                MinerBaseType.EWBF,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.EWBF, AlgorithmType.Equihash),
                    new Algorithm(MinerBaseType.EWBF, AlgorithmType.ZHash)
                }
            },
            //{
            //    MinerBaseType.dtsm,
            //    new List<Algorithm>
            //    {
            //        new Algorithm(MinerBaseType.dtsm, AlgorithmType.Equihash)
            //    }
            //},
            {
                MinerBaseType.trex,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.trex, AlgorithmType.Skunk, "skunk"),
                    //new Algorithm(MinerBaseType.trex, AlgorithmType.Lyra2z, "lyra2z"),
                    new Algorithm(MinerBaseType.trex, AlgorithmType.X16R, "x16r")
                }
            },
            {
                MinerBaseType.GMiner,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.GMiner, AlgorithmType.Beam), // BEAM added for NVIDIA and AMD check GPU section
                    new Algorithm(MinerBaseType.GMiner, AlgorithmType.ZHash),
                    new Algorithm(MinerBaseType.GMiner, AlgorithmType.GrinCuckaroo29),
                    //new Algorithm(MinerBaseType.GMiner, AlgorithmType.GrinCuckatoo31)
                }
            },
            {
                MinerBaseType.BMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.BMiner, AlgorithmType.ZHash),
                    new Algorithm(MinerBaseType.BMiner, AlgorithmType.DaggerHashimoto),
                    new Algorithm(MinerBaseType.BMiner, AlgorithmType.Beam),
                    //new Algorithm(MinerBaseType.BMiner, AlgorithmType.Equihash),
                    // TODO Only getting CUDA errors when dual mining
                    //new DualAlgorithm(MinerBaseType.BMiner, AlgorithmType.DaggerHashimoto, AlgorithmType.Blake2s)
                    new Algorithm(MinerBaseType.BMiner, AlgorithmType.GrinCuckaroo29)
                }
            },
            {
                MinerBaseType.TTMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.TTMiner, AlgorithmType.MTP),
                    new Algorithm(MinerBaseType.TTMiner, AlgorithmType.Lyra2REv3),
                }
            },
            {
                MinerBaseType.NBMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.NBMiner, AlgorithmType.GrinCuckaroo29),
                    new Algorithm(MinerBaseType.NBMiner, AlgorithmType.GrinCuckatoo31)
                }
            }
            
        }.ConcatDictList(All, Gpu);

        #endregion
    }
}
