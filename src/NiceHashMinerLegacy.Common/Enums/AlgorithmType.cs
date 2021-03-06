﻿namespace NiceHashMinerLegacy.Common.Enums
{
    /// <summary>
    /// AlgorithmType enum should/must mirror the values from https://www.nicehash.com/?p=api
    /// Some algorithms are not used anymore on the client, rename them with _UNUSED postfix so we can catch compile time errors if they are used.
    /// </summary>
    public enum AlgorithmType
    {
        // dual algos for grouping
        DaggerKeccak = -8,
        DaggerBlake2s = -7,
        //DaggerSia = -6,
        DaggerDecred = -5,
        //DaggerLbry = -4,
        //DaggerPascal = -3,
        INVALID = -2,
        NONE = -1,
        #region NiceHashAPI
        //Scrypt_UNUSED = 0,
        //SHA256_UNUSED = 1,
        //ScryptNf_UNUSED = 2,
        //X11_UNUSED = 3,
        //X13 = 4,
        Keccak = 5,
        //X15 = 6,
        Nist5_UNUSED = 7,
        //NeoScrypt = 8,
        //Lyra2RE = 9,
        //WhirlpoolX = 10,
        //Qubit = 11,
        //Quark = 12,
        //Axiom_UNUSED = 13,
        //Lyra2REv2 = 14,
        //ScryptJaneNf16_UNUSED = 15,
        //Blake256r8 = 16,
        //Blake256r14 = 17, // NOT USED ANYMORE?
        //Blake256r8vnl = 18,
        //Hodl = 19,
        //DaggerHashimoto = 20,
        //Decred = 21,
        //CryptoNight = 22,
        //Lbry = 23,
        //Equihash = 24,
        //Pascal = 25
        // UNUSED START
        Scrypt_UNUSED = 0,
        SHA256_UNUSED = 1,
        ScryptNf_UNUSED = 2,
        X11_UNUSED = 3,
        X13_UNUSED = 4,
        //Keccak_UNUSED = 5,
        X15_UNUSED = 6,
        //Nist5_UNUSED = 7,

        WhirlpoolX_UNUSED = 10,
        Qubit_UNUSED = 11,
        Quark_UNUSED = 12,
        Axiom_UNUSED = 13,

        ScryptJaneNf16_UNUSED = 15,
        Blake256r8_UNUSED = 16,
        Blake256r14_UNUSED = 17,
        Blake256r8vnl_UNUSED = 18,
        // UNUSED END

        NeoScrypt = 8,
        Lyra2RE_UNUSED = 9,

        Lyra2REv2_UNUSED = 14,

        Hodl = 19,
        DaggerHashimoto = 20,
        Decred = 21,
        CryptoNight_UNUSED = 22,
        Lbry_UNUSED = 23,
        Equihash_UNUSED = 24,
        Pascal = 25,
        X11Gost_UNUSED = 26,
        Sia_UNUSED = 27,
        Blake2s = 28,
        Skunk = 29,
        CryptoNightV7_UNUSED = 30,
        CryptoNightHeavy = 31,
        Lyra2z_UNUSED = 32,
        X16R = 33,
        CryptoNightV8 = 34,
        ZHash = 36,
        Beam = 37,
        // Tentative
        GrinCuckaroo29 = 38,
        GrinCuckatoo31 = 39,
        Lyra2REv3 = 40,
        MTP = 41,
        CryptoNightR = 42,
        #endregion // NiceHashAPI
    }
}
